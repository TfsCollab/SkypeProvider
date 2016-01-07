using System;
using System.Collections.Generic;
using Microsoft.TeamFoundation.Collaboration;
using SKYPE4COMLib;

namespace TfsCommunity.Collaboration.Skype
{
    public static class HelperClass
    {
        public static PresenceStatus UserStatusToPresenseStatus(this TUserStatus SkypeStatus)
        {
            switch (SkypeStatus)
            {
                case TUserStatus.cusOnline:
                    return PresenceStatus.Online;
                case TUserStatus.cusOffline:
                    return PresenceStatus.Offline;
                case TUserStatus.cusDoNotDisturb:
                    return PresenceStatus.DoNotDisturb;
                case TUserStatus.cusAway:
                    return PresenceStatus.BeRightBack;
                case TUserStatus.cusInvisible:
                    return PresenceStatus.Invisible;
                case TUserStatus.cusNotAvailable:
                    return PresenceStatus.Away;
                case TUserStatus.cusSkypeMe:
                    return PresenceStatus.Custom;
                case TUserStatus.cusUnknown:
                    return PresenceStatus.Unknown;
                default:
                    return PresenceStatus.Unknown;
            }
        }

        public static PresenceStatus OnlineStatusToPresenceStatus(this TOnlineStatus SkypeStatus)
        {
            switch (SkypeStatus)
            {
                case TOnlineStatus.olsOnline:
                    return PresenceStatus.Online;
                case TOnlineStatus.olsOffline:
                    return PresenceStatus.Offline;
                case TOnlineStatus.olsDoNotDisturb:
                    return PresenceStatus.DoNotDisturb;
                case TOnlineStatus.olsNotAvailable:
                    return PresenceStatus.BeRightBack;
                case TOnlineStatus.olsAway:
                    return PresenceStatus.Away;
                case TOnlineStatus.olsSkypeMe:
                    return PresenceStatus.Custom;
                case TOnlineStatus.olsUnknown:
                    return PresenceStatus.Unknown;
                default:
                    return PresenceStatus.Custom;
            }
        }

        public static TOnlineStatus PresenceStatusToOnlineStatus(this PresenceStatus PresenceStatus)
        {
            switch (PresenceStatus)
            {
                case PresenceStatus.Online:
                    {
                        return TOnlineStatus.olsOnline;
                    }
                case PresenceStatus.Offline:
                    {
                        return TOnlineStatus.olsOffline;
                    }
                case PresenceStatus.OnThePhone:
                    {
                        return TOnlineStatus.olsNotAvailable;
                    }
                case PresenceStatus.OutOfOffice:
                    {
                        return TOnlineStatus.olsNotAvailable;
                    }
                case PresenceStatus.OutToLunch:
                    {
                        return TOnlineStatus.olsNotAvailable;
                    }
                case PresenceStatus.Away:
                    {
                        return TOnlineStatus.olsAway;
                    }
                case PresenceStatus.Unknown:
                    {
                        return TOnlineStatus.olsUnknown;
                    }
                case PresenceStatus.DoNotDisturb:
                    {
                        return TOnlineStatus.olsDoNotDisturb;
                    }
                case PresenceStatus.Custom:
                    {
                        return TOnlineStatus.olsUnknown;
                    }
                default:
                    {
                        return TOnlineStatus.olsUnknown;
                    }
            }
        }

        public static void AddFromContactIds(this UserCollection users, IList<string> contactIds, UserMappingCollection mappings, string myContactId)
        {
            foreach (string contactId in contactIds)
            {
                if ((mappings.FindBySkypeName(contactId) != null || mappings[contactId] != null))
                {
                    if (contactId != myContactId)
                        users.Add(new User { Handle = mappings.GetUserMapping(contactId).SkypeName });
                }
            }
        }

        public static User[] ToArray(this UserCollection users)
        {
            var result = new User[users.Count];
            for (int i = 0; i < users.Count; i++)
            {
                result[i] = users[i + 1];
            }
            Array.Sort(result, new UserComparer());
            return result;
        }
    }

    internal class UserComparer : IComparer<User>
    {
        #region Implementation of IComparer<User>

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <returns>
        /// Value  Condition  Less than zero
        /// <paramref name="x" /> is less than <paramref name="y" />. Zero
        /// <paramref name="x" /> equals <paramref name="y" />. Greater than zero
        /// <paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        /// <param name="x"> The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        public int Compare(User x, User y)
        {
            if ((x != null) && (y != null))
            {
                return x.FullName.CompareTo(y.FullName);
            }
            return -1;
        }

        #endregion
    }
}