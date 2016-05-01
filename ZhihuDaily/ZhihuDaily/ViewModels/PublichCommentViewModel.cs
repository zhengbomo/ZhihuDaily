using Windows.UI.Xaml.Controls;
using Caliburn.Micro;
using Shagu.UI.Utils;
using Shagu.Utils.Controls;
using Shagu.Utils.Utils;
using ZhihuDaily.Domain.Service;
using ZhihuDaily.Util;
using ZhihuDaily.Views;

namespace ZhihuDaily.ViewModels
{
    public class PublichCommentViewModel : Screen
    {
        private readonly GlobalInfoManager _globalInfoManager;
        private readonly IFrameManager _frameManager;

        public int ArticleId { get; set; }
        public string ReplyName { get; set; }
        public int ReplyId { get; set; }

        public string Title => ReplyId == 0 ? "写点评" : string.Format("回复：{0}", ReplyName);

        private TextBox _contentTextBox;
        private CheckBox _shareToWeiboCheckBox;

        public PublichCommentViewModel(GlobalInfoManager globalInfoManager, IFrameManager frameManager)
        {
            _globalInfoManager = globalInfoManager;
            _frameManager = frameManager;
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            var v = (PublichCommentView) view;
            _contentTextBox = v.contentTextBox;
            _shareToWeiboCheckBox = v.shareToWeiboCheckBox;
        }



        public async void Publish()
        {
            var loadingView = LoadingView.ShowLoading("正在保存");

            bool isShareToWeibo = _shareToWeiboCheckBox.IsChecked.HasValue && _shareToWeiboCheckBox.IsChecked.Value;

            var commentResult = await
                DataService.AddComment(_globalInfoManager.AccessToken, ArticleId, ReplyId, isShareToWeibo,
                    _contentTextBox.Text);

            RequestUtil.DoResult(commentResult, result =>
            {
                ToastHelper.ToastInfo("发表成功", _globalInfoManager.IsNightMode);
                _frameManager.RightFrameGoback();
            });

            loadingView.Hide();
        }
    }
}