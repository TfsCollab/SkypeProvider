#region

using System.Collections.Generic;
using Microsoft.TeamFoundation.Collaboration;
using SKYPE4COMLib;
using TfsCommunity.Collaboration.Skype.Controls;

#endregion

namespace TfsCommunity.Collaboration.Skype.Interfaces
{
    public interface ICollaborationUIProviderExtended<TClientInstance> : ICollaborationUIProvider
    {
        // IMClient Instance
        TClientInstance ClientInstance { get; }

        //Contact Name, custom control object -> Contact name is from TFSCollab framework
        Dictionary<string, IContactControl<TClientInstance>> MappedContactControls { get; }

        //Contact Name, custom contact object (stores all user data, used for binding wpf control)
        Dictionary<string, IContact<IUser, ISkype>> MappedContacts { get; }

        //Contact Name, internal object 
        Dictionary<string, ContactData> Contacts { get; }

    }
}