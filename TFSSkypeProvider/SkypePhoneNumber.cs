using Microsoft.TeamFoundation.Collaboration;
using SKYPE4COMLib;

namespace TfsCommunity.Collaboration.Skype
{
    /// <summary>
    /// Provides an implementation of <see cref="PhoneNumber"/> for skype.
    /// </summary>
    public class SkypePhoneNumber : PhoneNumber
    {
        private readonly string phoneNumber = string.Empty;
        private readonly string phoneType;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkypePhoneNumber"/> class.
        /// </summary>
        /// <param name="skypeUser">The skype user.</param>
        /// <param name="phoneType">Type of the phone.</param>
        public SkypePhoneNumber(IUser skypeUser, PhoneType phoneType)
        {
            if (phoneType == PhoneType.Home)
            {
                phoneNumber = skypeUser.PhoneHome;
                this.phoneType = HomePhone;
            }
            if (phoneType == PhoneType.Mobile)
            {
                phoneNumber = skypeUser.PhoneMobile;
                this.phoneType = MobilePhone;
            }
            if (phoneType == PhoneType.Office)
            {
                phoneNumber = skypeUser.PhoneOffice;
                this.phoneType = WorkPhone;
            }
        }


        /// <summary>
        /// Gets the number.
        /// </summary>
        /// <value>The number.</value>
        public override string Number
        {
            get { return phoneNumber; }
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public override string Type
        {
            get { return phoneType; }
        }
    }
}