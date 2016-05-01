using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Windows.ApplicationModel.Activation;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Caliburn.Micro;
using ZhihuDaily.Domain.Core;
using ZhihuDaily.Domain.Service;
using ZhihuDaily.Util;
using ZhihuDaily.Utils;
using ZhihuDaily.Views;
using Shagu.UI.Utils;
using Shagu.Utils;
using Shagu.Utils.Controls;
using Shagu.Utils.Extends;
using Shagu.Utils.Utils;
using SQLite.Net.Interop;
using SQLite.Net.Platform.WinRT;
using ZhihuDaily.Controls;

namespace ZhihuDaily
{
    sealed partial class App 
    {
        public App()
        {
            InitializeComponent();
            UmengAppKey = Constants.UmengAppKey;
            

            var trans = new TransitionCollection
            {
                new NavigationThemeTransition
                {
                    DefaultNavigationTransitionInfo = new DrillInNavigationTransitionInfo()
                },
            };
        }

        protected override void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
#if DEBUG
            IoC.Get<GlobalInfoManager>().ErrorInfo = string.Format("exception={0}, meesage={1}", e, e.Message);
            base.OnUnhandledException(sender, e);
#endif
        }


        protected override void Configure()
        {
            base.Configure();


            #region 注册ViewModels（底层文件夹为ViewModels,以ViewModel结尾）

            var types = GetType().GetTypeInfo().Assembly.DefinedTypes;

            //改为贪婪匹配
            var viewModels = types.Where(t =>
                Regex.IsMatch(t.FullName, @"ViewModels.[^\s]+?ViewModel$"))
                .ToList();

            foreach (var viewModel in viewModels)
            {
                var type = viewModel.AsType();
                Container.RegisterPerRequest(type, null, type);
            }

            #endregion

            if (DeviceInfoHelper.DeviceType == DeviceType.Mobile)
            {
                Container.RegisterSingleton(typeof (IFrameManager), null, typeof (MobileFrameManager));
            }
            else
            {
                Container.RegisterSingleton(typeof (IFrameManager), null, typeof (DesktopFrameManager));
            }

            Container.RegisterSingleton(typeof (ISQLitePlatform), null, typeof (SQLitePlatformWinRT));
            Container.RegisterSingleton(typeof (CollectionService), null, typeof (CollectionService));
            DataService.DeviceId = Md5Helper.ComputeHash(DeviceInfoHelper.UniqueId);

            //注册资源
            var globalInfoManager = Resources["GlobalInfoManager"];
            Container.RegisterInstance(typeof (GlobalInfoManager), null, globalInfoManager);



            if (DeviceInfoHelper.DeviceType == DeviceType.Mobile)
            {
                var statusBar = StatusBar.GetForCurrentView();
                statusBar.ForegroundColor = Colors.White;
                statusBar.BackgroundOpacity = 0;

                //                var applicationView = ApplicationView.GetForCurrentView();
                //                //设置状态栏透明
                //                applicationView.SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);
            }
        }

        //初始化静态变量
        protected override void InitStaticValue()
        {
            var globalInfoManager = IoC.Get<GlobalInfoManager>();
            LoadThemeResource(globalInfoManager.Theme);
            globalInfoManager.ThemeChanged += GlobalInfoManager_ThemeChanged;
        }

        private void GlobalInfoManager_ThemeChanged(object sender, ElementTheme e)
        {
            LoadThemeResource(e);
        }

        private void LoadThemeResource(ElementTheme e)
        {
            var mainAccessBrush = (SolidColorBrush)
                    ((ResourceDictionary)Resources.ThemeDictionaries[e.ToString()])["MainAccessBrush"];
            RightToastView.InfoBackground = mainAccessBrush;


            if (DeviceInfoHelper.DeviceType == DeviceType.Desktop)
            {
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;

                titleBar.BackgroundColor = titleBar.ButtonBackgroundColor = mainAccessBrush.Color;
                titleBar.ButtonForegroundColor = titleBar.ForegroundColor = Colors.White;

            }
            else
            {
               
                var mainFrame = IoC.Get<IFrameManager>().MainFrame;
                if (mainFrame != null)
                {
                    mainFrame.Background = mainAccessBrush;
                }
            }
        }

        protected override Frame CreateApplicationFrame()
        {
            var globalInfoManager = IoC.Get<GlobalInfoManager>();
            LoadThemeResource(globalInfoManager.Theme);

            var mainAccessBrush = (SolidColorBrush)
                  ((ResourceDictionary)Resources.ThemeDictionaries[globalInfoManager.Theme.ToString()])["MainAccessBrush"];
            RightToastView.InfoBackground = mainAccessBrush;


            var frame = new CacheFrame
            {
                CachedAll = false,
                CacheSize = 4,
//                ContentTransitions = new TransitionCollection { new NavigationThemeTransition() },
                Background = mainAccessBrush
            };

            IoC.Get<IFrameManager>().MainFrame = frame;
            return frame;
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);

            if (Window.Current.Content == null)
            {
                var globalInfoManager = (GlobalInfoManager) Resources["GlobalInfoManager"];

                if (globalInfoManager.IsStartImageReady)
                {
                    var extendedSplash = new ExtendedSplash(args.SplashScreen, globalInfoManager);
                    Window.Current.Content = extendedSplash;
                    Window.Current.Activate();
                    await extendedSplash.DoSomeThing();
                }
                DisplayRootView<MainShellView>();

                //使用次数+1
                IoC.Get<GlobalInfoManager>().UsedTime++;
            }

            Window.Current.Activate();

            


        }
    }
}
