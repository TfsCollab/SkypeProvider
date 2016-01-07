using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using Microsoft.TeamFoundation.Collaboration;
using SKYPE4COMLib;

namespace TfsCommunity.Collaboration.Skype
{
    [CollaborationProvider("Skype Provider powered by AIT TeamSystemPro")]
    public class SkypeProvider : CollaborationProviderBase
    {
        #region Fields

        private readonly List<string> _requestedForCreation = new List<string>();
        private UserMappingCollection _mappings;
        private ISkype _skypeInstance;

        #endregion

        #region Constants

        private const short MaxSkypeCallParticipants = 4;
        private const int VideoWaitTime = 1000;

        #endregion

        #region Implementation of IDisposable

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _skypeInstance = null;
        }

        #endregion

        #region Collaboration Provider overrides

        /// <summary>
        /// Gets the provider readiness.
        /// </summary>
        /// <value>The provider readiness.</value>
        public override ProviderReadiness ProviderReadiness
        {
            get
            {
                // try to Access Skype
                try
                {
                    return SkypeInstance.Client.IsRunning ? ProviderReadiness.Running : ProviderReadiness.Unknown;
                }
                catch (Exception)
                {
                    return ProviderReadiness.NotInstalled;
                }

            }
        }

        /// <summary>
        /// Gets the sign in status.
        /// </summary>
        /// <value>The sign in status.</value>
        /// <exception cref="ArgumentOutOfRangeException"><c></c> is out of range.</exception>
        public override SignInStatus SignInStatus
        {
            get
            {
                SignInStatus status = SignInStatus.NotSignedIn;
                if (SkypeInstance != null)
                {
                    // Do not access skype if it is not running
                    if (!IsSkypeRunning())
                    {
                        status = SignInStatus.NotSignedIn;
                    }
                    else
                    {
                        try
                        {
                            switch (SkypeInstance.CurrentUserStatus)
                            {
                                case TUserStatus.cusOnline:
                                    status = SignInStatus.SignedIn;
                                    break;
                                case TUserStatus.cusAway:
                                    status = SignInStatus.SignedIn;
                                    break;
                                case TUserStatus.cusDoNotDisturb:
                                    status = SignInStatus.SignedIn;
                                    break;
                                case TUserStatus.cusInvisible:
                                    status = SignInStatus.SignedIn;
                                    break;
                                case TUserStatus.cusNotAvailable:
                                    status = SignInStatus.SignedIn;
                                    break;
                                case TUserStatus.cusSkypeMe:
                                    status = SignInStatus.SignedIn;
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteExceptionDetails("SignInStatus",ex);
                        }
                    }
                }
                return status;
            }
        }

        private bool IsSkypeRunning()
        {
            return ((SkypeInstance != null) && (SkypeInstance.Client.IsRunning));
        }

        /// <summary>
        /// Gets my contact id.
        /// </summary>
        /// <value>My contact id.</value>
        public override string MyContactId
        {
            get
            {
                // Do not access skype if it is not running
                return (!IsSkypeRunning())
                           ? string.Empty
                           : SkypeInstance.CurrentUser.Handle;
            }
        }

        /// <summary>
        /// Gets the name of my friendly.
        /// </summary>
        /// <value>The name of my friendly.</value>
        public override string MyFriendlyName
        {
            get
            {
                // Do not access skype if it is not running
                return (!IsSkypeRunning())
                           ? string.Empty
                           : SkypeInstance.CurrentUser.DisplayName;
            }
        }

        /// <summary>
        /// Gets my status.
        /// </summary>
        /// <value>My status.</value>
        public override PresenceStatus MyStatus
        {
            get
            {
                // Do not access skype if it is not running
                try
                {
                    return (!IsSkypeRunning())
                           ? SkypeInstance.CurrentUser.OnlineStatus.OnlineStatusToPresenceStatus()
                           : PresenceStatus.Offline;
                }
                catch { return PresenceStatus.Offline; }
            }
        }

        /// <summary>
        /// Gets my status text.
        /// </summary>
        /// <value>My status text.</value>
        public override string MyStatusText
        {
            get
            {
                // Do not access skype if it is not running
                return (IsSkypeRunning())
                           ? SkypeInstance.CurrentUser.MoodText
                           : string.Empty;
            }
        }

        /// <summary>
        /// Determines whether the specified capability is a supported capability.
        /// </summary>
        /// <param name="capability">The capability.</param>
        /// <returns>
        /// 	<c>true</c> if the specified capability is a supported capability; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"><c>capability</c> is out of range.</exception>
        public override bool IsSupportedCapability(CollaborationCapability capability)
        {
            bool isSupported = false;
            switch (capability)
            {
                case CollaborationCapability.EnumerateContactGroupContents:
                    break;
                case CollaborationCapability.InstantMessageConversation_Multiparty:
                case CollaborationCapability.InstantMessageConversation:
                case CollaborationCapability.AudioConversation:
                case CollaborationCapability.AudioConversation_Multiparty:
                case CollaborationCapability.VideoConversation:
                case CollaborationCapability.AddContact:
                case CollaborationCapability.SendInstantMessageText:
                    isSupported = true;
                    break;
                case CollaborationCapability.VideoConversation_Multiparty:
                    break;
                case CollaborationCapability.PhoneConversation:
                    break;
                case CollaborationCapability.PhoneConversation_Multiparty:
                    break;
                case CollaborationCapability.LiveMeetingConversation:
                    break;
                case CollaborationCapability.LiveMeetingConversation_Multiparty:
                    break;
                case CollaborationCapability.PstnConversation:
                    break;
                case CollaborationCapability.PstnConversation_MultiParty:
                    break;
                case CollaborationCapability.Tagging:
                    break;
                case CollaborationCapability.CreateMailMessage:
                    break;
                case CollaborationCapability.EditMailMessage:
                    break;
                case CollaborationCapability.MailCcRecpients:
                    break;
                case CollaborationCapability.MailBccRecipients:
                    break;
                case CollaborationCapability.MailAttachments:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("capability");
            }
            return isSupported;
        }

        /// <summary>
        /// Performs auto sign in.
        /// </summary>
        /// <returns>The <see cref="SignInStatus"/> after performing the auto sign in</returns>
        public override SignInStatus AutoSignIn()
        {
            if (!SkypeInstance.Client.IsRunning)
                SkypeInstance.Client.Start(true, true);
            return SignInStatus;
        }

        /// <summary>
        /// Performs sign in.
        /// </summary>
        /// <param name="hwnd">The handle to use for sign in.</param>
        /// <param name="signInName">Name of the user to sign in.</param>
        /// <param name="password">The password.</param>
        /// <returns>The <see cref="SignInStatus"/> after performing the sign in</returns>
        public override SignInStatus SignIn(IntPtr hwnd, string signInName, string password)
        {
            // We do not allow signing in from here. We only return the current sign in status
            return SignInStatus;
        }

        /// <summary>
        /// Gets the status text for status.
        /// </summary>
        /// <param name="isBlocked">if set to <c>true</c> [is blocked].</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public override string GetStatusTextForStatus(bool isBlocked, PresenceStatus status)
        {
            // Do not access skype if it is not running
            if ((!IsSkypeRunning()))
                return string.Empty;

            return SkypeInstance.Convert.OnlineStatusToText(status.PresenceStatusToOnlineStatus());
        }

        #region Conversations

        /// <summary>
        /// Starts the instant message conversation.
        /// </summary>
        /// <param name="contactIds">The contact ids.</param>
        /// <returns></returns>
        public override IConversation StartInstantMessageConversation(IList<string> contactIds)
        {
            if (contactIds == null)
                return null;
            if (contactIds.Count == 0)
                return null;
            // Note: If there is only one conatc we use a single chat,. otherwise we start a multi user chat
            if (contactIds.Count == 1)
            {
                Logger.Write(string.Format("Starting im conversation with contactid {0}.",contactIds[0]));
                return StartInstantMessageConversation(contactIds[0]);
            }


            var chatMembers = new UserCollection();
            chatMembers.AddFromContactIds(contactIds, UserMappings, MyContactId);
            Logger.Write(string.Format("Starting im conversation with contactids {0}.", contactIds));

            // ChatWithMultiple is a little bit buggy
            // we first open a single chat and than adding the othters
            var mapping = UserMappings.GetUserMapping(contactIds[0]);
            if (mapping != null)
            {
                IChat skypeChat = SkypeInstance.CreateChatWith(mapping.SkypeName);
                IConversation conversation = new SkypeConversation(skypeChat) { Skype = SkypeInstance };
                skypeChat.AcceptAdd();
                skypeChat.AddMembers(chatMembers);
                skypeChat.OpenWindow();
                return conversation;
            }
            return null;
        }

        /// <summary>
        /// Starts the instant message conversation.
        /// </summary>
        /// <param name="contactId">The contact id.</param>
        /// <returns></returns>
        public override IConversation StartInstantMessageConversation(string contactId)
        {
            var mapping = UserMappings.GetUserMapping(contactId);
            if (mapping != null)
            {
                IChat skypeChat = SkypeInstance.CreateChatWith(mapping.SkypeName);
                IConversation conversation = new SkypeConversation(skypeChat) { Skype = SkypeInstance };
                skypeChat.OpenWindow();
                return conversation;
            }
            return null;
        }



        /// <summary>
        /// Starts the audio conversation.
        /// </summary>
        /// <param name="contactIds">The contact ids.</param>
        /// <returns></returns>
        public override IConversation StartAudioConversation(IList<string> contactIds)
        {
            if (contactIds == null)
                return null;
            if (contactIds.Count == 0)
                return null;
            // Note: If there is only one contact we use a single chat,. otherwise we start a multi user chat
            if (contactIds.Count == 1)
                return StartAudioConversation(contactIds[0]);
            // Do not access skype if it is not running
            if ((!IsSkypeRunning()))
                return null;
            // Note: Skype supports calls with up to 5 parties:
            // Get the parties from the list and start the calls
            var participants = new string[MaxSkypeCallParticipants];
            for (int i = 0; i < MaxSkypeCallParticipants; i++)
            {
                if (i < contactIds.Count)
                {
                    var mapping = UserMappings.GetUserMapping(contactIds[i]);
                    if (mapping != null)
                    {
                        IUser skypeUser = SkypeInstance.User[mapping.SkypeName];
                        participants[i] = skypeUser != null ? skypeUser.Handle : null;
                    }
                }
            }
            ICall skypeCall = SkypeInstance.PlaceCall(participants[0], participants[1], participants[2], participants[3]);
            IConversation conversation = new SkypeConversation(skypeCall) { Skype = SkypeInstance };
            return conversation;
        }

        /// <summary>
        /// Starts the audio conversation.
        /// </summary>
        /// <param name="contactId">The contact id.</param>
        /// <returns></returns>
        public override IConversation StartAudioConversation(string contactId)
        {
            // Do not access skype if it is not running
            if ((!IsSkypeRunning()))
                return null;
            var mapping = UserMappings.GetUserMapping(contactId);
            if (mapping != null)
            {
                IUser skypeUser = SkypeInstance.User[mapping.SkypeName];
                ICall skypeCall = SkypeInstance.PlaceCall(skypeUser.Handle, null, null, null);
                IConversation conversation = new SkypeConversation(skypeCall) { Skype = SkypeInstance };
                return conversation;
            }
            return null;
        }

        /// <summary>
        /// Starts the video conversation.
        /// </summary>
        /// <param name="contactId">The contact id.</param>
        /// <returns></returns>
        public override IConversation StartVideoConversation(string contactId)
        {
            var conversation = StartAudioConversation(contactId) as SkypeConversation;
            if (conversation != null)
            {
                if (conversation.SkypeCall != null)
                {
                    var thread = new Thread(AsyncEnableVideo) { IsBackground = true, Name = "SkypeVideoEnabler" };
                    thread.Start(conversation.SkypeCall);
                }
            }
            return conversation;
        }

        /// <summary>
        /// Starts the video conversation.
        /// </summary>
        /// <param name="contactIds">The contact ids.</param>
        /// <returns></returns>
        public override IConversation StartVideoConversation(IList<string> contactIds)
        {
            var conversation = StartAudioConversation(contactIds) as SkypeConversation;
            if (conversation != null)
            {
                if (conversation.SkypeCall != null)
                {
                    var thread = new Thread(AsyncEnableVideo) { IsBackground = true, Name = "SkypeVideoEnabler" };
                    thread.Start(conversation.SkypeCall);
                }
            }
            return conversation;
        }

        /// <summary>
        /// Enables video on skype calls once the call is running.
        /// </summary>
        /// <param name="threadStartParameter">The thread start parameter.</param>
        private void AsyncEnableVideo(object threadStartParameter)
        {
            var call = threadStartParameter as ICall;
            if (call == null)
                return;
            while (call.Status != TCallStatus.clsInProgress)
            {
                // if the call is canceled. cancel waiting too
                if ((call.Status == TCallStatus.clsFailed) ||
                    (call.Status == TCallStatus.clsRefused) ||
                    (call.Status == TCallStatus.clsCancelled) ||
                    (call.Status == TCallStatus.clsFinished) ||
                    (call.Status == TCallStatus.clsBusy))
                {
                    return;
                }
                Thread.Sleep(VideoWaitTime);
            }

            // Once we established a call we start the video
            try
            {
                call.StartVideoSend();
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionDetails("AsyncEnableVideo", ex);
            }
        }

        /// <summary>
        /// Gets the active conversations.
        /// </summary>
        /// <value>The active conversations.</value>
        public override ICollection<IConversation> ActiveConversations
        {
            get
            {
                var conversations = new BindingList<IConversation>();
                // Do not access skype if it is not running
                if ((!IsSkypeRunning()))
                    return conversations;
                foreach (IChat activeChat in SkypeInstance.ActiveChats)
                {
                    IConversation conversation = new SkypeConversation(activeChat) { Skype = SkypeInstance };
                    conversations.Add(conversation);
                }
                return conversations;
            }
        }

        /// <summary>
        /// Gets the skype instance.
        /// </summary>
        /// <value>The skype instance.</value>
        public ISkype SkypeInstance
        {
            get
            {
                // Upon first access we ensure to create the COM instance
                if (_skypeInstance == null)
                {
                    _skypeInstance = new SkypeClass();
                    // Online Status is only available if we access skypeInstance as SkypeClass
                    var skypeClass = (SkypeClass)_skypeInstance;
                    skypeClass.OnlineStatus += skypeClass_OnlineStatus;
                }
                return _skypeInstance;
            }
        }

        private void skypeClass_OnlineStatus(User pUser, TOnlineStatus status)
        {
            if (_mappings[pUser.Handle] != null)
            {
                PresenceChangedEventArgs presenceChangedEventArgs = new SkypePresenceChangedEventArgs(pUser, status,
                                                                                                      _mappings[
                                                                                                          pUser.Handle].
                                                                                   TfsName);
                OnPresenceChanged(presenceChangedEventArgs);
            }
        }

        /// <summary>
        /// Gets the user mappings.
        /// </summary>
        /// <value>The user mappings.</value>
        public UserMappingCollection UserMappings
        {
            get
            {
                // Upon first access to the mappings we ensure to laod them
                if (_mappings == null)
                {
                    _mappings = new UserMappingCollection();
                    _mappings.Load();
                }
                return _mappings;
            }
        }

        #endregion

        #region Contact management

        /// <summary>
        /// Gets the contact.
        /// </summary>
        /// <param name="contactId">The contact id.</param>
        /// <returns>The contact that matches the specified <paramref name="contactId"/>.</returns>
        public override Contact GetContact(string contactId)
        {
            Logger.Write(string.Format("GetContact was called with contactId {0}.",contactId));
            var mapping = UserMappings.GetUserMapping(contactId);
            if (mapping != null)
            {
                User user = null;
                // Do not access skype if it is not running
                if (IsSkypeRunning())
                {
                    // We return the contact even though it might be marked as ignored.
                    user = SkypeInstance.User[mapping.SkypeName];
                }
                var skypeContact = new SkypeContact(user, SkypeInstance, mapping);
                return skypeContact;
            }
            return null;
        }

        /// <summary>
        /// Adds the contact.
        /// </summary>
        /// <param name="contactId">The contact id.</param>
        public override void AddContact(string contactId)
        {
            // If this user was already requested do nothing
            if (_requestedForCreation.Contains(contactId))
                return;
            // If the user is already mapped do nothing but if it is not mapped or ignored handle it.
            var mapping = UserMappings.GetUserMapping(contactId);
            if (mapping == null)
            {
                _requestedForCreation.Add(contactId);
                Logger.Write(string.Format("Added contactid {0} to request for creation list.",contactId));
            }

        }

        /// <summary>
        /// Shows the add edit contact UI.
        /// </summary>
        /// <param name="contactId">The contact id.</param>
        public override void ShowAddEditContactUI(string contactId)
        {
            // Do not access skype if it is not running
            if ((!IsSkypeRunning()))
            {
                MessageBox.Show(Properties.Resources.YouCannotAddUsersAsLongAsSkypeIsNotRunning);
                return;
            }
            IList<User> skypeUsersList = new List<User>(SkypeInstance.Friends.ToArray());
            skypeUsersList.Add(SkypeInstance.CurrentUser);
            var skypeUsers = new BindingList<User>(skypeUsersList);
            Logger.Write(string.Format("Found {0} users without a mapping (requestesforcreation.",_requestedForCreation.Count));
            _requestedForCreation.Sort();
            var tfsUsers = new BindingList<string>(_requestedForCreation);
            var presenter = new UserManagementFormPresenter(skypeUsers, tfsUsers) { SelecetdTfsUser = contactId, Mappings = UserMappings };
            var addUserDialog = new UserManagementForm(presenter);
            addUserDialog.ShowDialog();
            UserMappings.Save();
        }

        /// <summary>
        /// Gets the group.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        /// <returns></returns>
        public override IContactGroup GetGroup(string groupName)
        {
            Logger.Write(string.Format("GetGroup was called with groupName {0}.", groupName));
            IContactGroup contactGroup = null;
            // Do not access skype if it is not running
            if ((!IsSkypeRunning()))
                return null;

            foreach (Group group in SkypeInstance.Groups)
            {
                if (group.CustomGroupId == groupName)
                {
                    contactGroup = new SkypeContactGroup(SkypeInstance, groupName);
                }
            }
            return contactGroup;
        }

        /// <summary>
        /// Adds the group.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        public override void AddGroup(string groupName)
        {
            // TODO: Verifiy if adding a group is required
            //skype.CreateGroup(groupName);
        }

        #endregion

        #endregion
    }
}