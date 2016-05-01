using System;
using Windows.UI.Xaml;

namespace ZhihuDaily.Controls
{
    public sealed partial class LoadMoreControl 
    {
        public event EventHandler<object> LoadMore;

        public LoadMoreControl()
        {
            InitializeComponent();
        }


        private void OnLoadMore()
        {
            var handler = LoadMore;
            handler?.Invoke(this, EventArgs.Empty);
        }

        private void LoadMoreButton_OnClick(object sender, RoutedEventArgs e)
        {
            OnLoadMore();
        }
    }
}
