using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Caliburn.Micro;
using Microsoft.AdMediator.Universal;
using ZhihuDaily.Domain.Core;
using ZhihuDaily.Util;

namespace ZhihuDaily.Controls
{
    public sealed partial class AdControl
    {
//        private Microsoft.Advertising.WinRT.UI.AdControl _msadControl;

            

        public AdControl()
        {
            InitializeComponent();

            Visibility = Visibility.Collapsed;
            return;

//            Loaded += AdControl_Loaded;
//            SizeChanged += AdControl_SizeChanged;

//            MsAdId = "252633";

#if DEBUG
//            Visibility = Visibility.Collapsed;
#endif
        }

        public string MsAdId { get; set; }

        private void AdControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
//            if (_msadControl != null)
//            {
//                _msadControl.Width = e.NewSize.Width;
//                _msadControl.Height = e.NewSize.Width*80/480;
//            }

            layoutRoot.Width = e.NewSize.Width;
            layoutRoot.Height = e.NewSize.Width*80/480;
        }

        private void AdControl_Loaded(object sender, RoutedEventArgs e)
        {
            var globalInfoManager = IoC.Get<GlobalInfoManager>();
            if (globalInfoManager.AdCreateTime % 2 >= 0)
            {
                Debug.WriteLine("create ms ad");

//                if (_msadControl != null)
//                {
//                    layoutRoot.Children.Remove(_msadControl);
//                }
//                CreateMsAdControl();
            }

            globalInfoManager.AdCreateTime++;
        }

        #region MS_Ad

        private void CreateMsAdControl()
        {
//            var adControl = new Microsoft.Advertising.WinRT.UI.AdControl
//            {
//                HorizontalAlignment = HorizontalAlignment.Stretch,
//                AdUnitId = MsAdId,
//                IsAutoRefreshEnabled = true,
//                AutoRefreshIntervalInSeconds = 30,
//                ApplicationId = Constants.AdApplicationId,
//                Background = new SolidColorBrush(Colors.Transparent),
//                Width = ActualWidth,
//                Height = ActualWidth * 80 / 480,
//            };
//
//            adControl.ApplicationId = Constants.AdApplicationId;
//
//            adControl.AdRefreshed += MsAdControl_AdRefreshed;
//            adControl.OnPointerDown += MsAdControl_OnPointerDown;
//            adControl.ErrorOccurred += MsAdControl_OnErrorOccurred;
////            _msadControl = adControl;
//            layoutRoot.Children.Add(adControl);
        }

//        private async void MsAdControl_OnPointerDown(object sender, PointerDownEventArgs e)
//        {
//            await UmengSDK.UmengAnalytics.TrackEvent("ad_pointer_down");
//        }
//
//        private async void MsAdControl_AdRefreshed(object sender, RoutedEventArgs e)
//        {
//            await UmengSDK.UmengAnalytics.TrackEvent("ad_refresh");
//        }
//
//        private async void MsAdControl_OnErrorOccurred(object sender, AdErrorEventArgs e)
//        {
////            layoutRoot.Children.Remove(_msadControl);
////            _msadControl = null;
//            await Task.Delay(10000);
//
//            CreateMsAdControl();
//        }

        #endregion
    }
}
