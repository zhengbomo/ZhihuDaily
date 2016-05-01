using System;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Caliburn.Micro;
using ZhihuDaily.Models;
using ZhihuDaily.Utils;
using ZhihuDaily.ViewModels;
using Shagu.UI.Utils;
using Shagu.Utils.Utils;
using ZhihuDaily.Domain.Models;

namespace ZhihuDaily.Util
{
    public class NavigationManager
    {
        private IFrameManager _frameManager => IoC.Get<IFrameManager>();
        private GlobalInfoManager _globalInfoManager => IoC.Get<GlobalInfoManager>();

        public void Nav2AboutView()
        {
            _frameManager.RightFrameAndNav(navigationService =>
            {
                navigationService.For<MainAboutViewModel>().Navigate();
            });
        }

        public async void Nav2LinkUrl(ArticleParagraph paragraph)
        {
            if (!string.IsNullOrEmpty(paragraph.Value))
            {
                await Launcher.LaunchUriAsync(new Uri(paragraph.Value));
            }
        }

        public void Nav2RecommendAppView()
        {
            _frameManager.RightFrameAndNav(navigationService =>
            {
                navigationService.For<RecommendAppViewModel>().Navigate();
            });
        }

        public void Nav2LoginView()
        {
            _frameManager.CenterNavService.For<LoginViewModel>().Navigate();

        }

        public void Nav2Setting()
        {
            _frameManager.CenterNavService.For<MainSettingViewModel>().Navigate();
        }

        public void Nav2Favorite()
        {
            _frameManager.CenterNavService.For<ArticleCollectionViewModel>().Navigate();
        }

        public void ChangeNightmode()
        {
            _globalInfoManager.IsNightMode = !_globalInfoManager.IsNightMode;
        }

        public void ItemClick(ItemClickEventArgs item)
        {
            var article = (StoryInfo)item.ClickedItem;
            Debug.WriteLine(article.Type);

            switch (article.Type)
            {
                case 0:
                case 2:
                    _frameManager.RightFrameClearAndNav(navigationService =>
                    {
                        navigationService.For<ArticleDetailViewModel>()
                            .WithParam(vm => vm.Image, article.Images?.FirstOrDefault() ?? "")
                            .WithParam(vm => vm.ArticleId, article.Id)
                            .WithParam(vm => vm.Multipic, article.Multipic)
                            .Navigate();
                    });
                    break;
                case 1:
                    _frameManager.RightFrameClearAndNav((navigationService) =>
                    {
                        navigationService.For<WebViewArticleViewModel>()
                            .WithParam(vm => vm.Image, article.Images?.FirstOrDefault()??"")
                            .WithParam(vm => vm.ArticleId, article.Id)
                            .WithParam(vm => vm.Multipic, article.Multipic)
                            .Navigate();
                    });
                    break;

            }
            

        }

        public void CopyText(string type)
        {
            switch (type)
            {
                case "alipay":
                    copyText("449179249@163.com", "支付宝账号复制成功");
                    break;
                case "qun":
                    copyText("40828573", "QQ群号复制成功");
                    break;
            }
        }

        private void copyText(string text, string info)
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.SetText(text);
            Clipboard.SetContent(dataPackage);

            var globalInfoManager = IoC.Get<GlobalInfoManager>();
            ToastHelper.ToastInfo(info, globalInfoManager.IsNightMode);
        }
    }
}
