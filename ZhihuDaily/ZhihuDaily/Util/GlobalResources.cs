using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ZhihuDaily.Domain.Core;
using Shagu.Utils;
using Shagu.Utils.Models;

namespace ZhihuDaily.Util
{
    public class GlobalResources
    {
        public List<HeaderInfo> MainTitles => new List<HeaderInfo>
        {
            new HeaderInfo {SymbolIcon = Symbol.List, Title = "首页"},
            new HeaderInfo {SymbolIcon = Symbol.Emoji2, Title = "本地收藏"},
            new HeaderInfo {SymbolIcon = Symbol.Emoji2, Title = "收藏"},
            new HeaderInfo {SymbolIcon = Symbol.Link, Title = "推荐"},
            new HeaderInfo {SymbolIcon = Symbol.Setting, Title = "设置"},
#if DEBUG
//            new HeaderInfo {SymbolIcon = Symbol.Manage, Title = "test"},
#endif
        };

        public string AppName => Constants.AppName;

        public GridLength StatusBarHeight =>
            DeviceInfoHelper.DeviceType == DeviceType.Mobile
                ? new GridLength(24)
                : new GridLength(0);


        public List<HeaderInfo<Thickness, CornerRadius>> CommunityCategorys
          => new List<HeaderInfo<Thickness, CornerRadius>>
          {
                new HeaderInfo<Thickness, CornerRadius>
                {
                    Title = "板块",
                    Value1 = new Thickness(1, 1, 0.5, 1),
                    Value2 = new CornerRadius(6, 0, 0, 6)
                },
                new HeaderInfo<Thickness, CornerRadius>
                {
                    Title = "专题",
                    Value1 = new Thickness(0.5, 1, 1, 1),
                    Value2 = new CornerRadius(0, 6, 6, 0)
                },
          };

        public List<HeaderInfo<Thickness, CornerRadius>> SubjectCategorys
       => new List<HeaderInfo<Thickness, CornerRadius>>
       {
                new HeaderInfo<Thickness, CornerRadius>
                {
                    Title = "全部",
                    Value1 = new Thickness(1, 1, 0.5, 1),
                    Value2 = new CornerRadius(6, 0, 0, 6)
                },
                new HeaderInfo<Thickness, CornerRadius>
                {
                    Title = "精华",
                    Value1 = new Thickness(0.5, 1, 1, 1),
                    Value2 = new CornerRadius(0, 6, 6, 0)
                },
       };
    }
}
