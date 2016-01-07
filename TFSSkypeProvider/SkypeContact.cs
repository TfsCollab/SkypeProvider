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

        private readonly string _contactId;
        private readonly string _friendlyName;
        private bool _isBlocked;
        private readonly bool _isSelf;
        private readonly PhoneNumberCollection _phoneNumberCollection;
        private string _statusDetail;
        private bool _isTagged;
        private PresenceStatus _presenceStatus;
        private readonly ISkype _skypeInstance;
        private readonly IUser _skypeUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkypeContact"/> class.
        /// </summary>
        /// <param name="skypeContact">The skype contact.</param>
        /// <param name="skypeInstance">The skype instance.</param>
        /// <param name="mapping">The user mapping for this contact</param>
        public SkypeContact(IUser skypeContact, ISkype skypeInstance, UserMapping mapping)
        {
            if (skypeInstance != null) _skypeInstance = skypeInstance;
            _skypeUser = skypeContact;
            // Do not access skype if it is not running
            if ((!IsSkypeRunning()) || (skypeContact == null))
            {
                _contactId = mapping.SkypeName;
                _friendlyName = mapping.SkypeName;
                _presenceStatus = PresenceStatus.Offline;
                _statusDetail = string.Empty;
                _isBlocked = false;
                _isTagged = false;
            }
            else if ((mapping != null) && (!mapping.IsIgnored))
            {
                _contactId = skypeContact.Handle;
                _friendlyName = skypeContact.FullName;
                _presenceStatus = skypeContact.OnlineStatus.OnlineStatusToPresenceStatus();
                _statusDetail = skypeContact.MoodText;
                _isBlocked = skypeContact.IsBlocked;
                _isTagged = false;
                if (skypeInstance != null)
                    if (skypeContact.Handle == skypeInstance.CurrentUser.Handle)
                    {
                        _isSelf = true;
                    }
                _phoneNumberCollection = new SkypePhoneCollection(skypeContact);
            }
            else if (mapping != null)
            {
                _friendlyName = mapping.SkypeName;
                _contactId = mapping.TfsName;
                _statusDetail = Properties.Resources.UserNotMapped;
            }
        }



        /// <summary>
        /// Gets the contact id.
        /// </summary>
        /// <value>The contact id.</value>
        public override string ContactId
        {
            get { return _contactId; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance matches the user running the application.
        /// </summary>
        /// <value><c>true</c> if this instance represents the current user; otherwise, <c>false</c>.</value>
        public override bool IsSelf
        {
            get { return _isSelf; }
        }

        /// <summary>
        /// Gets the friendly name of the instance.
        /// </summary>
        /// <value>The friendly name.</value>
        public override string FriendlyName
        {
            get { return _friendlyName; }
        }

        /// <summary>
        /// Gets the phone numbers.
        /// </summary>
        /// <value>The phone numbers.</value>
        public override PhoneNumberCollection PhoneNumbers
        {
            get { return _phoneNumberCollection; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is blocked.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is blocked; otherwise, <c>false</c>.
        /// </value>
        public override bool IsBlocked
        {
            get { return _isBlocked; }
        }

        /// <summary>
        /// Gets the presence status.
        /// </summary>
        /// <value>The status.</value>
        public override PresenceStatus Status
        {
            get { return _presenceStatus; }
        }

        /// <summary>
        /// Gets the status detail.
        /// </summary>
        /// <value>The status detail.</value>
        public override string StatusDetail
        {
            get { return _statusDetail; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is tagged.
        /// </summary>
        /// <value><c>true</c> if this instance is tagged; otherwise, <c>false</c>.</value>
        public override bool IsTagged
        {
            get
            {
                return _isTagged;
            }
            set
            {
                _isTagged = value;
            }

        }

        private bool IsSkypeRunning()
        {
            return ((_skypeInstance != null) && (_skypeInstance.Client.IsRunning));
        }


        /// <summary>
        /// Updates the presence status.
        /// </summary>
        /// <param name="e">The <see cref="Microsoft.TeamFoundation.Collaboration.PresenceChangedEventArgs"/> instance containing the event data.</param>
        public override void UpdateStatus(PresenceChangedEventArgs e)
        {
            _presenceStatus = e.Status;
            _statusDetail = _skypeUser.MoodText;
            _isBlocked = e.IsBlocked;
        }

        #endregion
    }
}