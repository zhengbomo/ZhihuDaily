using Windows.UI.Xaml;

namespace ZhihuDaily.Views
{
    public sealed partial class CommentListView 
    {
        public CommentListView()
        {
            InitializeComponent();
        }

        private void GoToTop_OnClick(object sender, RoutedEventArgs e)
        {
            articleView.GoToTop();
        }
    }
}
