#region

using System;
using System.Globalization;
using System.Windows.Data;
using TfsCommunity.Collaboration.Skype.Enum;

#endregion

namespace TfsCommunity.Collaboration.Skype.Helpers
{
    /// <summary>
    /// Returns a picture foreach known state
    /// For consistency reasons we use the official skype icons
    /// </summary>
    public class StateToImageSourceConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is UserStatus)
            {
                switch ((UserStatus) value)
                {
                    case UserStatus.Online:
                        return ImageConverter.Convert(Resources.Resources.PresenceOnline);
                    case UserStatus.Offline:
                        return ImageConverter.Convert(Resources.Resources.PresenceOffline);
                    case UserStatus.Away:
                        return ImageConverter.Convert(Resources.Resources.PresenceAway);
                    case UserStatus.DoNotDisturb:
                        return ImageConverter.Convert(Resources.Resources.PresenceDoNotDisturb);
                    case UserStatus.Invisible:
                        return ImageConverter.Convert(Resources.Resources.PresenceInvisible);
                    default:
                        return ImageConverter.Convert(Resources.Resources.PresenceOffline);
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}