using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Imaging;
using XamlBrewer.Uwp.Controls;

namespace ZhihuDaily.Controls
{
    public sealed partial class CropImageView
    {
        private bool _isCancel;
        private Popup _popup;
        private readonly SemaphoreSlim _semaphore;

        public CropImageView()
        {
            this.InitializeComponent();
            _semaphore = new SemaphoreSlim(0,1);
        }

        public static async Task<WriteableBitmap> ShowDialog(StorageFile imgFile)
        {
            var imageViewer = new CropImageView();

            var wb = new WriteableBitmap(1, 1);
            await wb.LoadAsync(imgFile);
            imageViewer.ImageCropper.SourceImage = wb;

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
            await imageViewer._semaphore.WaitAsync();
            return imageViewer._isCancel ? null : imageViewer.ImageCropper.CroppedImage;
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


        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Hide();
            _isCancel = true;
            _semaphore.Release();
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            Hide();
            _semaphore.Release();
        }
    }
}
