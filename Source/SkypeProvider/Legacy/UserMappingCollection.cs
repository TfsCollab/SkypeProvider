﻿#region

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

#endregion

namespace TfsCommunity.Collaboration.Skype.Legacy
{
    /// <summary>
    ///   Gets the (legacy) user mappings (2008 + 2010 skype provider)
    /// </summary>
    public class UserMappingCollection : KeyedCollection<string, UserMapping>
    {
        /// <summary>
        ///   Gets the mapping file path.
        /// </summary>
        /// <value> The mapping file path. </value>
        public string MappingFilePath
        {
            get
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                path = Path.Combine(path, Resources.Resources.SkypeMappingFileFolder2010);
                return path;
            }
        }


        /// <summary>
        ///   Loads the values for this instance from the file system.
        /// </summary>
        public void Load()
        {
            if (File.Exists(MappingFilePath))
            {
                var serializer = new XmlSerializer(typeof (UserMappingCollection), new XmlRootAttribute("UserMappings"));
                using (Stream stream = File.OpenRead(MappingFilePath))
                {
                    var userMappings = serializer.Deserialize(stream) as UserMappingCollection;
                    if (userMappings != null)
                        foreach (UserMapping mapping in userMappings)
                        {
                            Add(mapping);
                        }
                }
            }
        }

        /// <summary>
        ///   Saves this instance to the file system.
        /// </summary>
        public void Save()
        {
            if (MappingFilePath != null && !File.Exists(MappingFilePath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(MappingFilePath));
            }
            var serializer = new XmlSerializer(typeof (UserMappingCollection), new XmlRootAttribute("UserMappings"));
            using (Stream stream = File.OpenWrite(MappingFilePath))
            {
                serializer.Serialize(stream, this);
                //Logger.Write(string.Format("Saved mapping file. Output path is {0}.", MappingFilePath));
            }
        }

        /// <summary>
        ///   Finds the user mapping using the skype name.
        /// </summary>
        /// <param name="skypeName"> Skype name of the user. </param>
        /// <returns> The user mapping </returns>
        public UserMapping FindBySkypeName(string skypeName)
        {
            foreach (UserMapping mapping in this)
            {
                if (mapping.SkypeName.Equals(skypeName, StringComparison.OrdinalIgnoreCase))
                    return mapping;
            }
            return null;
        }

        /// <summary>
        ///   Gets the user mapping.
        /// </summary>
        /// <param name="contactId"> The contact id. </param>
        /// <returns> </returns>
        public UserMapping GetUserMapping(string contactId)
        {
            var mapping = FindBySkypeName(contactId);
            if (mapping == null)
            {
                if (Contains(contactId) && (!this[contactId].IsIgnored))
                    mapping = this[contactId];
            }

            if (mapping != null)
            {
                //Logger.Write(string.Format("Found a mapping for contactid {0} / tfs name {1}. Skype name is {2}.",contactId,mapping.TfsName, mapping.SkypeName));
                return mapping;
            }
            return null;
        }

        #region Overrides of KeyedCollection<string,UserMapping>

        /// <summary>
        ///   When implemented in a derived class, extracts the key from the specified element.
        /// </summary>
        /// <returns> The key for the specified element. </returns>
        /// <param name="item"> The element from which to extract the key. </param>
        protected override string GetKeyForItem(UserMapping item)
        {
            return item.TfsName;
        }

        #endregion
    }
}