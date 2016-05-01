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
    public class DesktopFrameManager : FrameManagerBase
    {
        private readonly GlobalInfoManager _globalInfoManager;

        private Frame _rightFrame;

        public DesktopFrameManager(WinRTContainer container, GlobalInfoManager globalInfoManager)
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
                }
            }

        }

        public override Frame RightFrame
        {
            get { return _rightFrame; }
            set
            {
                RightNavService = new FrameAdapter(value);
                Container.RegisterInstance(typeof(INavigationService), "rightFrame", RightNavService);
                _rightFrame = value;
            }
        }

        public override INavigationService RightNavService { get; protected set; }
    }
}
