#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.TeamFoundation.Collaboration;
using SKYPE4COMLib;
using TfsCommunity.Collaboration.Skype.Enum;
using TfsCommunity.Collaboration.Skype.Helpers;
using TfsCommunity.Collaboration.Skype.Interfaces;

#endregion

namespace TfsCommunity.Collaboration.Skype.Models
{
    /// <summary>
    ///   TODO: Update summary.
    /// </summary>
    internal class SkypeContact : IContact<IUser, ISkype>, INotifyPropertyChanged
    {
        #region Constructors

        public SkypeContact(ContactData contact, IUser skypeUser, ICollaborationUIProviderExtended<ISkype> provider)
        {
            _messengerUserObject = skypeUser;
            _tfsContactData = contact;
            _provider = provider;

            // register to onlinestatus changes of skype users
            if (_messengerUserObject != null)
            {
                var skypeEvents = (_ISkypeEvents_Event)provider.ClientInstance;
                skypeEvents.OnlineStatus += skypeEvents_OnlineStatus;
                skypeEvents.Command += skypeEvents_Command;
            }
        }

        #endregion

        #region Fields

        private  IUser _messengerUserObject;

        private ICollaborationUIProviderExtended<ISkype> _provider;
        private ContactData _tfsContactData;
        private ICall _activeSkypeCall;

        private bool _avatarAvailable;
        private string _avatarFileName = string.Empty;

        private ImageSource _avatarImage;

        #endregion

        #region Constants

        private const int VideoWaitTime = 1000;

        #endregion

        #region IContact<IUser> Members

        public string UserName
        {
            get { return MessengerUserObject.Handle; }
        }

        /// <summary>
        ///   DisplayName - Priority is Alias -> DisplayName -> FullName -> SkypeLoginname
        /// </summary>
        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(_messengerUserObject.Aliases))
                    return _messengerUserObject.Aliases;
                if (!string.IsNullOrEmpty(MessengerUserObject.DisplayName))
                    return MessengerUserObject.DisplayName;
                if (!string.IsNullOrEmpty(MessengerUserObject.FullName))
                    return MessengerUserObject.FullName;
                return _messengerUserObject.Handle;
            }
        }

        /// <summary>
        ///   Skype's RichMoodText
        /// </summary>
        public string StatusDetailText
        {
            get
            {
                // Timezone is utc + diff hours
                var destinationTime = DateTime.UtcNow.AddHours(MessengerUserObject.Timezone);

                // Take city or country value if available
                string location = string.Empty;
                if (!string.IsNullOrEmpty(MessengerUserObject.City) && !string.IsNullOrEmpty(MessengerUserObject.Country))
                {
                    location = string.Format("{0},{1}", MessengerUserObject.City, MessengerUserObject.Language);
                }
                else
                {
                    if (!string.IsNullOrEmpty(MessengerUserObject.City))
                    {
                        location = MessengerUserObject.City;
                    }
                    if (!string.IsNullOrEmpty(MessengerUserObject.CountryCode))
                        location = MessengerUserObject.CountryCode;
                }
                if (!string.IsNullOrEmpty(location))
                {
                    location = string.Format("({0})", location);
                }

                var statusDetailText = string.Format("{0}{1} {2}", destinationTime, location, MessengerUserObject.MoodText);

                // Some debugging information
                #if Debug
                    Logger.Write(statusDetailText);
                #endif

                return statusDetailText;
            }
        }

        public UserStatus Status
        {
            get
            {
                // we convert the skype status with our own extention method
                return MessengerUserObject.OnlineStatus.ToUserStatus();
            }
        }


        /// <summary>
        ///   Profile picture as local disk filename
        /// </summary>
        public string AvatarFilename
        {
            get
            {
                _avatarFileName = string.Format("{0}\\TFSCollab\\{1}_{2}.jpg",
                                                Environment.GetEnvironmentVariable("Temp"),
                                                UserName, ProtocolName);
                return _avatarFileName;
            }
        }

        /// <summary>
        ///   Profile picture as ImageSource compatible type for direct binding
        /// </summary>
        public ImageSource AvatarSource
        {
            get
            {
                if (!string.IsNullOrEmpty(AvatarFilename))
                {
                    if (!_avatarAvailable && SaveSkypeAvatarToDiskAsync(UserName, AvatarFilename).Result)
                    {
                        _avatarAvailable = true;
                        _avatarImage = new BitmapImage(new Uri(AvatarFilename));
                    }

                    if (_avatarAvailable)
                    {
                        return _avatarImage;
                    }
                }
                // If no picture is available we return the nopicture image from the ressource file
                return ImageConverter.Convert(Resources.Resources.NoPicture);
            }
        }

        public bool IsMapped
        {
            get
            {
                if (null != MessengerUserObject)
                    return true;
                return false;
            }
        }

        public bool IsEnabled
        {
            get
            {
                if (null != MessengerUserObject)
                    return true;
                return false;
            }
        }

        /// <summary>
        ///   Skype UserObject from Skype API
        /// </summary>
        public IUser MessengerUserObject
        {
            get { return _messengerUserObject; }
        }

        /// <summary>
        ///   UserObject from TFS Collab API
        /// </summary>
        public ContactData TfsContactData
        {
            get { return _tfsContactData; }
        }

        public ICollaborationUIProviderExtended<ISkype> ProviderObject
        {
            get { return _provider; }
        }

        public string ProtocolName
        {
            get { return "Skype"; }
        }

        public bool IsVoipAvailable
        {
            get
            {
                if (!MessengerUserObject.IsSkypeOutContact)
                    return true;
                return false;
            }
        }

        public bool IsMessagingAvailable
        {
            get
            {
                if (!MessengerUserObject.IsSkypeOutContact)
                    return true;
                return false;
            }
        }

        public bool IsVideoAvailable
        {
            get
            {
                if (MessengerUserObject.IsVideoCapable)
                    return true;
                return false;
            }
        }

        public bool IsVoipConferenceAvailable
        {
            get { return false; }
        }

        public bool IsMessagingConferenceAvailable
        {
            get { return false; }
        }

        public bool IsVideoConferenceAvailable
        {
            get { return false; }
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var e = new PropertyChangedEventArgs(propertyName);
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        #region Skype video and avatar handling

        private async Task<bool> SaveSkypeAvatarToDiskAsync(string userHandle, string rootedPathFileName)
        {
            var result = await SaveSkypeAvatarToDisk(userHandle, rootedPathFileName);
            return result;
        }

        /// <summary>
        ///   Retrieves skype profile picture to local harddisk
        /// </summary>
        /// <param name="userHandle"> </param>
        /// <param name="rootedPathFileName"> </param>
        /// <returns> </returns>
        private Task<bool> SaveSkypeAvatarToDisk(string userHandle, string rootedPathFileName)
        {
            // Source: http://community.skype.com/t5/Windows/how-to-get-the-profile-picture-of-users-using-skype4com-api-with/td-p/444063
            // filename must contain full path
            if (!Path.IsPathRooted(rootedPathFileName))
                // ReSharper disable LocalizableElement
                throw new ArgumentException("Filename does not contain full path!", "rootedPathFileName");
            // ReSharper restore LocalizableElement

            // filename must have filetype jpg
            if (!".jpg".Equals(Path.GetExtension(rootedPathFileName)))
                // ReSharper disable LocalizableElement
                throw new ArgumentException("Filename does not represent jpg file!", "rootedPathFileName");
            // ReSharper restore LocalizableElement

            // Delete profile picture file localy if it exists
            if (File.Exists(rootedPathFileName))
            {
                FileManagement.DeleteFile(rootedPathFileName);
            }

            // Create profile directory if it doesn't exist
            // ReSharper disable AssignNullToNotNullAttribute
            if (!Directory.Exists(Path.GetDirectoryName(rootedPathFileName)))
            // ReSharper restore AssignNullToNotNullAttribute
            {
                // ReSharper disable AssignNullToNotNullAttribute
                Directory.CreateDirectory(Path.GetDirectoryName(rootedPathFileName));
                // ReSharper restore AssignNullToNotNullAttribute
            }

            // http://developer.skype.com/public-api-reference#COMMAND_GET_USER_AVATAR
            var command0 = new Command { Command = string.Format("GET USER {0} AVATAR 1 {1}", userHandle, rootedPathFileName) };
            var skype = _provider.ClientInstance;
            skype.SendCommand(command0);
            Thread.Sleep(100);
            //debugging
            string replay = command0.Reply;
            Logger.Write(string.Format("{0}: Replay: {1}", Resources.Resources.ProviderName, replay));

            while (!File.Exists(_avatarFileName))
            {
                Thread.Sleep(100);
            }

            // Check if file was saved sucessfully to local harddisk
            if (File.Exists(rootedPathFileName) && new FileInfo(rootedPathFileName).Length > 0)
                return Task.FromResult(true);

            // Delete zero size files
            if (FileManagement.IsZeroSizeFile(rootedPathFileName))
            {
                try
                {
                    File.Delete(rootedPathFileName);
                }
                catch (IOException ex)
                {
                    Logger.Write("Avatar file with size of 0 byte is locked and couldn't be deleted.");
                    Logger.WriteExceptionDetails(Resources.Resources.ProviderName, ex);
                }
            }

            return Task.FromResult(false);
        }

        /// <summary>
        ///   Enables video on skype calls once the call is running.
        /// </summary>
        /// <param name="call"> The thread start parameter. </param>
        private void EnableVideo(ICall call)
        {
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

        #endregion

        #region Skype Events

        /// <summary>
        ///   Change the Status property based on skype user status changes
        ///   Addiontionaly we refresh also the avatar
        /// </summary>
        /// <param name="pUser"> Skype User Object </param>
        /// <param name="status"> </param>
        private void skypeEvents_OnlineStatus(User pUser, TOnlineStatus status)
        {
            if (pUser.Handle == UserName)
            {
                RefreshAsync();
            }
        }

        private void skypeEvents_Command(Command pCommand)
        {
            // debugging information
            #if Debug
            Logger.Write(string.Format("{0}: id:{3} Command: {1} Replay:{2}", Resources.Resources.ProviderName,
                                          pCommand.Command, pCommand.Reply, pCommand.Id));
            #endif
        }

        #endregion

        #region Skype actions

        /// <summary>
        ///   Start an audio conversation vis Skype
        /// </summary>
        public async void StartCallAsync()
        {
            await StartCall();
        }

        /// <summary>
        ///   Start a chat session via Skype
        /// </summary>
        public async void StartChatAsync()
        {
            await StartChat();
        }

        /// <summary>
        ///   Start a video conversation (webcam) via Skype
        /// </summary>
        public async void StartVideoCallAsync()
        {
            await StartVideoCall();
        }

        /// <summary>
        /// Start a call with a group of people
        /// </summary>
        /// <param name="conferenceContacts"></param>
        public void StartCallAsync(IEnumerable<IContact<IUser, ISkype>> conferenceContacts)
        {
            throw new NotSupportedException("At the moment TfsCollab doesn't support group collaboration features.");
        }

        /// <summary>
        /// Start a chat session with a group of people
        /// </summary>
        /// <param name="conferenceContacts"></param>
        public void StartChatAsync(IEnumerable<IContact<IUser, ISkype>> conferenceContacts)
        {
            throw new NotSupportedException("At the moment TfsCollab doesn't support group collaboration features.");
        }

        /// <summary>
        /// Start a video call with a group of people
        /// </summary>
        /// <param name="conferenceContacts"></param>
        public void StartVideoCallAsync(IEnumerable<IContact<IUser, ISkype>> conferenceContacts)
        {
            throw new NotSupportedException("At the moment TfsCollab doesn't support group collaboration features.");
        }

        private Task StartCall()
        {
            return Task.Run(() =>
                            _activeSkypeCall = _provider.ClientInstance.PlaceCall(UserName, null, null, null));
        }

        private Task StartChat()
        {
            return Task.Run(() =>
                                {
                                    IChat skypeChat = _provider.ClientInstance.CreateChatWith(UserName);
                                    skypeChat.OpenWindow();
                                });
        }

        private Task StartVideoCall()
        {
            //Before we could start a video call we need to start an audio call
            StartCallAsync();

            var result = Task.Run(() => EnableVideo(_activeSkypeCall));
            return result;
        }

        #endregion

        #region internal actions
        public async void RefreshAsync()
        {
            await Refresh();
        }

        private Task Refresh()
        {
            return Task.Run(
                () =>
                    {
                        OnPropertyChanged("Status");
                        OnPropertyChanged("StatusDetailText");
                        OnPropertyChanged("AvatarFilename");
                        OnPropertyChanged("AvatarSource");
                    });
        }

        #endregion

        #region IDisposable
        public void Dispose()
        {
            this._avatarImage = null;
            this._messengerUserObject = null;
            this._tfsContactData = null;
            this._activeSkypeCall = null;
            if (_provider != null)
            {
                var skypeEvents = (_ISkypeEvents_Event)_provider.ClientInstance;
                skypeEvents.OnlineStatus -= skypeEvents_OnlineStatus;
                skypeEvents.Command -= skypeEvents_Command;
            }
            this._provider = null;
        }
        #endregion
    }
}