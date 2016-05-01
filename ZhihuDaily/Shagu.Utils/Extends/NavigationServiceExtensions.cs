using System;
using System.Linq;
using Windows.UI.Xaml.Navigation;
using Caliburn.Micro;

namespace Shagu.Utils.Extends
{
    public static class NavigationServiceExtensions
    {
        public static void BackToMainPage(this INavigationService navigationService)
        {
            while (navigationService.BackStack.Count > 1)
            {
                navigationService.BackStack.RemoveAt(1);
            }
            navigationService.GoBack();
        }

        public static void GoTo<TViewModel>(this INavigationService navigationService)
        {
            navigationService.For<TViewModel>().Navigate();
            while (navigationService.BackStack.Count > 0)
            {
                navigationService.BackStack.RemoveAt(0);
            }
        }

        public static void GoTo<TViewModel>(this NavigateHelper<TViewModel> uriBuilder)
        {
            var navigationService = IoC.Get<INavigationService>();
            uriBuilder.Navigate();
            while (navigationService.BackStack.Count > 0)
            {
                navigationService.BackStack.RemoveAt(0);
            }
        }


        /// <summary>
        /// 扩展INavigationService,让导航支持复杂对象赋值
        /// </summary>
        public static void Navigate<TViewModel>(this INavigationService navigationService, Action<TViewModel> action) where TViewModel : class
        {
            NavigatedEventHandler navigationServerOnNavigated = null;
            navigationServerOnNavigated = (s, e) =>
            {
                var viewModel = ViewModelLocator.LocateForView(e.Content) as TViewModel;
                if (viewModel != null)
                {
                    action(viewModel);
                }
                navigationService.Navigated -= navigationServerOnNavigated;
            };

            navigationService.Navigated += navigationServerOnNavigated;
            navigationService.For<TViewModel>().Navigate();
        }

        public static void Navigate<TViewModel>(this NavigateHelper<TViewModel> uriBuilder, INavigationService navigationService, Action<TViewModel> action) where TViewModel : class
        {
            NavigatedEventHandler navigationServerOnNavigated = null;
            navigationServerOnNavigated = (s, e) =>
            {
                var viewModel = ViewModelLocator.LocateForView(e.Content) as TViewModel;
                if (viewModel != null)
                {
                    action(viewModel);
                }
                navigationService.Navigated -= navigationServerOnNavigated;
            };

            navigationService.Navigated += navigationServerOnNavigated;
            uriBuilder.Navigate();
        }



        /// <summary>
        /// 返回到匹配的导航页面（true:存在且返回成功，false:不存在不操作）
        /// </summary>
        public static void RemoveToView<TView>(this INavigationService navigationService)
        {
            foreach (var journalEntry in navigationService.BackStack.Reverse())
            {
                if (journalEntry.SourcePageType == typeof(TView))
                {
                    break;
                }
                else
                {
                    navigationService.BackStack.Remove(journalEntry);
                }
            }
        }

        public static void RemoveBackEntry(this INavigationService navigationService)
        {
            navigationService.BackStack.RemoveAt(navigationService.BackStack.Count - 1);
        }
    }
}
