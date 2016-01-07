using System.Collections.Generic;
using Microsoft.TeamFoundation.Collaboration;
using SKYPE4COMLib;

namespace TfsCommunity.Collaboration.Skype
{
    /// <summary>
    /// Implements a <see cref="IContactGroup"/> for Skype
    /// </summary>
    public class SkypeContactGroup : IContactGroup
    {
        #region Implementation of IContactGroup

        private readonly string _groupName;
        private readonly ISkype _skypeInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkypeContactGroup"/> class.
        /// </summary>
        /// <param name="skypeInstance">The skype instance.</param>
        /// <param name="groupName">Name of the group.</param>
        public SkypeContactGroup(ISkype skypeInstance, string groupName)
        {
            _groupName = groupName;
            _skypeInstance = skypeInstance;
        }

        /// <summary>
        /// Gets the name of the group.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return _groupName; }
        }

        /// <summary>
        /// Gets the contacts belonging to the group.
        /// </summary>
        /// <value>The contacts.</value>
        public ICollection<Contact> Contacts
        {
            get
            {
                ICollection<Contact> contactsCollection = new List<Contact>();
                UserMappingCollection mappings = new UserMappingCollection();
                mappings.Load();
                foreach (IGroup group in _skypeInstance.Groups)
                {
                    if (group.DisplayName.Equals(_groupName))
                    {
                        foreach (IUser skypeUser in group.Users)
                        {
                            var mapping = mappings.FindBySkypeName(skypeUser.Handle);
                            contactsCollection.Add(new SkypeContact(skypeUser, _skypeInstance, mapping));
                        }
                        return contactsCollection;
                    }
                }
                return null;
            }
        }

        #endregion
    }
}