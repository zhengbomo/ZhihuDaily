using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ZhihuDaily.Views
{
    public sealed partial class WebViewArticleView 
    {
        public WebViewArticleView()
        {
            this.InitializeComponent();
        }

        private void WebView_OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            progressRing.Visibility = Visibility.Collapsed;
        }

        private void WebView_OnNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            progressRing.Visibility = Visibility.Visible;
        }
    }
}
