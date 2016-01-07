using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace TfsCommunity.Collaboration.Skype
{
    /// <summary>
    /// Gets the user mappings
    /// </summary>
    public class UserMappingCollection : KeyedCollection<string, UserMapping>
    {
        /// <summary>
        /// Gets the mapping file path.
        /// </summary>
        /// <value>The mapping file path.</value>
        public string MappingFilePath
        {
            get
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                path = Path.Combine(path, "TfsCommunity");
                path = Path.Combine(path, "Collaboration");
                path = Path.Combine(path, "Skype");
                path = Path.Combine(path, "SkypeUserMappings.xml");
                return path;
            }
        }


        /// <summary>
        /// Loads the values for this instance from the file system.
        /// </summary>
        public void Load()
        {
            if (File.Exists(MappingFilePath))
            {
                var serializer = new XmlSerializer(typeof (UserMappingCollection), new XmlRootAttribute("UserMappings"));
                using (Stream stream = File.OpenRead(MappingFilePath))
                {
                    var userMappings = serializer.Deserialize(stream) as UserMappingCollection;
                    foreach (UserMapping mapping in userMappings)
                    {
                        Add(mapping);
                    }
                }
            }
        }

        /// <summary>
        /// Saves this instance to the file system.
        /// </summary>
        public void Save()
        {
            if (!File.Exists(MappingFilePath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(MappingFilePath));
            }
            var serializer = new XmlSerializer(typeof (UserMappingCollection), new XmlRootAttribute("UserMappings"));
            using (Stream stream = File.OpenWrite(MappingFilePath))
            {
                serializer.Serialize(stream, this);
            }
        }

        #region Overrides of KeyedCollection<string,UserMapping>

        /// <summary>
        /// When implemented in a derived class, extracts the key from the specified element.
        /// </summary>
        /// <returns>
        /// The key for the specified element.
        /// </returns>
        /// <param name="item">
        /// The element from which to extract the key.
        /// </param>
        protected override string GetKeyForItem(UserMapping item)
        {
            return item.TFsName;
        }

        #endregion

        /// <summary>
        /// Finds the user mapping using the skype name.
        /// </summary>
        /// <param name="skypeName">Skype name of the user.</param>
        /// <returns>The user mapping</returns>
        public UserMapping FindBySkypeName(string skypeName)
        {
            foreach (UserMapping mapping in this)
            {
                if (mapping.SkypeName.Equals(skypeName))
                    return mapping;
            }
            return null;
        }

        /// <summary>
        /// Gets the user mapping.
        /// </summary>
        /// <param name="contactId">The contact id.</param>
        /// <returns></returns>
        public UserMapping GetUserMapping(string contactId)
        {
            var mapping = this.FindBySkypeName(contactId);
            if (mapping == null)
            {
                if (this.Contains(contactId) && (!this[contactId].IsIgnored))
                    mapping = this[contactId];
            }
            return mapping;
        }
    }
}