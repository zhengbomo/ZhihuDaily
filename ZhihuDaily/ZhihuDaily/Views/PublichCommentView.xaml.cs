using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Caliburn.Micro;
using Shagu.UI.Utils;
using Shagu.Utils.Utils;

namespace ZhihuDaily.Views
{
    public sealed partial class PublichCommentView
    {
        private readonly IFrameManager frameManager;
        public PublichCommentView()
        {
            InitializeComponent();
            frameManager = IoC.Get<IFrameManager>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            frameManager.BackKeyPressing += FrameManager_BackKeyPressing;
        }

        private async void FrameManager_BackKeyPressing(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        {
            if (contentTextBox.Text.Length > 0)
            {
                e.Handled = true;
                if (ContentDialogResult.Primary == await MessageBox.ShowAsync("是否放弃当前输入的内容 : (", "提示", "确定", "继续编辑"))
                {
                    frameManager.OnBackKeyPressed();
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            frameManager.BackKeyPressing -= FrameManager_BackKeyPressing;
        }
    }
}
