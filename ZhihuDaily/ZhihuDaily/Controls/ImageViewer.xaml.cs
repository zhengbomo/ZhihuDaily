using System;
using System.IO;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Caliburn.Micro;
using ZhihuDaily.Util;
using Shagu.Utils;
using Shagu.Utils.Utils;

namespace ZhihuDaily.Controls
{
    public sealed partial class ImageViewer
    {
        private Popup _popup;
        public ImageViewer()
        {
            this.InitializeComponent();
        }

        public static void ShowDialog(string imageUrl)
        {
            var imageViewer = new ImageViewer { ImageUrl = imageUrl };
            imageViewer._popup = new Popup
            {
                Child = new Border
                {
                    Width = Window.Current.Bounds.Width,
                    Height = Window.Current.Bounds.Height,
                    Child = imageViewer
                },
                IsOpen = true
            };
            imageViewer.Show();


            WindowSizeChangedEventHandler sizeChangeHalHandler = (s, e) =>
            {
                ((Border)imageViewer._popup.Child).Width = e.Size.Width;
                ((Border)imageViewer._popup.Child).Height = e.Size.Height;
            };

            Window.Current.SizeChanged += sizeChangeHalHandler;
            imageViewer._popup.Closed += (s, e) =>
            {
                Window.Current.SizeChanged -= sizeChangeHalHandler;
            };
            
        }
        

        public void Show()
        {
            PopInStoryboard.Begin();
        }

        public void Hide()
        {
            PopOutStoryboard.Begin();
        }

        private void PopOutStoryboard_OnCompleted(object sender, object e)
        {
            Visibility = Visibility.Collapsed;
            _popup.IsOpen = false;
        }

        public static readonly DependencyProperty ImageUrlProperty = DependencyProperty.Register(
            "ImageUrl", typeof (string), typeof (ImageViewer), new PropertyMetadata(default(string), ImageUrlChangedCallback));

        private static async void ImageUrlChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var view = (ImageViewer) dependencyObject;
            await ImageUtil.LoadImage(view.image, null, view.ImageUrl, true, async bytes =>
            {
                Stream stream = new MemoryStream(bytes);
                var writeableBitmap = new WriteableBitmap(1, 1);
                writeableBitmap = await writeableBitmap.FromStream(stream);
                view.UpdateImage(writeableBitmap);
            }, () =>
            {
                ToastHelper.ToastError("加载失败");
            });

            view.progressRing.Visibility = Visibility.Collapsed;
        }

        private void UpdateImage(BitmapSource bitmap)
        {
            if (bitmap.PixelWidth > ActualWidth || bitmap.PixelHeight > ActualHeight)
            {
                image.Stretch = Stretch.Uniform;
                image.Source = bitmap;

                var size = CalulateSize(new Size(bitmap.PixelWidth, bitmap.PixelHeight), new Size(ActualWidth, ActualHeight));
                image.Width = size.Width;
                image.Height = size.Height;
            }
            else
            {
                image.Stretch = Stretch.None;
                image.Source = bitmap;

                image.Height = bitmap.PixelHeight;
                image.Width = bitmap.PixelWidth;
            }

            zoomOutBtn.IsEnabled = zoomInBtn.IsEnabled = saveBtn.IsEnabled = true;
        }

        private static Size CalulateSize(Size originSize, Size maxSize)
        {
            var originRate = originSize.Width/originSize.Height;
            var maxRate = maxSize.Width/maxSize.Height;

            if (originRate > maxRate)
            {
                return new Size(maxSize.Width, maxSize.Width/originRate);
            }
            else
            {
                return new Size(maxSize.Height*originRate, maxSize.Height);
            }
        }



        public string ImageUrl
        {
            get { return (string) GetValue(ImageUrlProperty); }
            set { SetValue(ImageUrlProperty, value); }
        }

        private void ZoomOutButton_OnClick(object sender, RoutedEventArgs e)
        {
            scrollViewer.ChangeView(null, null, (float)(scrollViewer.ZoomFactor + 0.5));
        }
        private void ZoomInButton_OnClick(object sender, RoutedEventArgs e)
        {
            scrollViewer.ChangeView(null, null, (float)(scrollViewer.ZoomFactor - 0.5));
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private async void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var value = ImageUrl;
            if (value.EndsWith("!640x"))
            {
                value = value.Substring(0, value.Length - 5);
            }
            var fileName = string.Format("cacheImage\\{0}", Md5Helper.ComputeHash(value));
            var bytes = await RequestUtilBase.CacheFile(ImageUrl, fileName);
            var globalInfoManager = IoC.Get<GlobalInfoManager>();
            if (bytes != null)
            {
                var folder = await KnownFolders.PicturesLibrary.CreateFolderAsync("片刻UWP", CreationCollisionOption.OpenIfExists);
                var file = await folder.CreateFileAsync(string.Format("pianke_{0}.jpg", DateTime.Now.Ticks), CreationCollisionOption.GenerateUniqueName);
                await FileManager.WriteBytes(file, bytes);

                ToastHelper.ToastInfo("图片已保存到我的图片中", globalInfoManager.IsNightMode);
            }
            else
            {
                ToastHelper.ToastError("图片保存失败", globalInfoManager.IsNightMode);
            }
        }
    }
}
