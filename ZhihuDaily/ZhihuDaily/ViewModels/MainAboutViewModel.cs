using System;
using System.Linq;
using Windows.ApplicationModel.Email;
using Windows.ApplicationModel.Store;
using Caliburn.Micro;
using Shagu.Utils;
using ZhihuDaily.Domain.Core;
using Shagu.Utils.Utils;
using ZhihuDaily.Util;

namespace ZhihuDaily.ViewModels
{
    public class MainAboutViewModel : Screen
    {
        public string Version => AppInfoHelper.Version;

        public string Description
            => @"知乎日报第三方客户端，知乎日报是一款拥有千万用户的资讯类客户端，每日提供来自知乎社区的精选问答，还有国内一流媒体的专栏特稿。

主题日报包括动漫、设计、大公司、游戏、财经、电影、电子音乐、互联网安全等丰富内容，为业内人和资深爱好
者推荐各领域最精彩文章，满足高质量阅读需求。

在知乎日报，告别浮躁，重获阅读的愉悦。

QQ群：40828573
PS：本应用由第三方开发者开发。";

        private readonly GlobalInfoManager _globalInfoManager;
        public MainAboutViewModel(GlobalInfoManager globalInfoManager)
        {
            _globalInfoManager = globalInfoManager;
        }



        public async void Feedback()
        {
            var appName = await AppInfoHelper.GetAppName();

            var emailMessage = new EmailMessage();
            emailMessage.To.Add(new EmailRecipient("zhengbomo@hotmail.com", "bomo"));

            emailMessage.Subject = string.Format("{0} {1} Ver {2}反馈", appName, DeviceInfoHelper.DeviceType, Version);
            emailMessage.Body = "";
            await EmailManager.ShowComposeNewEmailAsync(emailMessage);
        }

        public async void Rate()
        {
            await AppInfoHelper.Rate(Constants.ProductId);
        }

        public async void PayForAd()
        {
            try
            {
                var listings = await CurrentApp.LoadListingInformationByProductIdsAsync(
                                                new[] { Constants.PayForAdId });

                var payforad = listings.ProductListings.FirstOrDefault();

                try
                {
                    if (payforad.Value?.ProductId != null)
                    {
                        var res = await CurrentApp.RequestProductPurchaseAsync(payforad.Value.ProductId);

                        if (CurrentApp.LicenseInformation.ProductLicenses[Constants.PayForAdId].IsActive)
                        {
                            _globalInfoManager.HasPayForAd = true;
                        }

                        ToastHelper.ToastInfo("操作成功", _globalInfoManager.IsNightMode);
                        CurrentApp.ReportProductFulfillment(Constants.PayForAdId);
                    }
                    else
                    {
                        ToastHelper.ToastError("操作失败", _globalInfoManager.IsNightMode);
                    }

                }
                catch (Exception ex)
                {
#if DEBUG
                    ToastHelper.ToastError(ex.Message, _globalInfoManager.IsNightMode);
#else
                    ToastHelper.ToastError("操作失败", _globalInfoManager.IsNightMode);
#endif
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                    ToastHelper.ToastError(ex.Message, _globalInfoManager.IsNightMode);
#else
                ToastHelper.ToastError("操作失败", _globalInfoManager.IsNightMode);
#endif

            }
        }

    }
}
