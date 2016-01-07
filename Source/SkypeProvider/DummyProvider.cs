using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.TeamFoundation.Collaboration;
using TfsCommunity.Collaboration.Skype.Interfaces;

namespace TfsCommunity.Collaboration.Skype
{
    [TeamMembersProvider("E5F7B919-7945-4FB5-9933-AEB58870D698", "Dummy", true)]
    public class DummyProvider : ICollaborationUIProvider
    {
        public void Dispose()
        {

        }

        public object ContactPanel(ContactData contact)
        {
            return new UserControl();
        }

        public void Clear()
        {

        }

        public void RefreshAsync()
        {

        }

        public void InitializeAsync()
        {
            this.CanConnect = false;
            this.IsConnected = true;
            this.CanGetArbitraryContactID = false;
        }

        public void ConnectAsync()
        {

        }

        public void Disconnect()
        {

        }

        public void LocateContactAsync(ContactData contact)
        {

        }

        public string GetArbitraryContactID()
        {
            return string.Empty;
        }

        public bool IsConnected { get; private set; }
        public bool CanConnect { get; private set; }
        public bool AllowsSettingOthersContactIDs { get; private set; }
        public string FriendlyName { get; private set; }
        public string MyContactId { get; private set; }
        public bool CanGetArbitraryContactID { get; private set; }
        public event EventHandler<MessengerContactEventArgs> ContactHostingUpdate;
        public event EventHandler<MessengerProviderStatusEventArgs> ServiceStatusUpdate;
    }
}
