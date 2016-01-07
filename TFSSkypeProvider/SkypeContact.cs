using System;
using System.Diagnostics;
using Microsoft.TeamFoundation.Collaboration;
using SKYPE4COMLib;

namespace TfsCommunity.Collaboration.Skype
{
    /// <summary>
    /// Implements a <see cref="Contact"/> for skype
    /// </summary>
    internal class SkypeContact : ContactBase
    {
        #region Overrides of Contact

        private readonly string contactID;
        private readonly string friendlyName;
        private bool isBlocked;
        private readonly bool isSelf;
        private readonly PhoneNumberCollection phoneNumberCollection;
        private string statusDetail;
        private bool isTagged;
        private PresenceStatus presenceStatus;
        private ISkype skypeInstance;
        private IUser skypeUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkypeContact"/> class.
        /// </summary>
        /// <param name="skypeContact">The skype contact.</param>
        /// <param name="skypeInstance">The skype instance.</param>
        /// <param name="mapping">The user mapping for this contact</param>
        public SkypeContact(IUser skypeContact, ISkype skypeInstance, UserMapping mapping)
        {
            this.skypeInstance = skypeInstance;
            this.skypeUser = skypeContact;
            // Do not access skype if it is not running
            if ((!IsSkypeRunning()) || (skypeContact == null))
            {
                contactID = mapping.SkypeName;
                friendlyName = mapping.SkypeName;
                presenceStatus = PresenceStatus.Offline;
                statusDetail = string.Empty;
                isBlocked = false;
                isTagged = false;
            }
            else if ((mapping != null) && (!mapping.IsIgnored))
            {
                contactID = skypeContact.Handle;
                friendlyName = skypeContact.FullName;
                presenceStatus = skypeContact.OnlineStatus.OnlineStatusToPresenceStatus();
                statusDetail = skypeContact.MoodText;
                isBlocked = skypeContact.IsBlocked;
                isTagged = false;
                if (skypeContact.Handle == skypeInstance.CurrentUser.Handle)
                {
                    isSelf = true;
                }
                phoneNumberCollection = new SkypePhoneCollection(skypeContact);
            }
            else if (mapping != null)
            {
                friendlyName = mapping.SkypeName;
                contactID = mapping.TFsName;
                statusDetail = Properties.Resources.UserNotMapped;
            }
        }



        /// <summary>
        /// Gets the contact id.
        /// </summary>
        /// <value>The contact id.</value>
        public override string ContactId
        {
            get { return contactID; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance matches the user running the application.
        /// </summary>
        /// <value><c>true</c> if this instance represents the current user; otherwise, <c>false</c>.</value>
        public override bool IsSelf
        {
            get { return isSelf; }
        }

        /// <summary>
        /// Gets the friendly name of the instance.
        /// </summary>
        /// <value>The friendly name.</value>
        public override string FriendlyName
        {
            get { return friendlyName; }
        }

        /// <summary>
        /// Gets the phone numbers.
        /// </summary>
        /// <value>The phone numbers.</value>
        public override PhoneNumberCollection PhoneNumbers
        {
            get { return phoneNumberCollection; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is blocked.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is blocked; otherwise, <c>false</c>.
        /// </value>
        public override bool IsBlocked
        {
            get { return isBlocked; }
        }

        /// <summary>
        /// Gets the presence status.
        /// </summary>
        /// <value>The status.</value>
        public override PresenceStatus Status
        {
            get { return presenceStatus; }
        }

        /// <summary>
        /// Gets the status detail.
        /// </summary>
        /// <value>The status detail.</value>
        public override string StatusDetail
        {
            get { return statusDetail; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is tagged.
        /// </summary>
        /// <value><c>true</c> if this instance is tagged; otherwise, <c>false</c>.</value>
        public override bool IsTagged
        {
            get
            {
                return isTagged;
            }
            set
            {
                isTagged = value;
            }

        }

        private bool IsSkypeRunning()
        {
            return ((skypeInstance != null) && (skypeInstance.Client.IsRunning));
        }


        /// <summary>
        /// Updates the presence status.
        /// </summary>
        /// <param name="e">The <see cref="Microsoft.TeamFoundation.Collaboration.PresenceChangedEventArgs"/> instance containing the event data.</param>
        public override void UpdateStatus(PresenceChangedEventArgs e)
        {
            presenceStatus = e.Status;
            statusDetail = skypeUser.MoodText;
            isBlocked = e.IsBlocked;
        }

        #endregion
    }
}