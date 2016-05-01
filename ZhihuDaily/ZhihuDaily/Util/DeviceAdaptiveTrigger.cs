using Windows.ApplicationModel;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Caliburn.Micro;
using Shagu.UI.Utils;
using Shagu.Utils;
using ZhihuDaily.Views;

namespace ZhihuDaily.Util
{
    public enum AdaptiveType
    {
        Show2,
        Show12,
        Show123,
        Show3,
    }


    public class DeviceAdaptiveTrigger : StateTriggerBase
    {
        private const double minwidth = 800;
        private const double maxwidth = 1000;



        private readonly IFrameManager _frameManager;
        private readonly GlobalInfoManager _globalInfoManager;
        private bool rightFrameHasContent;

        public DeviceAdaptiveTrigger()
        {
            if (!DesignMode.DesignModeEnabled)
            {
                _frameManager = IoC.Get<IFrameManager>();
                _globalInfoManager = IoC.Get<GlobalInfoManager>();
                Window.Current.SizeChanged += Current_SizeChanged;
                _frameManager.RightFrameContentChange += _frameManager_RightFrameContentChange;
            }
            
        }

        private void _frameManager_RightFrameContentChange(object sender, bool e)
        {
            rightFrameHasContent = e;
            var adaptiveType = AdaptiveDevice(Window.Current.Bounds.Width);
            SetActive(adaptiveType == AdaptiveType);
        }


        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            var adaptiveType = AdaptiveDevice(e.Size.Width);
            SetActive(adaptiveType == AdaptiveType);
        }
        
        private AdaptiveType AdaptiveDevice(double width)
        {
            if (_globalInfoManager.ArticleFullScreen && _frameManager.RightFrame?.Content != null &&
                   _frameManager.RightFrame.Content.GetType() == typeof(ArticleDetailView))
            {
                return AdaptiveType.Show3;
            }
            else
            {
                if (width > 0 && width <= minwidth)
                {
                    if (rightFrameHasContent)
                    {
                        return AdaptiveType.Show3;
                    }
                    else
                    {
                        return AdaptiveType.Show2;
                    }
                }
                else if (width > minwidth && width <= maxwidth || _globalInfoManager.KeepTwoPanel)
                {
                    if (rightFrameHasContent)
                    {
                        return AdaptiveType.Show3;
                    }
                    else
                    {
                        return AdaptiveType.Show12;
                    }
                }
                else
                {
                    return AdaptiveType.Show123;
                }
            }
        }


        private AdaptiveType _adaptiveType;

        public AdaptiveType AdaptiveType
        {
            get { return _adaptiveType; }
            set
            {
                _adaptiveType = value;
                var device = AdaptiveDevice(Window.Current.Bounds.Width);
                SetActive(device == _adaptiveType);
            }
        }



        public static readonly DependencyProperty MainGridProperty = DependencyProperty.Register(
            "MainGrid", typeof (Grid), typeof (DeviceAdaptiveTrigger), new PropertyMetadata(default(Grid)));

        public Grid MainGrid
        {
            get { return (Grid) GetValue(MainGridProperty); }
            set { SetValue(MainGridProperty, value); }
        }

        public static readonly DependencyProperty LeftPanelProperty = DependencyProperty.Register(
            "LeftPanel", typeof (Grid), typeof (DeviceAdaptiveTrigger), new PropertyMetadata(default(Grid)));

        public Grid LeftPanel
        {
            get { return (Grid) GetValue(LeftPanelProperty); }
            set { SetValue(LeftPanelProperty, value); }
        }

        public static readonly DependencyProperty RightPanelProperty = DependencyProperty.Register(
            "RightPanel", typeof (Grid), typeof (DeviceAdaptiveTrigger), new PropertyMetadata(default(Grid)));

        public Grid RightPanel
        {
            get { return (Grid) GetValue(RightPanelProperty); }
            set { SetValue(RightPanelProperty, value); }
        }
    }
}
