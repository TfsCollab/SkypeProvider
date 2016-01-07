using System.Collections.Generic;
using System.Diagnostics;
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

        private readonly string groupName;
        private readonly ISkype skypeInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkypeContactGroup"/> class.
        /// </summary>
        /// <param name="SkypeInstance">The skype instance.</param>
        /// <param name="GroupName">Name of the group.</param>
        public SkypeContactGroup(ISkype SkypeInstance, string GroupName)
        {
            groupName = GroupName;
            skypeInstance = SkypeInstance;
        }

        /// <summary>
        /// Gets the name of the group.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return groupName; }
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
                foreach (IGroup Group in skypeInstance.Groups)
                {
                    if (Group.DisplayName.Equals(groupName))
                    {
                        foreach (IUser skypeUser in Group.Users)
                        {
                            var mapping = mappings.FindBySkypeName(skypeUser.Handle);
                            contactsCollection.Add(new SkypeContact(skypeUser, skypeInstance, mapping));
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