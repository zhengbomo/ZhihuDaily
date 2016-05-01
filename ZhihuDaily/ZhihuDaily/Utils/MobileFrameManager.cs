using System;
using System.Linq;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Caliburn.Micro;
using ZhihuDaily.Util;
using Shagu.UI.Utils;
using Shagu.Utils;
using Shagu.Utils.Utils;

namespace ZhihuDaily.Utils
{
    public class MobileFrameManager : FrameManagerBase
    {
        private readonly GlobalInfoManager _globalInfoManager;

        public MobileFrameManager(WinRTContainer container, GlobalInfoManager globalInfoManager)
            :base(container)
        {
            _globalInfoManager = globalInfoManager;
            SystemNavigationManager.GetForCurrentView().BackRequested += FrameManager_BackRequested;
        }

        private bool _readyToExit;

        private void FrameManager_BackRequested(object sender, BackRequestedEventArgs e)
        {
            OnBackKeyPressing(e);
            if (!e.Handled)
            {
                OnBackKeyPressed();
            }
            e.Handled = true;
        }

        public override void OnBackKeyPressed()
        {
            if (!RightFrameGoback())
            {
                if (DeviceInfoHelper.DeviceType == DeviceType.Mobile)
                {
                    if (CenterNavService.CanGoBack && CenterNavService.BackStack.Count > 0 &&
                        CenterNavService.BackStack.First().SourcePageType != CenterNavService.CurrentSourcePageType)
                    {
                        while (CenterNavService.BackStack.Count > 1)
                        {
                            CenterNavService.BackStack.RemoveAt(1);
                        }
                        CenterNavService.GoBack();
                        OnBack2MainView(null);
                    }
                    else
                    {
                        if (_readyToExit)
                        {
                            Application.Current.Exit();
                        }
                        else
                        {
                            ToastHelper.ToastInfo("再按一次返回退出App", _globalInfoManager.IsNightMode, () =>
                            {
                                _readyToExit = false;
                            });
                            _readyToExit = true;
                        }
                    }
                }
            }
        }

        public override Frame RightFrame { get; set; }

        public override INavigationService RightNavService
        {
            get { return MainNavService; }
            protected set { throw new NotImplementedException("手机没有RightFrame"); }
        }
    }
}
