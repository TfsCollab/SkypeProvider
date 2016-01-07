using System;
using Microsoft.TeamFoundation.Collaboration;
using SKYPE4COMLib;

namespace TfsCommunity.Collaboration.Skype
{
    /// <summary>
    /// Event arguments for Presence changes
    /// </summary>
    public class SkypePresenceChangedEventArgs : PresenceChangedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkypePresenceChangedEventArgs"/> class.
        /// </summary>
        /// <param name="contactId">The contact id.</param>
        /// <param name="isBlocked">if set to <c>true</c> [is blocked].</param>
        /// <param name="status">The status.</param>
        /// <param name="statusDetail">The status detail.</param>
        [Obsolete]
        public SkypePresenceChangedEventArgs(string contactId, bool isBlocked, PresenceStatus status, string statusDetail)
        {
            _contactId = contactId;
            _isBlocked = isBlocked;
            _status = status;
            _statusDetail = statusDetail;
        }

        public SkypePresenceChangedEventArgs(IUser skypeContact, TOnlineStatus onlineStatus, string tfsContactId)
        {
            _contactId = skypeContact.Handle;
            _isBlocked = skypeContact.IsBlocked;
            _status = onlineStatus.OnlineStatusToPresenceStatus();
            _statusDetail = skypeContact.MoodText;
            _contactId = tfsContactId;
        }

        #region Overrides of PresenceChangedEventArgs

        private readonly string _contactId;

        private readonly bool _isBlocked;

        private readonly PresenceStatus _status;

        private readonly string _statusDetail;

        /// <summary>
        /// Gets the contact id.
        /// </summary>
        /// <value>The contact id.</value>
        public override string ContactId
        {
            get { return _contactId; }
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
        /// Gets the status.
        /// </summary>
        /// <value>The status.</value>
        public override PresenceStatus Status
        {
            get { return _status; }
        }

        /// <summary>
        /// Gets the status detail.
        /// </summary>
        /// <value>The status detail.</value>
        public override string StatusDetail
        {
            get { return _statusDetail; }
        }

        #endregion
    }
}