using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.TeamFoundation.Collaboration;
using SKYPE4COMLib;

namespace TfsCommunity.Collaboration.Skype
{
    internal class SkypePhoneCollection : PhoneNumberCollection
    {
        private readonly List<PhoneNumber> phoneNumbers;

        public SkypePhoneCollection(IUser skypeUser)
        {
            phoneNumbers = new List<PhoneNumber>();
            if (!string.IsNullOrEmpty(skypeUser.PhoneHome))
            {
                PhoneNumber homePhoneNumber = new SkypePhoneNumber(skypeUser, PhoneType.Home);
                phoneNumbers.Add(homePhoneNumber);
            }
            if (!string.IsNullOrEmpty(skypeUser.PhoneMobile))
            {
                PhoneNumber mobilePhoneNumber = new SkypePhoneNumber(skypeUser, PhoneType.Mobile);
                phoneNumbers.Add(mobilePhoneNumber);
            }
            if (!string.IsNullOrEmpty(skypeUser.PhoneOffice))
            {
                PhoneNumber officePhoneNumber = new SkypePhoneNumber(skypeUser, PhoneType.Office);
                phoneNumbers.Add(officePhoneNumber);
            }
        }

        #region Overrides of PhoneNumberCollection

        /// <summary>
        /// Gets the number of phone numbers within the collection.
        /// </summary>
        /// <value>The count.</value>
        public override int Count
        {
            get { return phoneNumbers.Count; }
        }

        /// <summary>
        /// Gets the <see cref="Microsoft.TeamFoundation.Collaboration.PhoneNumber"/> at the specified index.
        /// </summary>
        /// <value></value>
        public override PhoneNumber this[int index]
        {
            get { return phoneNumbers[index]; }
        }

        /// <summary>
        /// Gets the <see cref="Microsoft.TeamFoundation.Collaboration.PhoneNumber"/> with the specified type.
        /// </summary>
        /// <value></value>
        public override PhoneNumber this[string type]
        {
            get
            {
                foreach (PhoneNumber phoneNumber in phoneNumbers)
                {
                    if (phoneNumber.Type == type)
                    {
                        return phoneNumber;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Determines whether this instance contains the specified phone number type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// 	<c>true</c> if this instance contains the specified phone number type; otherwise, <c>false</c>.
        /// </returns>
        public override bool Contains(string type)
        {
            foreach (PhoneNumber phoneNumber in phoneNumbers)
            {
                if (phoneNumber.Type == type)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<PhoneNumber> GetEnumerator()
        {
            return phoneNumbers.GetEnumerator();
        }

        #endregion
    }
}