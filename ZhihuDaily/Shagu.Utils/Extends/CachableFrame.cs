using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Shagu.Utils.Extends
{
    public class CacheFrame : Frame
    {
        public CacheFrame()
        {
            Navigated += CacheFrame_Navigated;
            Navigating += CacheFrame_Navigating;

            ContentTransitions = new TransitionCollection
            {
                new NavigationThemeTransition
                {
                    DefaultNavigationTransitionInfo = new DrillInNavigationTransitionInfo()
                },
            };
        }

        public bool CachedAll { get; set; }

        private void CacheFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            var page = Content as Page;
            
            if (page == null)
                return;

            switch (e.NavigationMode)
            {
                case NavigationMode.Back:
                    if (!CachedAll)
                    {
                        page.NavigationCacheMode = NavigationCacheMode.Disabled;
                    }
                    break;

                case NavigationMode.Forward:
                case NavigationMode.Refresh:
                case NavigationMode.New:
                    if (page.GetType() == e.SourcePageType && !CachedAll)
                    {
                        //如果相同类型，则关闭缓存
                        page.NavigationCacheMode = NavigationCacheMode.Disabled;
                    }
                    else
                    {
                        page.NavigationCacheMode = NavigationCacheMode.Required;
                    }

                    break;
            }
            UmengSDK.UmengAnalytics.TrackPageEnd(page.GetType().Name);
        }

        private void CacheFrame_Navigated(object sender, NavigationEventArgs e)
        {
            var page = Content as Page;
            if (page == null)
                return;

            switch (e.NavigationMode)
            {
                case NavigationMode.Back:
                case NavigationMode.Forward:
                case NavigationMode.Refresh:
                case NavigationMode.New:
                    page.NavigationCacheMode = NavigationCacheMode.Required;
                    break;
            }
            UmengSDK.UmengAnalytics.TrackPageStart(page.GetType().Name);
        }
    }
}
