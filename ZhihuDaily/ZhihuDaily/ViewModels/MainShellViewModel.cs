using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using Windows.Security.Authentication.Web;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Caliburn.Micro;
using Shagu.UI.Utils;
using Shagu.Utils;
using Shagu.Utils.Controls;
using Shagu.Utils.Utils;
using Shagu.Weibo.Social;
using ZhihuDaily.Domain.Core;
using ZhihuDaily.Domain.Models;
using ZhihuDaily.Domain.Service;
using ZhihuDaily.Util;
using ZhihuDaily.Utils;

namespace ZhihuDaily.ViewModels
{
    public class MainShellViewModel : Screen
    {
        private readonly IFrameManager _frameManager;
        private readonly GlobalInfoManager _globalInfoManager;

        private readonly CancellationToken _cancelToken;



        public ObservableCollection<CategoryInfo> MainTitles { get; set; }

        public int SelectedIndex { get; set; }

        private static readonly List<CategoryInfo> DefaultCategory = new List<CategoryInfo>
        {
            new CategoryInfo {Name = "首页", Id = 0},
//            new CategoryInfo {Name = "收藏", Id = 0},
//            new CategoryInfo {Name = "设置", Id = 0},
//            new CategoryInfo {Name = "test", Id = 0},
        };

        public MainShellViewModel(IFrameManager frameManager, GlobalInfoManager globalInfoManager)
        {
            _frameManager = frameManager;
            _globalInfoManager = globalInfoManager;

            _cancelToken = new CancellationToken();

            SelectedIndex = -1;
            MainTitles = new BindableCollection<CategoryInfo>(DefaultCategory);
            SelectedIndex = 0;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            
#if DEBUG
            var errorInfo = _globalInfoManager.ErrorInfo;
            if (errorInfo != null)
            {
                await MessageBox.ShowAsync(errorInfo);
                _globalInfoManager.ErrorInfo = null;
            }
#endif


            //如果使用3次，提示好评
            if (_globalInfoManager.ShouldRate)
            {
                if (ContentDialogResult.Primary ==
                    await MessageBox.ShowAsync("开发UWP不易，给开发者好评鼓励一下呗 ^o^)", "提示", "走起", "以后再说"))
                {
                    _globalInfoManager.HasRated = true;
                    await AppInfoHelper.Rate(Constants.ProductId);
                }
            }
            else
            {
                Version ver;
                if (Version.TryParse(_globalInfoManager.LastVersion, out ver))
                {
                    if (ver < new Version(1,1,3,0))
                    {
                        //提示
                        await MessageBox.ShowAsync("更新说明：新版带来微博登录，评论，点赞，优化UI显示，新版本采用联网收藏，需要登陆后才能使用，旧版本的收藏可以在“本地收藏”找到");
                        _globalInfoManager.LastVersion = AppInfoHelper.Version;
                    }
                }
                else
                {
                    //新包，写入新版本
                    _globalInfoManager.LastVersion = AppInfoHelper.Version;
                }

            }

            //加载分类
            LoadCategory();

            LoadStartImage();
        }

        private async void LoadStartImage()
        {
            //加载欢迎页
            await Task.Delay(2000, _cancelToken);
            var result = await DataService.GetStartImage();

            RequestUtil.DoResult(result, async res =>
            {
                if (_globalInfoManager.StartImage == null || !_globalInfoManager.StartImage.Equals(res.Img))
                {
                    var data = await RequestUtilBase.CacheFile(res.Img, Constants.StartImageSourcePath, true);
                    if (data != null)
                    {
                        var file = await
                            ApplicationData.Current.LocalFolder.CreateFileAsync(Constants.StartImageBluePath,
                                CreationCollisionOption.ReplaceExisting);

                        await BlurUtil.Blur(data, file);

                        _globalInfoManager.StartImage = res.Img;
                        _globalInfoManager.StartText = res.Text;
                        _globalInfoManager.IsStartImageReady = true;
                    }
                }
            });
        }
        private async void LoadCategory()
        {
            //读缓存
            var categorys = await CacheManager.GetCategory();
            if (categorys != null)
            {
                foreach (var categoryInfo in categorys.Categorys)
                {
                    MainTitles.Add(categoryInfo);
                }
            }

            //请求
            var result = await DataService.GetCategory();
            RequestUtil.DoResult(result, async res =>
            {
                await CacheManager.SetCategory(res);

                var prevIndex = SelectedIndex;
                SelectedIndex = -1;
                NotifyOfPropertyChange(nameof(SelectedIndex));
                MainTitles.Clear();

                foreach (var categoryInfo in DefaultCategory)
                {
                    MainTitles.Add(categoryInfo);
                }
                foreach (var categoryInfo in res.Categorys)
                {
                    MainTitles.Add(categoryInfo);
                }

                SelectedIndex = prevIndex;
                NotifyOfPropertyChange(nameof(SelectedIndex));
            });
        }


        public void SetupCenterFrame(Frame frame)
        {
            if (_frameManager.CenterFrame == null)
            {
                _frameManager.CenterFrame = frame;
                _frameManager.CenterNavService.For<MainHomeViewModel>().Navigate();
            }
        }

        public void SetupRightFrame(Frame frame)
        {
            if (_frameManager.RightFrame == null)
            {
                _frameManager.RightFrame = frame;
                _frameManager.RightNavService.For<RightPlaceholdViewModel>().Navigate();
            }
        }

        public void TitleClick(ItemClickEventArgs item)
        {
            var categoryInfo = (CategoryInfo) item.ClickedItem;
            if (categoryInfo.Id == 0)
            {
                switch (categoryInfo.Name)
                {
                    case "首页":
                        _frameManager.CenterNavService.For<MainHomeViewModel>().Navigate();
                        break;
                    case "本地收藏":
                        _frameManager.CenterNavService.For<ArticleCollectionViewModel>().Navigate();
                        break;
                    case "收藏":
                        _frameManager.CenterNavService.For<NewCollectionViewModel>().Navigate();
                        break;
                    case "推荐":
                        _frameManager.CenterNavService.For<RecommendAppViewModel>().Navigate();
                        break;
                    case "设置":
                        _frameManager.CenterNavService.For<MainSettingViewModel>().Navigate();
                        break;
                    case "test":
                        _frameManager.CenterNavService.For<TestViewModel>().Navigate();
                        break;
                }
            }
            else
            {
                //调到分类
                _frameManager.CenterNavService.For<CategoryDetailViewModel>()
                    .WithParam(vm => vm.Title, categoryInfo.Name)
                    .WithParam(vm => vm.CategoryId, categoryInfo.Id)
                    .Navigate();
            }
        }

        public void RightFrameGoBack()
        {
            _frameManager.RightFrameGoback();
        }


        public void Nav2Setting()
        {
            _frameManager.CenterNavService.For<MainSettingViewModel>().Navigate();
            SelectedIndex = -1;
            NotifyOfPropertyChange(nameof(SelectedIndex));
        }

        public void Nav2Favorite()
        {

            _frameManager.CenterNavService.For<NewCollectionViewModel>().Navigate();
            SelectedIndex = -1;
            NotifyOfPropertyChange(nameof(SelectedIndex));
        }

        public async void AvatarClick()
        {
            if (!_globalInfoManager.IsLogin)
            {
                var client = new WeiboClient(Constants.WeiboAppId, Constants.WeiboSecret, Constants.WeiboRedirectUrl);

                var result = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None,
                    new Uri(client.AuthorizeUrl, UriKind.Absolute),
                    new Uri(Constants.WeiboRedirectUrl, UriKind.Absolute));



                switch (result.ResponseStatus)
                {
                    case WebAuthenticationStatus.Success:

                        //ShowLoading
                        var loadingView = LoadingView.ShowLoading("正在登陆");

                        var data = result.ResponseData;
                        //如果授权成功
                        var match = Regex.Match(data, @"(?<=code=)[^&]*");
                        if (match.Success)
                        {
                            var code = match.Value;
                            var tokenResult = await AuthHelper.GetWeiboToken(client, code, _cancelToken);

                            RequestUtil.DoResult(tokenResult, async token =>
                            {
                                if (token != null)
                                {
                                    _globalInfoManager.WeiboToken = token;

                                    var loginResult = await
                                        DataService.Login(token.RefreshToken, token.ExpiresIn, token.AccessToken,
                                            token.Uid, _cancelToken);

                                    RequestUtil.DoResult(loginResult, loging =>
                                    {
                                        _globalInfoManager.LoginResult = loging;
                                        ToastHelper.ToastInfo("登陆成功", _globalInfoManager.IsNightMode);
                                    });
                                }
                            });
                        }
                        else
                        {
                            ToastHelper.ToastError("授权失败");
                        }

                        loadingView.Hide();

                        break;
                    case WebAuthenticationStatus.UserCancel:
                    case WebAuthenticationStatus.ErrorHttp:
                        ToastHelper.ToastError("授权失败");
                        break;

                }
            }
            else
            {
                _frameManager.CenterNavService.For<UserInfoViewModel>().Navigate();
            }
        }
    }
}