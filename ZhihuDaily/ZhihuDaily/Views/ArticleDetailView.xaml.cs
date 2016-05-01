using Windows.Foundation;
using Windows.UI.Xaml;

namespace ZhihuDaily.Views
{
    public partial class ArticleDetailView
    {
        private FrameworkElement headerImage;
        public ArticleDetailView()
        {
            InitializeComponent();
            SizeChanged += ArticleDetailView_SizeChanged;
        }

        private void ArticleDetailView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            adapteHeaderImage(e.NewSize.Width);
        }

        private void HeaderImage_OnLoaded(object sender, RoutedEventArgs e)
        {
            headerImage = sender as FrameworkElement;
            adapteHeaderImage(ActualWidth);

        }

        private void adapteHeaderImage(double size)
        {
            if (headerImage != null)
            {
                var rate = size / 645;
                headerImage.Width = 645 * rate;
                headerImage.Height = 445 * rate;
            }
        }

        private void GoToTop_OnClick(object sender, RoutedEventArgs e)
        {
            articleView.GoToTop();
           
        }
    }
}
