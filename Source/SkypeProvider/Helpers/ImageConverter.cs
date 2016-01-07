#region

using System;
using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

#endregion

namespace TfsCommunity.Collaboration.Skype.Helpers
{
    public class ImageConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            using (var bitmap = value as Bitmap)
            {
                if (bitmap != null)
                    return Convert(bitmap);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

        public static BitmapSource Convert(Bitmap value)
        {
            if (null == value)
                throw new ArgumentNullException("value");

            return Imaging.CreateBitmapSourceFromHBitmap(
                value.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }
    }
}