using TfsCommunity.Collaboration.Skype.Interfaces;

namespace TfsCommunity.Collaboration.Skype.Controls
{
    public interface IContactControl<T>
    {
        // ContactName
        string Source { get; set; }

        ICollaborationUIProviderExtended<T> Provider { get; set; }

        void Refresh();
    }
}