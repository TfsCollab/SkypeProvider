#region

using SKYPE4COMLib;
using TfsCommunity.Collaboration.Skype.Enum;

#endregion

namespace TfsCommunity.Collaboration.Skype.Helpers
{
    /// <summary>
    ///   TODO: Update summary.
    /// </summary>
    public static class StatusConverter
    {
        public static UserStatus ToUserStatus(this TOnlineStatus onlineStatus)
        {
            switch (onlineStatus)
            {
                case TOnlineStatus.olsOnline:
                    {
                        return UserStatus.Online;
                    }
                case TOnlineStatus.olsAway:
                    {
                        return UserStatus.Away;
                    }
                case TOnlineStatus.olsOffline:
                    {
                        return UserStatus.Offline;
                    }
                case TOnlineStatus.olsDoNotDisturb:
                    {
                        return UserStatus.DoNotDisturb;
                    }
                default:
                    {
                        return UserStatus.Unkown;
                    }
            }
        }
    }
}