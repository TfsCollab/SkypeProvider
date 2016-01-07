#region

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.Collaboration;
using SKYPE4COMLib;
using TfsCommunity.Collaboration.Skype.Legacy;

#endregion

namespace TfsCommunity.Collaboration.Skype.Helpers
{
    using System.Text;

    public class SkypeNameResolver
    {
        #region Fields

        private readonly SkypeProvider _provider;

        #endregion

        #region Constructor

        public SkypeNameResolver(SkypeProvider provider)
        {
            if (null == provider)
            {
                throw new ArgumentNullException("provider");
            }

            _provider = provider;
        }

        #endregion

        #region methods
        /// <summary>
        /// Find a mapping for a given contactdata object
        /// We use different methods to find a skype handle
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        public string FindMapping(ContactData contact)
        {
            // Extract skypeName from TFS Power Tools 2012 custom id
            string skypeNameCustomId = FindSkypeUserByCustomId(contact);
            if (!string.IsNullOrEmpty(skypeNameCustomId))
            {
                return skypeNameCustomId;
            }

            // Find skypename from 2012 skype provider mapping file
            string skypeName2012Mapping = FindSkypeUserIn2012MappingFile(contact);
            if (!string.IsNullOrEmpty(skypeName2012Mapping))
            {
                return skypeName2012Mapping;
            }

            // Find skypename from old 2008 + 2010 skype provider mapping file
            string skypeName2008Mapping = FindSkypeUserIn20082010MappingFile(contact);
            if (!string.IsNullOrEmpty(skypeName2008Mapping))
            {
                return skypeName2008Mapping;
            }

            // Find skypename in Skype contact list
            string skypeNameFromSkypeContactList = FindSkypeUserInSkypeContactList(contact);
            if (!string.IsNullOrEmpty(skypeNameFromSkypeContactList))
            {
                return skypeNameFromSkypeContactList;
            }

            // Found nothing
            Logger.Write(string.Format("Found no mapping for user {0}.", contact.Name));
            return string.Empty;
        }

        private string FindSkypeUserIn20082010MappingFile(ContactData contact)
        {
            Logger.Write(string.Format("Find FindSkypeUserIn20082010MappingFile was executed for user {0}", contact.Name));
            // Maybe we replace later the old deserializer by an xml linq query
            var mappedUsers = new UserMappingCollection();
            mappedUsers.Load();

            // the old power tools has used the mail adress for identifing users in communicator / messenger
            IEnumerable<string> result;
            try
            {
                result = from user in mappedUsers
                         where
                             user.TfsName.ToLower().Equals(contact.Email, StringComparison.InvariantCultureIgnoreCase) &
                             !user.IsIgnored &
                             !user.IsUnassigned
                         select user.SkypeName;
            }

            catch (Exception ex)
            {
                return string.Empty;
            }

            // if more than one result was found we return the first mapping
            // ReSharper disable PossibleMultipleEnumeration
            if (result.Any())
            // ReSharper restore PossibleMultipleEnumeration
            {
                // ReSharper disable PossibleMultipleEnumeration
                Logger.Write(string.Format("Found mapping for user {0}. skypeName {1}", contact.Name,
                                           result.FirstOrDefault()));
                // ReSharper restore PossibleMultipleEnumeration
                // ReSharper disable PossibleMultipleEnumeration
                return result.FirstOrDefault();
                // ReSharper restore PossibleMultipleEnumeration
            }

            // if no mapping was found we return an empty string
            Logger.Write(string.Format("Found no mapping for user {0}.", contact.Name));
            return string.Empty;
        }

        private string FindSkypeUserIn2012MappingFile(ContactData contact)
        {
            Logger.Write(string.Format("Find FindSkypeUserIn2012MappingFile was executed for user {0}", contact.Name));
            // Maybe we replace later the old deserializer by an xml linq query
            var mappedUsers = new Mapping.UserMappingCollection();
            mappedUsers.Load();

            // the old power tools has used the mail adress for identifing users in communicator / messenger
            IEnumerable<string> result;
            try
            {
                result = from user in mappedUsers
                         where
                             user.TfsContactName.ToLower().Equals(contact.Email,StringComparison.InvariantCultureIgnoreCase) &
                             !user.IsIgnored &
                             !user.IsUnassigned &
                             user.ProtocolName.Equals("Skype", StringComparison.InvariantCultureIgnoreCase)
                         select user.IMClientName;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }


            // if more than one result was found we return the first mapping
            // ReSharper disable PossibleMultipleEnumeration
            if (result.Any())
            // ReSharper restore PossibleMultipleEnumeration
            {
                // ReSharper disable PossibleMultipleEnumeration
                Logger.Write(string.Format("Found mapping for user {0}. skypeName {1}", contact.Name,
                                           result.FirstOrDefault()));
                // ReSharper restore PossibleMultipleEnumeration
                // ReSharper disable PossibleMultipleEnumeration
                return result.FirstOrDefault();
                // ReSharper restore PossibleMultipleEnumeration
            }

            // if no mapping was found we return an empty string
            Logger.Write(string.Format("Found no mapping for user {0}.", contact.Name));
            return string.Empty;
        }

        private string FindSkypeUserInSkypeContactList(ContactData contact)
        {
            Logger.Write(string.Format("Find FindSkypeUserInSkypeContactList was executed for user {0}", contact.Name));
            foreach (IUser friend in _provider.ClientInstance.Friends)
            {
                // Remove the login name from contact.Name because if user is duplicate TFS is attaching the loginname
                string contactName = contact.Name;
                // If duplcate users exists we are only using the displayname part (without userlogin), e.g. Nico Orschel (domain\someuser) -> result: Nico Orschel
                if (contact.Name.Contains("(") & contact.Name.Contains(")"))
                {
                    contactName = contact.Name.Substring(0, contact.Name.IndexOf("(", StringComparison.Ordinal) - 1);
                }

                // All checks regarding name

                // First check the fullname
                if (friend.FullName.Equals(contactName, StringComparison.OrdinalIgnoreCase))
                {
                    Logger.Write(string.Format("Found fullname mapping for user {0}. skypeName {1}", contact.Name, friend.Handle));
                    return friend.Handle;
                }

                // Secound check the Alias
                if (friend.Aliases.ToLower().Contains(contactName.ToLower()))
                {
                    Logger.Write(string.Format("Found alias mapping for user {0}. skypeName {1}", contact.Name, friend.Handle));
                    return friend.Handle;
                }

                // Third check the displayname
                if (friend.DisplayName.ToLower().Contains(contactName.ToLower()))
                {
                    Logger.Write(string.Format("Found displayname mapping for user {0}. skypeName {1}", contact.Name, friend.Handle));
                    return friend.Handle;
                }

                // All checks regarding skype handle

                // Fourth check is the first part of the email adress is same as skype handle
                if (!string.IsNullOrEmpty(contact.Email))
                {
                    string usernameFromMailAdress = contact.Email.Substring(0,
                                                                            contact.Email.IndexOf("@",
                                                                                                  StringComparison.OrdinalIgnoreCase
                                                                                                      ));
                    Logger.Write(string.Format("Trying username from mail {0}", usernameFromMailAdress));
                    //check handle -> loginname
                    if (friend.Handle.Equals(usernameFromMailAdress, StringComparison.OrdinalIgnoreCase))
                    {
                        Logger.Write(string.Format("Found usernamefromemail mapping (handle) for user {0}. skypeName {1}", contact.Name,
                                                   friend.Handle));
                        return friend.Handle;
                    }

                    //check fullname
                    if (friend.FullName.Equals(usernameFromMailAdress.Replace(".", " "), StringComparison.OrdinalIgnoreCase))
                    {
                        Logger.Write(string.Format("Found usernamefromemail mapping (fullname) for user {0}. skypeName {1}", contact.Name,
                                                   friend.Handle));
                        return friend.Handle;
                    }

                    //check displayname
                    if (friend.DisplayName.ToLower().Contains(usernameFromMailAdress.Replace(".", " ").ToLower()))
                    {
                        Logger.Write(string.Format("Found usernamefromemail mapping (displayname) for user {0}. skypeName {1}", contact.Name,
                                                   friend.Handle));
                        return friend.Handle;
                    }

                    //check aliasname
                    if (friend.Aliases.ToLower().Contains(usernameFromMailAdress.Replace(".", " ").ToLower()))
                    {
                        Logger.Write(string.Format("Found usernamefromemail mapping (aliases) for user {0}. skypeName {1}", contact.Name,
                                                   friend.Handle));
                        return friend.Handle;
                    }
                }

                // Sixth check if a concated name (e.g. Nico Orschel) is a valid skype name (e.g. nicoorschel)
                string truncatedTfsName = RemoveBlankCharacters(contactName);
                if (friend.Handle.Equals(truncatedTfsName, StringComparison.OrdinalIgnoreCase))
                {
                    Logger.Write(string.Format("Found truncatedtfsname mapping for user {0}. skypeName {1}", contact.Name,
                                                   friend.Handle));
                    return friend.Handle;
                }

                //Seventh check: replace blank character with a dot -> sometimes it's possible that a email adress doesn't exist -> TFS without AD
                string dottedTfsName = ReplaceBlankCharactersWithDots(contactName);
                if (friend.Handle.Equals(dottedTfsName, StringComparison.OrdinalIgnoreCase))
                {
                    Logger.Write(string.Format("Found dottedtfsname mapping for user {0}. skypeName {1}", contact.Name,
                                                   friend.Handle));
                    return friend.Handle;
                }

            }

            // Found nothing
            Logger.Write(string.Format("Found no mapping for user {0}.", contact.Name));
            return string.Empty;
        }

        private string FindSkypeUserByCustomId(ContactData contact)
        {
            Logger.Write(string.Format("Find FindSkypeUserByCustomId was executed for user {0}", contact.Name));

            if (contact.ServiceIDs.ContainsKey(Resources.Resources.ServiceID)
                && !string.IsNullOrEmpty(contact.ServiceIDs[Resources.Resources.ServiceID]))
            {
                // if user doesn't exist we get an exception
                try
                {
                    var user = _provider.ClientInstance.User[contact.ServiceIDs[Resources.Resources.ServiceID]];
                    if (user.OnlineStatus != TOnlineStatus.olsUnknown)
                    {
                        Logger.Write(string.Format("Found mapping for user {0}. skypeName {1}", contact.Name,
                                                   user.Handle));
                        return user.Handle;
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteExceptionDetails(Resources.Resources.ProviderName, ex);
                    return string.Empty;
                }
            }

            // Found nothing
            Logger.Write(string.Format("Found no mapping for user {0}.", contact.Name));
            return string.Empty;
        }

        #endregion

        #region local helper methods
        private string RemoveBlankCharacters(string input)
        {
            var sb = new StringBuilder(input.Length);

            foreach (char i in input)
                //if (i != '\n' && i != '\r' && i != '\t')
                if (i != ' ')
                    sb.Append(i);

            input = sb.ToString();
            return input;
        }

        private string ReplaceBlankCharactersWithDots(string input)
        {
            Logger.Write(string.Format("ReplaceBlankCharactersWithDots was called with input {0}", input));
            var sb = new StringBuilder(input.Length);

            foreach (char i in input)
                //if (i != '\n' && i != '\r' && i != '\t')
                if (i != ' ')
                    sb.Append(i);
                else
                {
                    sb.Append(".");
                }

            input = sb.ToString();
            Logger.Write(string.Format("ReplaceBlankCharactersWithDots was called. Output is {0}", input));
            return input;
        }
        #endregion
    }


}