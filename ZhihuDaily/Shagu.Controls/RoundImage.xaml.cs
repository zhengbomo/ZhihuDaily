using System;
using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Brain.Animate;
using Shagu.Utils.Utils;

namespace Shagu.Controls
{
    public sealed partial class RoundImage 
    {
        public RoundImage()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source", typeof(string), typeof(RoundImage), new PropertyMetadata(default(string), SourceChangedCallback));

        private static async void SourceChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var view = (RoundImage)dependencyObject;
            var uri = await ImageUtil.LoadImage(view.Source, view.LoadNew);
            if (uri != null)
            {
                view.path.Fill = new ImageBrush {ImageSource = new BitmapImage(uri)};
                await view.path.AnimateAsync(new FadeInAnimation());
            }
            else
            {
                view.path.Fill = new ImageBrush {ImageSource = new BitmapImage(view.DefaultImageUri)};
            }
        }

        public bool LoadNew { get; set; }

        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            "StrokeThickness", typeof(double), typeof(RoundImage), new PropertyMetadata(default(double)));

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(
            "Stretch", typeof (Stretch), typeof (RoundImage), new PropertyMetadata(default(Stretch)));

        public Stretch Stretch
        {
            get { return (Stretch) GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
            "Stroke", typeof (Brush), typeof (RoundImage), new PropertyMetadata(default(Brush)));

        public Brush Stroke
        {
            get { return (Brush) GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public static readonly DependencyProperty DefaultImageUriProperty = DependencyProperty.Register(
            "DefaultImageUri", typeof (Uri), typeof (RoundImage), new PropertyMetadata(default(Uri), DefaultImageUriChangedCallback));

        private static void DefaultImageUriChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var roundImage = (RoundImage) dependencyObject;
            var imageBrush = new ImageBrush { ImageSource = new BitmapImage(roundImage.DefaultImageUri) };
            roundImage.path.Fill = imageBrush;
        }

        public Uri DefaultImageUri
        {
            get { return (Uri) GetValue(DefaultImageUriProperty); }
            set { SetValue(DefaultImageUriProperty, value); }
        }
        
    }
}
