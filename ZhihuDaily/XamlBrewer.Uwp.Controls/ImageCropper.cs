using XamlBrewer.Uwp.Controls.Helpers;

namespace XamlBrewer.Uwp.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using Windows.Foundation;
    using Windows.Graphics.Imaging;
    using Windows.Storage;
    using Windows.Storage.Streams;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Media.Imaging;
    using Windows.UI.Xaml.Shapes;

    /// <summary>
    /// Image Control with Cropper.
    /// </summary>
    [TemplatePart(Name = SelectRegionPartName, Type = typeof(Path))]
    [TemplatePart(Name = TopLeftCornerPartName, Type = typeof(ContentControl))]
    [TemplatePart(Name = TopRightCornerPartName, Type = typeof(ContentControl))]
    [TemplatePart(Name = BottomLeftCornerPartName, Type = typeof(ContentControl))]
    [TemplatePart(Name = BottomRightCornerPartName, Type = typeof(ContentControl))]
    [TemplatePart(Name = LayoutRootPartName, Type = typeof(Grid))]
    [TemplatePart(Name = ImageCanvasPartName, Type = typeof(Canvas))]
    [TemplatePart(Name = SourceImagePartName, Type = typeof(Image))]
    public sealed class ImageCropper : Control, INotifyPropertyChanged
    {
        #region Constants
        private const string SelectRegionPartName = "PART_SelectRegion";

        private const string TopLeftCornerPartName = "PART_TopLeftCorner";
        private const string TopRightCornerPartName = "PART_TopRightCorner";
        private const string BottomLeftCornerPartName = "PART_BottomLeftCorner";
        private const string BottomRightCornerPartName = "PART_BottomRightCorner";

        private const int CornerSize = 30;

        private const string LayoutRootPartName = "PART_LayoutRoot";
        private const string ImageCanvasPartName = "PART_ImageCanvas";
        private const string SourceImagePartName = "PART_SourceImage";
        #endregion

        #region Fields

        private SelectedRegion selectedRegion;
        private Path selectRegion;
        private ContentControl topLeftCorner;
        private ContentControl topRightCorner;
        private ContentControl bottomLeftCorner;
        private ContentControl bottomRightCorner;
        private Canvas imageCanvas;
        private Image sourceImage;
        private Grid layoutRoot;

        private uint sourceImagePixelWidth;
        private uint sourceImagePixelHeight;

        private Dictionary<uint, Point?> pointerPositionHistory = new Dictionary<uint, Point?>();

        private StorageFile sourceImageFile = null;

        private WriteableBitmap croppedImage;

        #endregion

        #region Dependency Properties

        public WriteableBitmap SourceImage
        {
            get { return (WriteableBitmap)GetValue(SourceImageProperty); }
            set { SetValue(SourceImageProperty, value); }
        }

        public static readonly DependencyProperty SourceImageProperty =
            DependencyProperty.Register("SourceImage", typeof(WriteableBitmap), typeof(ImageCropper), new PropertyMetadata(null, OnSourceImageChanged));

        private static async void OnSourceImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var that = d as ImageCropper;
            var wb = e.NewValue as WriteableBitmap;

            // Save local copy.
            StorageFolder temp = ApplicationData.Current.LocalFolder;
            StorageFile file = await temp.CreateFileAsync("current_image.png", CreationCollisionOption.ReplaceExisting);
            await wb.SaveAsync(file);

            // Load
            await that.LoadImage(file);
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public ImageCropper()
        {
            this.DefaultStyleKey = typeof(ImageCropper);
            this.DataContext = this.selectedRegion;
        }

        public WriteableBitmap CroppedImage
        {
            get { return this.croppedImage; }
            private set
            {
                this.croppedImage = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("CroppedImage"));
                }
            }
        }

        /// <summary>
        /// Loads the image.
        /// </summary>
        /// <param name="imageFile">The image file.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">imageFile;Image is too small.</exception>
        private async Task LoadImage(StorageFile imageFile)
        {
            using (IRandomAccessStream fileStream = await imageFile.OpenAsync(Windows.Storage.FileAccessMode.Read))
            {
                this.sourceImageFile = imageFile;
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream);

                this.sourceImagePixelHeight = decoder.PixelHeight;
                this.sourceImagePixelWidth = decoder.PixelWidth;
            }

            if (this.sourceImagePixelHeight < 2 * CornerSize ||
                this.sourceImagePixelWidth < 2 * CornerSize)
            {
                // Image too small.
                throw new ArgumentOutOfRangeException("imageFile", "Image is too small.");
            }
            else
            {
                double sourceImageScale = 1;

                if (this.sourceImagePixelHeight > this.layoutRoot.ActualHeight ||
                    this.sourceImagePixelWidth > this.layoutRoot.ActualWidth)
                {
                    sourceImageScale = Math.Min(this.layoutRoot.ActualWidth / this.sourceImagePixelWidth,
                         this.layoutRoot.ActualHeight / this.sourceImagePixelHeight);
                }

                if (sourceImageScale == 0)
                {
                    // Control is invisible, unable to scale the source image.
                    return;
                }

                this.sourceImage.Source = await CropBitmap.GetCroppedBitmapAsync(
                    this.sourceImageFile,
                    new Point(0, 0),
                    new Size(this.sourceImagePixelWidth, this.sourceImagePixelHeight),
                    sourceImageScale);

                this.CroppedImage = null;
            }
        }

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call ApplyTemplate. In simplest terms, this means the method is called just before a UI element displays in your app. Override this method to influence the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            // Code might use some null reference checks here.

            this.layoutRoot = this.GetTemplateChild(LayoutRootPartName) as Grid;

            this.selectRegion = this.GetTemplateChild(SelectRegionPartName) as Path;
            this.selectRegion.ManipulationMode = ManipulationModes.Scale | ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            selectedRegion = new SelectedRegion { MinSelectRegionSize = 2 * CornerSize };
            this.DataContext = selectedRegion;

            this.topLeftCorner = this.GetTemplateChild(TopLeftCornerPartName) as ContentControl;
            this.topRightCorner = this.GetTemplateChild(TopRightCornerPartName) as ContentControl;
            this.bottomLeftCorner = this.GetTemplateChild(BottomLeftCornerPartName) as ContentControl;
            this.bottomRightCorner = this.GetTemplateChild(BottomRightCornerPartName) as ContentControl;

            this.imageCanvas = this.GetTemplateChild(ImageCanvasPartName) as Canvas;
            this.sourceImage = this.GetTemplateChild(SourceImagePartName) as Image;

            // Handle the pointer events of the corners.
            AddCornerEvents(this.topLeftCorner);
            AddCornerEvents(this.topRightCorner);
            AddCornerEvents(this.bottomLeftCorner);
            AddCornerEvents(this.bottomRightCorner);

            // Handle the manipulation events of the selectRegion
            this.selectRegion.ManipulationDelta += SelectRegion_ManipulationDelta;
            this.selectRegion.ManipulationCompleted += SelectRegion_ManipulationCompleted;

            this.sourceImage.SizeChanged += SourceImage_SizeChanged;
        }

        private void AddCornerEvents(Control corner)
        {
            corner.PointerPressed += Corner_PointerPressed;
            corner.PointerMoved += Corner_PointerMoved;
            corner.PointerReleased += Corner_PointerReleased;
        }

        /// <summary>
        /// If a pointer presses in the corner, it means that the user starts to move the corner.
        /// 1. Capture the pointer, so that the UIElement can get the Pointer events (PointerMoved,
        ///    PointerReleased...) even the pointer is outside of the UIElement.
        /// 2. Record the start position of the move.
        /// </summary>
        private void Corner_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            (sender as UIElement).CapturePointer(e.Pointer);

            Windows.UI.Input.PointerPoint pt = e.GetCurrentPoint(this);

            // Record the start point of the pointer.
            pointerPositionHistory[pt.PointerId] = pt.Position;

            e.Handled = true;
        }

        /// <summary>
        /// If a pointer which is captured by the corner moves, the select region will be updated.
        /// </summary>
        private void Corner_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Windows.UI.Input.PointerPoint pt = e.GetCurrentPoint(this);
            uint ptrId = pt.PointerId;

            if (pointerPositionHistory.ContainsKey(ptrId) && pointerPositionHistory[ptrId].HasValue)
            {
                Point currentPosition = pt.Position;
                Point previousPosition = pointerPositionHistory[ptrId].Value;

                double xUpdate = currentPosition.X - previousPosition.X;
                double yUpdate = currentPosition.Y - previousPosition.Y;

                var side = Math.Max(Math.Abs(xUpdate), Math.Abs(yUpdate));

                this.selectedRegion.UpdateCorner((sender as ContentControl).Tag as string,
                    side * Math.Sign(xUpdate),
                    side * Math.Sign(yUpdate));

                pointerPositionHistory[ptrId] = currentPosition;
            }

            e.Handled = true;
        }

        /// <summary>
        /// The pressed pointer is released, which means that the move is ended.
        /// 1. Release the Pointer.
        /// 2. Clear the position history of the Pointer.
        /// </summary>
        private void Corner_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            uint ptrId = e.GetCurrentPoint(this).PointerId;
            if (this.pointerPositionHistory.ContainsKey(ptrId))
            {
                this.pointerPositionHistory.Remove(ptrId);
            }

            (sender as UIElement).ReleasePointerCapture(e.Pointer);

            this.UpdatePreviewImage();
            e.Handled = true;
        }

        /// <summary>
        /// Update preview image.
        /// </summary>
        private async void UpdatePreviewImage()
        {
            double sourceImageWidthScale = this.imageCanvas.Width / this.sourceImagePixelWidth;
            double sourceImageHeightScale = this.imageCanvas.Height / this.sourceImagePixelHeight;

            Size previewImageSize = new Size(
                this.selectedRegion.SelectedRect.Width / sourceImageWidthScale,
                this.selectedRegion.SelectedRect.Height / sourceImageHeightScale);

            double previewImageScale = 1;

            if (previewImageSize.Width <= this.imageCanvas.Width &&
                previewImageSize.Height <= this.imageCanvas.Height)
            {
                previewImageScale = Math.Max(this.imageCanvas.Width / previewImageSize.Width,
                 this.imageCanvas.Height / previewImageSize.Height);
            }

            this.CroppedImage = await CropBitmap.GetCroppedBitmapAsync(
                   this.sourceImageFile,
                   new Point(this.selectedRegion.SelectedRect.X / sourceImageWidthScale, this.selectedRegion.SelectedRect.Y / sourceImageHeightScale),
                   previewImageSize,
                   previewImageScale);
        }

        /// <summary>
        /// The user manipulates the selectRegion. The manipulation includes
        /// 1. Translate
        /// 2. Scale
        /// </summary>
        private void SelectRegion_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            this.selectedRegion.UpdateSelectedRect(e.Delta.Scale, e.Delta.Translation.X, e.Delta.Translation.Y);
            e.Handled = true;
        }

        /// <summary>
        /// The manipulation is completed, and then update the preview image
        /// </summary>
        private void SelectRegion_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            this.UpdatePreviewImage();
        }

        /// <summary>
        /// This event will be fired when
        /// 1. An new image is opened.
        /// 2. The source of the sourceImage is set to null.
        /// 3. The view state of this application is changed.
        /// </summary>
        private void SourceImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.IsEmpty || double.IsNaN(e.NewSize.Height) || e.NewSize.Height <= 0)
            {
                this.imageCanvas.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                this.selectedRegion.OuterRect = Rect.Empty;
                this.selectedRegion.ResetCorner(0, 0, 0, 0);
            }
            else
            {
                this.imageCanvas.Visibility = Windows.UI.Xaml.Visibility.Visible;

                this.imageCanvas.Height = e.NewSize.Height;
                this.imageCanvas.Width = e.NewSize.Width;
                this.selectedRegion.OuterRect = new Rect(0, 0, e.NewSize.Width, e.NewSize.Height);

                // Always Reset Selected Region
                var width = Math.Min(e.NewSize.Width, e.NewSize.Height);
                this.selectedRegion.ResetCorner(0, 0, width, width);

                this.UpdatePreviewImage();
            }
        }
    }
}
