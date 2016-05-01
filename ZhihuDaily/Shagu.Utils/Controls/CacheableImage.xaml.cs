using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Brain.Animate;
using Shagu.Utils.Utils;

namespace Shagu.Utils.Controls
{
    public sealed partial class CacheableImage 
    {
        public CacheableImage()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty DefaultImageUriProperty = DependencyProperty.Register(
            "DefaultImageUri", typeof (Uri), typeof (CacheableImage), new PropertyMetadata(default(Uri), DefaultImageUriChangedCallback));

        private static void DefaultImageUriChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var view = (CacheableImage)dependencyObject;
            var image = view.mainImage;
            image.Source = new BitmapImage(view.DefaultImageUri);
        }

        
        public Uri DefaultImageUri
        {
            get { return (Uri) GetValue(DefaultImageUriProperty); }
            set { SetValue(DefaultImageUriProperty, value); }
        }

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(
            "Stretch", typeof (Stretch), typeof (CacheableImage), new PropertyMetadata(Stretch.UniformToFill));

        public Stretch Stretch
        {
            get { return (Stretch) GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        public static readonly DependencyProperty ImageUrlProperty = DependencyProperty.Register(
            "ImageUrl", typeof (string), typeof (CacheableImage), new PropertyMetadata(default(string), ImageUriChangedCallback));
        
        public string ImageUrl
        {
            get { return (string) GetValue(ImageUrlProperty); }
            set { SetValue(ImageUrlProperty, value); }
        }

        private static async void ImageUriChangedCallback(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var view = (CacheableImage) dependencyObject;
            var uri = await ImageUtil.LoadImage(view.ImageUrl, view.LoadNew);
            view.mainImage.Source = uri != null ? new BitmapImage(uri) : new BitmapImage(view.DefaultImageUri);
            await view.mainImage.AnimateAsync(new FadeInAnimation());
        }

        public static readonly DependencyProperty IsNightModeProperty = DependencyProperty.Register(
            "IsNightMode", typeof (bool), typeof (CacheableImage), new PropertyMetadata(default(bool), PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var image = (CacheableImage) dependencyObject;
            image.maskRect.Visibility = image.IsNightMode ? Visibility.Visible : Visibility.Collapsed;
        }

        public bool IsNightMode
        {
            get { return (bool) GetValue(IsNightModeProperty); }
            set { SetValue(IsNightModeProperty, value); }
        }

        public bool LoadNew { get; set; }
    }
}
