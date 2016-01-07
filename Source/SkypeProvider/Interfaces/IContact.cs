#region

using System;
using System.Collections.Generic;
using System.Windows.Media;
using Microsoft.TeamFoundation.Collaboration;
using TfsCommunity.Collaboration.Skype.Enum;

#endregion

namespace TfsCommunity.Collaboration.Skype.Interfaces
{
    public interface IContact<TUserObject,TClientInstance>:IDisposable
    {
        /// <summary>
        /// 3rd party messenger client user name
        /// </summary>
        string UserName { get; }

        // Display name of current IUser 
        string DisplayName { get; }

        /// <summary>
        /// Status text /mood text of user
        /// </summary>
        string StatusDetailText { get; }

        /// <summary>
        /// Status of user (3rd party independent)
        /// </summary>
        UserStatus Status { get; }

        /// <summary>
        /// Avatar of current IUser instance as ImageSource object
        /// </summary>
        ImageSource AvatarSource { get; }

        /// <summary>
        /// Is contact mapped
        /// </summary>
        bool IsMapped { get; }

        /// <summary>
        /// Is contact enabled/disabled 
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Access to the 3rd party instand messenger client object (e.g. Skype)
        /// </summary>
        TUserObject MessengerUserObject { get; }

        /// <summary>
        /// Access to IUser's ContactData object (TfsCollab framework)
        /// </summary>
        ContactData TfsContactData { get; }

        /// <summary>
        /// Access to the parent provider object
        /// </summary>
        ICollaborationUIProviderExtended<TClientInstance> ProviderObject { get;}

        /// <summary>
        /// Protocol of the current IUser object
        /// </summary>
        string ProtocolName { get; }

        #region Retrieving user related supported actions
        /// <summary>
        /// Does the current IUser supports VOIP calls
        /// </summary>
        bool IsVoipAvailable { get; }

        /// <summary>
        /// Does the current IUser supports chats
        /// </summary>
        bool IsMessagingAvailable { get; }

        /// <summary>
        /// Does the current IUser supports video calls
        /// </summary>
        bool IsVideoAvailable { get; }
        #endregion

        #region Retrieving group related supported actions
        /// <summary>
        /// Does the current IUser supports group VOIP calls with other IUser's
        /// </summary>
        bool IsVoipConferenceAvailable { get; }

        /// <summary>
        /// Does the current IUser supports group chats with other IUser's
        /// </summary>
        bool IsMessagingConferenceAvailable { get; }

        /// <summary>
        /// Does the current IUser supports group video calls with other IUser's
        /// </summary>
        bool IsVideoConferenceAvailable { get; }
        #endregion

        #region Starting user related actions
        /// <summary>
        /// Start a call with the current IUser
        /// </summary>
        void StartCallAsync();
        /// <summary>
        /// Start a chat session with the current IUser
        /// </summary>
        void StartChatAsync();
        
        /// <summary>
        /// Start a video call with the current IUser
        /// </summary>
        void StartVideoCallAsync();
        #endregion

        #region Starting user related actions
        /// <summary>
        /// Start a call with a group of people
        /// </summary>
        /// <param name="conferenceContacts"></param>
        void StartCallAsync(IEnumerable<IContact<TUserObject,TClientInstance>> conferenceContacts);

        /// <summary>
        /// Start a chat session with a group of people
        /// </summary>
        /// <param name="conferenceContacts"></param>
        void StartChatAsync(IEnumerable<IContact<TUserObject,TClientInstance>> conferenceContacts);

        /// <summary>
        /// Start a video call with a group of people
        /// </summary>
        /// <param name="conferenceContacts"></param>
        void StartVideoCallAsync(IEnumerable<IContact<TUserObject,TClientInstance>> conferenceContacts);

        /// <summary>
        /// Refresh some properties externaly
        /// </summary>
        void RefreshAsync();

        #endregion
    }
}