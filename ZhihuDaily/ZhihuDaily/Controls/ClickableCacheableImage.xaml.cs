using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Brain.Animate;
using Caliburn.Micro;
using Shagu.Utils.Utils;
using ZhihuDaily.Util;

namespace ZhihuDaily.Controls
{
    public sealed partial class ClickableCacheableImage
    {
        private readonly GlobalInfoManager _globalInfoManager;
        public ClickableCacheableImage()
        {
            InitializeComponent();

            _globalInfoManager = IoC.Get<GlobalInfoManager>();
        }


        public static readonly DependencyProperty DefaultImageUriProperty = DependencyProperty.Register(
            "DefaultImageUri", typeof (Uri), typeof (ClickableCacheableImage),
            new PropertyMetadata(default(Uri), DefaultImageUriChangedCallback));

        private static void DefaultImageUriChangedCallback(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var view = (ClickableCacheableImage) dependencyObject;
            var image = view.mainImage;
            image.Source = new BitmapImage(view.DefaultImageUri);
        }
        

        public Uri DefaultImageUri
        {
            get { return (Uri) GetValue(DefaultImageUriProperty); }
            set { SetValue(DefaultImageUriProperty, value); }
        }

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(
            "Stretch", typeof (Stretch), typeof (ClickableCacheableImage), new PropertyMetadata(Stretch.UniformToFill));

        public Stretch Stretch
        {
            get { return (Stretch) GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        public static readonly DependencyProperty ImageUrlProperty = DependencyProperty.Register(
            "ImageUrl", typeof (string), typeof (ClickableCacheableImage),
            new PropertyMetadata(default(string), ImageUriChangedCallback));

        public string ImageUrl
        {
            get { return (string) GetValue(ImageUrlProperty); }
            set { SetValue(ImageUrlProperty, value); }
        }

        private static async void ImageUriChangedCallback(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var view = (ClickableCacheableImage)dependencyObject;
            if (view._globalInfoManager.NoPicture)
            {
                var image = view.mainImage;
                image.Source = new BitmapImage(view.DefaultImageUri);
            }
            else
            {
                var uri = await ImageUtil.LoadImage(view.ImageUrl, view.LoadNew);
                view.mainImage.Source = uri != null ? new BitmapImage(uri) : new BitmapImage(view.DefaultImageUri);
                await view.mainImage.AnimateAsync(new FadeInAnimation());
            }
        }

        public bool LoadNew { get; set; }


        //点击显示图片详情
        public bool IsShowImageViewer { get; set; }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (IsShowImageViewer)
            {
                if (!string.IsNullOrEmpty(ImageUrl))
                {
                    ImageViewer.ShowDialog(ImageUrl);
                }
            }
        }
    }
}