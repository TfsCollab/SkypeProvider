#region

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using TfsCommunity.Collaboration.Skype.Enum;

#endregion

namespace TfsCommunity.Collaboration.Skype.Helpers
{
    /// <summary>
    /// Converting IUser status to a color for binding on Canvas control
    /// Color schema is same as Skype (consistency) 
    /// </summary>
    public class StateToColorConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is UserStatus)
            {
                switch ((UserStatus) value)
                {
                    case UserStatus.Online:
                        return new SolidColorBrush(System.Windows.Media.Color.FromRgb(170, 219, 89));
                    case UserStatus.Offline:
                        return new SolidColorBrush(System.Windows.Media.Color.FromRgb(171, 171, 171));;
                    case UserStatus.Away:
                        return new SolidColorBrush(System.Windows.Media.Color.FromRgb(247, 206, 49));
                    case UserStatus.DoNotDisturb:
                        return new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 79, 60));
                    case UserStatus.Invisible:
                        return new SolidColorBrush(System.Windows.Media.Color.FromRgb(171, 171, 171));
                    default:
                        return new SolidColorBrush(System.Windows.Media.Color.FromRgb(171, 171, 171));
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