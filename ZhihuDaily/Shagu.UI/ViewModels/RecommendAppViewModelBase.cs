/* ==============================================================================
 * 功能描述：RecommendAppViewModelBase  
 * 创 建 者：贤凯
 * 创建日期：2/9/2015 10:45:06 AM
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Caliburn.Micro;
using Shagu.UI.Models;
using Shagu.Utils;
using UmengSDK;

namespace Shagu.UI.ViewModels
{
    public class RecommendAppViewModelBase : Screen
    {
        public ObservableCollection<RecommendAppInfo> Items { get; set; }

        protected string ProductId { get; set; }

        public RecommendAppViewModelBase(string productId)
        {
            ProductId = productId;
            Items = new ObservableCollection<RecommendAppInfo>();
            if (DesignMode.DesignModeEnabled)
            {
                LoadItems();
            }
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            LoadItems();
        }

        private void LoadItems()
        {
            var apps = new List<RecommendAppInfo>
            {
                new RecommendAppInfo
                {
                    Descriptio =
                        "深陷浮躁的世界与喧闹的社交环境，你有多久没有停下驻足，观照内心？ 片刻是会讲故事的APP：这里有触动你的故事， 感动你的声音，适合你的写作方式，互通内心世界的朋友。慢下脚步，安静片刻，你会感受到，每一刻都有新故事正在发声。",
                    ProductId = "9NBLGGH5Z7WN",
                    Icon =
                        "https://store-images.s-microsoft.com/image/apps.2056.13510798886413317.b476a8c7-b9de-4cee-b9b9-6e8b4a527cf3.47ff6e7d-2cc1-489f-b92c-00d9a26fdb75?w=191&h=191",
                    Name = "片刻",
                    SupportType = SupportType.Both,
                    
                },
                new RecommendAppInfo
                {
                    Descriptio =
                        "知乎日报第三方客户端，知乎日报是一款拥有千万用户的资讯类客户端，每日提供来自知乎社区的精选问答，还有国内一流媒体的专栏特稿。主题日报包括动漫、设计、大公司、游戏、财经、电影、电子音乐、互联网安全等丰富内容，为业内人和资深爱好者推荐各领域最精彩文章，满足高质量阅读需求。在知乎日报，告别浮躁，重获阅读的愉悦。",
                    ProductId = "9NBLGGH6C72W",
                    SupportType = SupportType.Both,
                    Icon =
                        "https://store-images.s-microsoft.com/image/apps.42399.13510798886614396.19d73a8d-1353-4b9b-bff2-aa7c88f0f140.6ff9c5a6-1f12-483d-896a-91f0b5dd3bea?w=191&h=191",
                    Name = "知乎日报Win10"
                },
                 new RecommendAppInfo
                {
                    Descriptio =
                        "礼物说官方授权的Windows Phone客户端！ 精心于礼物攻略，再也不用烦恼圣诞节、情人节应该买什么礼物送给他/她了，快点来给你的亲人朋友挑选礼物吧~~~ 礼物说专注精品，相信WP版本也不会令你失望~~ ",
                    ProductId = "9nblggh5x991",
                    Icon =
                        "https://store-images.s-microsoft.com/image/apps.63579.13510798886394276.89c3110b-f4ce-43b5-80be-5297afa663e5.6bbfbc07-faa9-4a83-905f-be5776cb2996?w=191&h=191",
                    Name = "礼物说",
                    SupportType = SupportType.Both,

                },
                new RecommendAppInfo
                {
                    Descriptio = "知性是中文世界最大的情趣社区，截至今日，共有2,648,574位性情男女在此学习性技巧，提升性能力。在知性，你可以解决一切两性问题，不再青涩，不再恐惧，让你安全地拥抱“生命的大和谐”",
                    ProductId = "9NBLGGH6C453",
                    SupportType = SupportType.Both,
                    Icon =
                        "https://store-images.s-microsoft.com/image/apps.24448.13510798886600886.cba84053-8560-4b39-a826-67b9a793bf59.36842e4d-7ddb-4b17-8953-8477c6ca002e?w=191&h=191",
                    Name = "知性"
                },

                new RecommendAppInfo
                {
                    Descriptio =
                        "果壳精选用科技视角，看不一样的世界。 果壳网(Guokr.com)是开放、多元的泛科技主题网站，从食品安全到空气污染，从拖延症到健康知识，解读生活热点，粉碎网络谣言，用靠谱知识让生活更有品质。",
                    ProductId = "9NBLGGH6BPHL",
                    SupportType = SupportType.Both,
                    Icon =
                        "https://store-images.s-microsoft.com/image/apps.44166.13510798886599743.7e0aa584-8dbe-47c9-8de2-db081dada087.5ce71b25-28bb-4c02-b2da-57323fc2ee6b?w=191&h=191",
                    Name = "果壳精选"
                },
                new RecommendAppInfo
                {
                    Descriptio =
                        "一个历史知识的百科大全。趣历史专注于搜集整理中国各朝代人物、战争、野史、文化等全方面历史知识，以及环球各个国家历史文化,为您提供全面的历史知识阅读学习平台",
                    ProductId = "9NBLGGH6B32F",
                    SupportType = SupportType.Both,
                    Icon =
                        "https://store-images.s-microsoft.com/image/apps.40086.13510798886583778.afade396-6c4c-458e-89b4-608a9f80e06f.27845805-63a7-4cce-b137-f9565fe4ae98?w=191&h=191",
                    Name = "趣历史"
                },
            
                new RecommendAppInfo
                {
                    Descriptio = "个性化订阅，智能推荐，为你量身定制的军迷爱好者平台。实时资讯：随时随地发现新内容，资深军迷的聚集地。",
                    SupportType = SupportType.Both,
                    ProductId = "9NBLGGH6C5QG",
                    Icon =
                        "https://store-images.s-microsoft.com/image/apps.39677.13510798886604345.597aefba-5a61-452f-84b3-957e1d9d892b.bee30f19-0e30-4b99-a978-60efddc5cedb?w=191&h=191",
                    Name = "军事头条"
                }
            };

            if (DeviceInfoHelper.DeviceType == DeviceType.Mobile)
            {
                apps.Add(new RecommendAppInfo
                {
                    Descriptio = "一个万能的手机伴侣，拥有海量的精品应用，是中国最大的微软粉丝社区。",
                    SupportType = SupportType.Mobile,
                    ProductId = "9d22d8a7-0910-4cfb-b5e2-c2ea63ca63bc",
                    Icon = "http://xap.coolxap.com/d/file/app/tools/2015-05-18/ae17dcc011fbbfe7f1e6042f0be0a1a6.png",
                    Name = "酷七手机助手"
                });
            }


            foreach (var recommendAppInfo in
                apps.Where(recommendAppInfo => !ProductId.Equals(recommendAppInfo.ProductId)))
            {
                Items.Add(recommendAppInfo);
            }
        }
        
        public async void ItemClick(ItemClickEventArgs item)
        {
            var recommendApp = (RecommendAppInfo) item.ClickedItem;
            await UmengAnalytics.TrackEvent("app_recommend_tapped");

            switch (recommendApp.SupportType)
            {
                case SupportType.Mobile:
                    // 打开指定 app 在商店中的详细页
                    await Launcher.LaunchUriAsync(new Uri(string.Format("zune:navigate?appid={0}", recommendApp.ProductId)));
                    break;
                case SupportType.Desktop:
                case SupportType.Both:
                    // 打开指定 app 在商店中的详细页
                    await Launcher.LaunchUriAsync(
                        new Uri(string.Format("ms-windows-store://pdp/?ProductId={0}", recommendApp.ProductId)));

                    break;
            }

        }
    }
}
