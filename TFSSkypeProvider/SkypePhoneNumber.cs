using Microsoft.TeamFoundation.Collaboration;
using SKYPE4COMLib;

namespace TfsCommunity.Collaboration.Skype
{
    /// <summary>
    /// Provides an implementation of <see cref="PhoneNumber"/> for skype.
    /// </summary>
    public class SkypePhoneNumber : PhoneNumber
    {
        private readonly string _phoneNumber = string.Empty;
        private readonly string _phoneType;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkypePhoneNumber"/> class.
        /// </summary>
        /// <param name="skypeUser">The skype user.</param>
        /// <param name="phoneType">Type of the phone.</param>
        public SkypePhoneNumber(IUser skypeUser, PhoneType phoneType)
        {
            if (phoneType == PhoneType.Home)
            {
                _phoneNumber = skypeUser.PhoneHome;
                _phoneType = HomePhone;
            }
            if (phoneType == PhoneType.Mobile)
            {
                _phoneNumber = skypeUser.PhoneMobile;
                _phoneType = MobilePhone;
            }
            if (phoneType == PhoneType.Office)
            {
                _phoneNumber = skypeUser.PhoneOffice;
                _phoneType = WorkPhone;
            }
        }


        /// <summary>
        /// Gets the number.
        /// </summary>
        /// <value>The number.</value>
        public override string Number
        {
            get { return _phoneNumber; }
        }

        public override string PhoneNumberType
        {
            get { return _phoneType; }
        }

    }
}