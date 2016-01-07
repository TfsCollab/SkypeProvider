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
            this.contactId = contactId;
            this.isBlocked = isBlocked;
            this.status = status;
            this.statusDetail = statusDetail;
        }

        public SkypePresenceChangedEventArgs(IUser skypeContact, TOnlineStatus onlineStatus, string tFSContactId)
        {
            contactId = skypeContact.Handle;
            isBlocked = skypeContact.IsBlocked;
            status = onlineStatus.OnlineStatusToPresenceStatus();
            statusDetail = skypeContact.MoodText;
        }

        #region Overrides of PresenceChangedEventArgs

        private readonly string contactId;

        private readonly bool isBlocked;

        private readonly PresenceStatus status;

        private readonly string statusDetail;

        /// <summary>
        /// Gets the contact id.
        /// </summary>
        /// <value>The contact id.</value>
        public override string ContactId
        {
            get { return contactId; }
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
        /// Gets the status.
        /// </summary>
        /// <value>The status.</value>
        public override PresenceStatus Status
        {
            get { return status; }
        }

        /// <summary>
        /// Gets the status detail.
        /// </summary>
        /// <value>The status detail.</value>
        public override string StatusDetail
        {
            get { return statusDetail; }
        }

        #endregion
    }
}