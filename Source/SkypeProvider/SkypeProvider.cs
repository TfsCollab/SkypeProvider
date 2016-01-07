#region

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.TeamFoundation.Collaboration;
using SKYPE4COMLib;
using TfsCommunity.Collaboration.Skype.Controls;
using TfsCommunity.Collaboration.Skype.Helpers;
using TfsCommunity.Collaboration.Skype.Interfaces;
using TfsCommunity.Collaboration.Skype.Models;
using System.Threading;
#endregion

namespace TfsCommunity.Collaboration.Skype
{
    [TeamMembersProvider("E5F7B919-7945-4FB5-9933-AEB58870D697", "Skype by AIT TeamSystemPro", true)]
    public class SkypeProvider : ICollaborationUIProviderExtended<ISkype>
    {
        #region Private Fields

        private ISkype _skypeInstance;
        private Dictionary<string, ContactData> _contacts;
        private Dictionary<string, IContact<IUser, ISkype>> _mappedContacts;
        private Dictionary<string, IContactControl<ISkype>> _mappedContactControls;

        private bool _initialized;
        #endregion

        #region Properties

        /// <summary>
        /// Cached tfs contact objects, key is tfs user name
        /// </summary>
        public Dictionary<string, ContactData> Contacts
        {
            get { return this._contacts ?? (this._contacts = new Dictionary<string, ContactData>()); }
        }

        /// <summary>
        /// Cache internal icontact object for tfs contact objects, key is tfs user name
        /// </summary>
        public Dictionary<string, IContact<IUser, ISkype>> MappedContacts
        {
            get
            {
                if (this._mappedContacts == null)
                    this._mappedContacts = new Dictionary<string, IContact<IUser, ISkype>>();
                return this._mappedContacts;
            }
        }

        /// <summary>
        /// Cache user controls for mapped contacts, key is tfs user name
        /// </summary>
        public Dictionary<string, IContactControl<ISkype>> MappedContactControls
        {
            get
            {
                if (this._mappedContactControls == null)
                    this._mappedContactControls = new Dictionary<string, IContactControl<ISkype>>();
                return this._mappedContactControls;
            }
        }

        public ISkype ClientInstance
        {
            get
            {
                Logger.Write(string.Format("{0}: SkypeInstance property was called.",
                                              Resources.Resources.ProviderName));

                // Upon first access we ensure to create the COM instance
                if (this._skypeInstance == null)
                {
                    Logger.Write("Creating Skype COM object.");
                    this._skypeInstance = new SKYPE4COMLib.Skype();
                }
                return this._skypeInstance;
            }
            private set
            {
                _skypeInstance = value;
            }

        }

        #endregion

        #region ICollaborationUIProvider methods

        /// <summary>
        /// Indicate if current provider supports using of custom contact ids
        /// </summary>
        public bool AllowsSettingOthersContactIDs
        {
            get { return true; }
        }

        public bool CanGetArbitraryContactID
        {
            get { return false; }
        }

        public string GetArbitraryContactID()
        {
            return string.Empty;
        }

        /// <summary>
        /// Indicate if current provider supports connecting to Skype network  
        /// </summary>
        public bool CanConnect
        {
            get { return true; }
        }

        /// <summary>
        /// Clear custom lists
        /// </summary>
        public void Clear()
        {
            MappedContacts.Clear();
            MappedContactControls.Clear();
            Contacts.Clear();
        }

        // ReSharper disable CSharpWarnings::CS1998
        public async void ConnectAsync()
        // ReSharper restore CSharpWarnings::CS1998
        {
            Logger.Write("ConnectAsync was called.");
            await this.Connect();
        }

        public event EventHandler<MessengerContactEventArgs> ContactHostingUpdate;

        /// <summary>
        /// Create contact panel items/user controls
        /// </summary>
        /// <param name="contact">TFS contact object</param>
        /// <returns>WPF User Control</returns>
        public object ContactPanel(ContactData contact)
        {

            // Check if contact object contains the service if of current provider -> if not then we do nothing
            if (IsConnected && contact.ServiceIDs.ContainsKey(Resources.Resources.ServiceID) &&
                MappedContacts.ContainsKey(contact.Name))
            {
                Logger.Write(string.Format("ContactPanel was called for user {0}.", contact.Name));
                IContactControl<ISkype> contactControl;
                // Check if a control for the current contact aleady exists then we return this object
                if (_mappedContactControls.ContainsKey(contact.Name))
                {
                    Logger.Write("Reusing user control object.");
                    contactControl = new ContactControl(contact.Name, this);
                    MappedContactControls[contact.Name] = contactControl;
                    //old logic -> does not work anymore after update 1 power tools update
                    //contactControl = MappedContactControls[contact.Name];
                    //contactControl.Refresh();
                }
                else
                {
                    Logger.Write("Creating new user control.");
                    contactControl = new ContactControl(contact.Name, this);
                    MappedContactControls.Add(contact.Name, contactControl);
                }

                return contactControl;
            }

            // If not mapped user exists then we fire the lost event because a the event was trigged via locatecontact
            // The default action is lost because we reach this event only if something unexpected happend
            Logger.Write("ContactPanel was called.");
            Logger.Write("Lost status was fired and null returned.");
            /*OnContactHostingUpdate(
                new MessengerContactEventArgs(
                    contact, Resources.Resources.ServiceID, MessengerContactHostingStatus.Lost));*/

            // return a default contact if everything else is not working
            return new ContactControl(string.Empty, this);
        }

        /// <summary>
        ///  Disconnect connection to Skype client (not the network)
        /// </summary>
        public void Disconnect()
        {
            // ToDo: Disconnect
            // see connect 
            // we do not logout via VS if a skype client is online
            if (null != ClientInstance && ClientInstance.AttachmentStatus != TAttachmentStatus.apiAttachRefused)
            {

                ClientInstance.Client.Shutdown();
            }
            OnServiceStatusUpdate(new MessengerProviderStatusEventArgs(Resources.Resources.ServiceID, MessengerProviderStatus.Lost));
        }

        /// <summary>
        /// Skype provider name
        /// </summary>
        public string FriendlyName
        {
            get { return Resources.Resources.ProviderName; }
        }

        /// <summary>
        /// Initialite skype client (primarly events)
        /// </summary>
#pragma warning disable 1998
        public async void InitializeAsync()
#pragma warning restore 1998
        {
            Logger.Write("InitializeAsync was called.");
            await Initialize();
            Logger.Write("InitializeAsync sucessfully.");
        }

        /// <summary>
        /// Return Skype network connection status
        /// </summary>
        public bool IsConnected
        {
            get
            {
                if (IsSkypeRunning & ClientInstance.AttachmentStatus != TAttachmentStatus.apiAttachRefused)
                    return ClientInstance.ConnectionStatus == TConnectionStatus.conOnline;
                return false;
            }
        }

        /// <summary>
        /// Locate skype user asychronisouly 
        /// </summary>
        /// <param name="contact">TFS contact object</param>
        public async void LocateContactAsync(ContactData contact)
        {
            await LocateContact(contact);
        }

        /// <summary>
        /// Return the current skype contact id
        /// </summary>
        public string MyContactId
        {
            get
            {
                // Do not access skype if it is not running
                if (!IsSkypeRunning)
                    return string.Empty;
                return ClientInstance.CurrentUser.Handle;
            }
        }

        // ReSharper disable CSharpWarnings::CS1998
        public async void RefreshAsync()
        // ReSharper restore CSharpWarnings::CS1998
        {
            // ToDo: RefreshAsync
            // At the moment we don't support that feature -> maybe in the future
        }

        public event EventHandler<MessengerProviderStatusEventArgs> ServiceStatusUpdate;
        /// <summary>
        /// Dispose objects
        /// </summary>
        public void Dispose()
        {
            this.Clear();
            // Release Skype COM object
            this._skypeInstance = null;
        }

        /// <summary>
        /// Notify the collab framerwork about contact updates (e.g. status update)
        /// </summary>
        /// <param name="e"></param>
        private void OnContactHostingUpdate(MessengerContactEventArgs e)
        {
            EventHandler<MessengerContactEventArgs> handler = ContactHostingUpdate;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Initialize Provider 
        /// Register SkypeEvents for notifying CollabFramework 
        /// </summary>
        /// <returns></returns>
        private Task Initialize()
        {
            return Task.Run(() =>
            {
                Logger.Write("Initialize was called.");
                if (IsSkypeRunning && !_initialized)
                {
                    Logger.Write("Register skype connection changed event");
                    ((_ISkypeEvents_Event)ClientInstance).ConnectionStatus +=
                        SkypeProvider_ConnectionStatus;
                    _initialized = true;
                    //this.OnServiceStatusUpdate(new MessengerProviderStatusEventArgs(Resources.Resources.ServiceID,
                    //                                                                MessengerProviderStatus.Pending));
                }
            });
        }

        /// <summary>
        /// Start Skype client in non-minized mode with splashscreen)
        /// </summary>
        /// <returns></returns>
        private Task Connect()
        {
            return Task.Run(
                () =>
                {
                    int retryCounter = 30;
                    if (!IsSkypeRunning)
                    {
                        // Start Skype in non minimized mode and show splashscreen            
                        Logger.Write("Starting Skype");
                        ClientInstance.Client.Start();
                        do
                        {
                            //this.OnServiceStatusUpdate(new MessengerProviderStatusEventArgs(Resources.Resources.ServiceID, MessengerProviderStatus.Pending));
                            Logger.Write("Waiting for Skype.");
                            retryCounter--;
                            Thread.Sleep(1000);
                        }
                        while (!IsSkypeRunning && retryCounter > 0);
                    }

                    if (retryCounter == 0)
                    {
                        Logger.Write("Skype started not in expected time.");
                        this.OnServiceStatusUpdate(new MessengerProviderStatusEventArgs(Resources.Resources.ServiceID, MessengerProviderStatus.Lost));
                        OnServiceStatusUpdate(new MessengerProviderStatusEventArgs(Resources.Resources.ServiceID, MessengerProviderStatus.Lost));

                    }
                    else
                    {
                        OnServiceStatusUpdate(new MessengerProviderStatusEventArgs(Resources.Resources.ServiceID, MessengerProviderStatus.Ready));

                    }
                });
        }


        // ReSharper disable InconsistentNaming
        /// <summary>
        /// Check Skype client status, e.g. is client connected to the Skype network
        /// </summary>
        /// <param name="status"></param>
        private void SkypeProvider_ConnectionStatus(TConnectionStatus status)
        // ReSharper restore InconsistentNaming
        {
            switch (status)
            {
                case TConnectionStatus.conConnecting:
                    {
                        OnServiceStatusUpdate(
                            new MessengerProviderStatusEventArgs(Resources.Resources.ServiceID, MessengerProviderStatus.Pending));
                        break;
                    }
                case TConnectionStatus.conOffline:
                    {
                        OnServiceStatusUpdate(
                            new MessengerProviderStatusEventArgs(Resources.Resources.ServiceID, MessengerProviderStatus.Lost));
                        break;
                    }
                case TConnectionStatus.conOnline:
                    {
                        OnServiceStatusUpdate(
                            new MessengerProviderStatusEventArgs(Resources.Resources.ServiceID, MessengerProviderStatus.Ready));
                        break;
                    }
                default:
                    {
                        OnServiceStatusUpdate(
                            new MessengerProviderStatusEventArgs(Resources.Resources.ServiceID, MessengerProviderStatus.Lost));
                        break;
                    }
            }
        }

        /// <summary>
        /// Find a skype user for a TFS contact
        /// The resolver is using different methods to find an appropiate skype name (see SkypeNameResolver)  
        /// </summary>
        /// <param name="contact">TFS contact object</param>
        /// <returns></returns>
        private Task LocateContact(ContactData contact)
        {
            return Task.Run(() =>
                                {
                                    Logger.Write(string.Format("LocateContact was called for user {0}.", contact.Name));
                                    // custom contactid is stored in serviceid
                                    // use the current service id to get the custom id

                                    if ( //IsConnected && contact.ServiceIDs.ContainsKey(Resources.Resources.ServiceID) &&
                                            !Contacts.ContainsKey(contact.Name) && !MappedContacts.ContainsKey(contact.Name))
                                    {
                                        var skypeNameResolver = new SkypeNameResolver(this);
                                        string skypeName = skypeNameResolver.FindMapping(contact);

                                        if (!string.IsNullOrEmpty(skypeName))
                                        {
                                            Logger.Write(string.Format("Found skpye username {0} for TFS user {1}.", skypeName, contact.Name));

                                            var skypeUser = ClientInstance.User[skypeName];

                                            IContact<IUser, ISkype> internalContact = new SkypeContact(contact, skypeUser,
                                                                                               this);

                                            // TfsCollab will use our custom id as contact.Name in ContactPanel Method
                                            if (!MappedContacts.ContainsKey(contact.Name))
                                            {
                                                MappedContacts.Add(contact.Name, internalContact);

                                                // Save found skypename as collaboration provider custom id
                                                contact.ServiceIDs[Resources.Resources.ServiceID] = skypeUser.Handle;
                                            }
                                            if (!Contacts.ContainsKey(contact.Name))
                                                Contacts.Add(contact.Name, contact);

                                            // Check if a skype user names exists for the current contact object
                                            // If true then fire hostingstatus.found 
                                            // If false then fire notfound
                                            if (!string.IsNullOrEmpty(internalContact.UserName))
                                                OnContactHostingUpdate(new MessengerContactEventArgs(contact,
                                                                                                     Resources.
                                                                                                         Resources.
                                                                                                         ServiceID,
                                                                                                     MessengerContactHostingStatus
                                                                                                         .Found));
                                            else
                                                OnContactHostingUpdate(new MessengerContactEventArgs(contact,
                                                                                                     Resources.
                                                                                                         Resources.
                                                                                                         ServiceID,
                                                                                                     MessengerContactHostingStatus
                                                                                                         .NotFound));
                                            return;
                                        }

                                        // Fire not found event in case a Skype name is null or empty
                                        this.OnContactHostingUpdate(new MessengerContactEventArgs(contact,
                                            Resources.
                                                Resources.
                                                ServiceID,
                                            MessengerContactHostingStatus
                                                .NotFound));
                                        return;
                                    }
                                    else
                                    {
                                        // Check if customid was changed for an already known user
                                        // If true update contacts and mappedcontacts
                                        var skypeNameResolver = new SkypeNameResolver(this);
                                        string skypeName = skypeNameResolver.FindMapping(contact);

                                        if (MappedContacts.ContainsKey(contact.Name) && !MappedContacts[contact.Name].UserName.Equals(skypeName, StringComparison.OrdinalIgnoreCase))
                                        {
                                            var skypeUser = ClientInstance.User[skypeName];

                                            IContact<IUser, ISkype> internalContact = new SkypeContact(contact, skypeUser,
                                                                                               this);
                                            // Cleanup old com references
                                            MappedContacts[contact.Name].Dispose();
                                            // Assign new instance
                                            MappedContacts[contact.Name] = internalContact;
                                            Contacts[contact.Name] = contact;
                                            this.OnContactHostingUpdate(new MessengerContactEventArgs(contact,
                                           Resources.Resources.ServiceID,
                                           MessengerContactHostingStatus.Found));
                                            return;
                                        }
                                    }


                                    // User is already in our contacts and mappedcontacts list -> collab frameworks checks if user stil exists
                                    if (this.MappedContacts.ContainsKey(contact.Name))
                                        this.OnContactHostingUpdate(new MessengerContactEventArgs(contact,
                                            Resources.Resources.
                                                ServiceID,
                                            MessengerContactHostingStatus
                                                .Found));
                                    else
                                    {
                                        // A user is lost if no mapped contact exist
                                        // In most cases Skype is returning a skypehandle even if a skype user doesn't exist
                                        this.OnContactHostingUpdate(new MessengerContactEventArgs(contact,
                                            Resources.Resources.
                                                ServiceID,
                                            MessengerContactHostingStatus
                                                .Lost));
                                    }

                                    // Everything else means notfound
                                    /*OnContactHostingUpdate(new MessengerContactEventArgs(contact,
                                                                                         Resources.Resources.ServiceID,
                                                                                         MessengerContactHostingStatus.
                                                                                             NotFound));*/
                                });
        }

        /// <summary>
        /// Notify the Collab framework about provider level updates (not contact updates)
        /// </summary>
        /// <param name="e"></param>
        private void OnServiceStatusUpdate(MessengerProviderStatusEventArgs e)
        {
            EventHandler<MessengerProviderStatusEventArgs> handler = ServiceStatusUpdate;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        #region Helper methods

        private bool IsSkypeRunning
        {
            get
            {
                return (ClientInstance != null && ClientInstance.Client.IsRunning);
            }
        }

        #endregion
    }
}