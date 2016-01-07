using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SKYPE4COMLib;
using TfsCommunity.Collaboration.Skype.Helpers;
using TfsCommunity.Collaboration.Skype.Interfaces;

namespace TfsCommunity.Collaboration.Skype.Controls
{
    /// <summary>
    /// Interaction logic for ContactControl.xaml
    /// </summary>
    public partial class ContactControl : IContactControl<ISkype>
    {
        #region Fields
        private ICollaborationUIProviderExtended<ISkype> _provider;
        private Timer _timer;
        #endregion

        #region Constructor

        public ContactControl() : base()
        {
            
        }

        public ContactControl(string contactId, ICollaborationUIProviderExtended<ISkype> provider)
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionDetails("Control", ex);
            }

            if (contactId == null)
            {
                throw new ArgumentNullException("contactId");
            }

            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            Source = contactId;
            Provider = provider;

            if (Provider.MappedContacts.ContainsKey(contactId))
            {
                DataContext = Provider.MappedContacts[contactId];
            }
            else
            {
                lblName.Text = contactId;
                lblStatus.Text = "Unkown";
            }
        }

        #endregion

        #region Properties
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "SourceProperty", typeof(string), typeof(ContactControl), new PropertyMetadata(default(string)));
        /// <summary>
        /// Source is TFS user contact id
        /// </summary>
        public string Source
        {
            get
            {
                return (string)GetValue(SourceProperty);

            }
            set
            {
                SetValue(SourceProperty, value);
            }
        }

        /// <summary>
        /// Reference to Provider Object -> we need this to find user data
        /// </summary>
        public ICollaborationUIProviderExtended<ISkype> Provider
        {
            get
            {
                return _provider;
            }
            set
            {
                if (value != null && value != _provider)
                {
                    _provider = value;
                }
            }
        }

        /// <summary>
        /// Used for closing automatically the skype actions popup
        /// </summary>
        private Timer PopupTimer
        {
            get
            {
                if (null == _timer)
                {
                    _timer = new Timer(1000);
                    PopupTimer.AutoReset = false;
                    PopupTimer.Elapsed += OnTimerElapsed;
                }

                return _timer;
            }
        }
        #endregion

        #region Events
        // ReSharper disable InconsistentNaming
        private void chatImageButton_Click(object sender, RoutedEventArgs e)
        // ReSharper restore InconsistentNaming
        {
            if (_provider.MappedContacts.ContainsKey(Source) && _provider.MappedContacts[Source] != null && _provider.MappedContacts[Source].IsMessagingAvailable)
                _provider.MappedContacts[Source].StartChatAsync();
        }

        // ReSharper disable InconsistentNaming
        private void audioImageButton_Click(object sender, RoutedEventArgs e)
        // ReSharper restore InconsistentNaming
        {
            if (_provider.MappedContacts.ContainsKey(Source) && _provider.MappedContacts[Source] != null && _provider.MappedContacts[Source].IsVoipAvailable)
                _provider.MappedContacts[Source].StartCallAsync();
        }

        // ReSharper disable InconsistentNaming
        private void videoImageButton_Click(object sender, RoutedEventArgs e)
        // ReSharper restore InconsistentNaming
        {
            if (_provider.MappedContacts.ContainsKey(Source) && _provider.MappedContacts[Source] != null && _provider.MappedContacts[Source].IsVideoAvailable)
                _provider.MappedContacts[Source].StartVideoCallAsync();
        }

        private void ImgAvatar_OnMouseEnter(object sender, MouseEventArgs e)
        {
            StopPopupClosingTimer();
            PopupSkypeActions.StaysOpen = true;
            PopupSkypeActions.IsOpen = true;
        }

        // ReSharper disable InconsistentNaming
        private void PopupSkypeActions_MouseEnter(object sender, MouseEventArgs e)
        // ReSharper restore InconsistentNaming
        {
            StopPopupClosingTimer();
        }


        private void ImgAvatar_OnMouseLeave(object sender, MouseEventArgs e)
        {
            StartPopupClosingTimer();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (PopupSkypeActions.Dispatcher.CheckAccess())
            {
                ClosePopup();
            }
            else
            {
                PopupSkypeActions.Dispatcher.Invoke(ClosePopup);
            }
        }

        private void ClosePopup()
        {
            PopupSkypeActions.StaysOpen = false;
            PopupSkypeActions.IsOpen = false;
        }

        #endregion

        #region Internal methods
        /// <summary>
        /// Starting skype actions popup closing activities
        /// </summary>
        private void StartPopupClosingTimer()
        {
            PopupTimer.Start();
        }

        /// <summary>
        /// Stopping skype actions popup closing activities
        /// </summary>
        private void StopPopupClosingTimer()
        {
            PopupTimer.Stop();
        }

        /// <summary>
        /// Refresh the content of the control -> e.g. tooltip is shown, user has assigned a customid
        /// </summary>
        public void Refresh()
        {
            DataContext = Provider.MappedContacts[Source];
        }
        #endregion
    }
}