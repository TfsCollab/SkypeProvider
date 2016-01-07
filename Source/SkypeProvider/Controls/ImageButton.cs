#region

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

#endregion

namespace TfsCommunity.Collaboration.Skype.Controls
{
    /// <summary>
    ///   Custom Control for changing easily the image of the button 
    ///   Image depends on state of button - enabled / disabled
    /// </summary>
    public class ImageButton : Button
    {
        #region Properties

        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
            "ImageSource", typeof (BitmapSource), typeof (ImageButton), new PropertyMetadata(default(BitmapSource)));

        public static readonly DependencyProperty DisabledImageSourceProperty = DependencyProperty.Register(
            "DisabledImageSource", typeof (BitmapSource), typeof (ImageButton),
            new PropertyMetadata(default(BitmapSource)));

        // Image for enabled state of control
        public BitmapSource ImageSource
        {
            get { return (BitmapSource) GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        // Image for disabled state of control
        public BitmapSource DisabledImageSource
        {
            get { return (BitmapSource) GetValue(DisabledImageSourceProperty); }
            set { SetValue(DisabledImageSourceProperty, value); }
        }

        #endregion

        #region constructor

        #endregion
    }
}