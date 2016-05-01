using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Caliburn.Micro;
using Shagu.UI.Utils;

namespace ZhihuDaily.Views
{
    public sealed partial class MainShellView
    {
        public MainShellView()
        {
            InitializeComponent();
            Loaded += MainShellView_Loaded;
        }

        private void MainShellView_Loaded(object sender, RoutedEventArgs e)
        {
            IoC.Get<IFrameManager>().Back2MainView += MainShellView_Back2MainView;
            IoC.Get<IFrameManager>().BackKeyPressing += MainShellView_BackKeyPressing;
        }

        private void MainShellView_BackKeyPressing(object sender, BackRequestedEventArgs e)
        {
            if (mainSplitView.IsSwipeablePaneOpen)
            {
                mainSplitView.IsSwipeablePaneOpen = false;
                e.Handled = true;
            }
        }

        private void MainShellView_Back2MainView(object sender, object e)
        {
            mainListView.SelectedIndex = 0;
        }

        private void VisualStateGroup_OnCurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            if (e.OldState == Show123 || e.OldState == Show3)
            {
                if (e.NewState == Show12 || e.NewState == Show2)
                {
                    var animation = new PopInThemeAnimation
                    {
                        FromHorizontalOffset = 0,
                        FromVerticalOffset = 100,
                        SpeedRatio = 0.5,
                    };
                    Storyboard.SetTarget(animation, centerPanel);
                    var sb = new Storyboard();
                    sb.Children.Add(animation);
                    sb.Begin();
                }
            }
        }

        private void mainListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            mainSplitView.IsSwipeablePaneOpen = false;
        }

        private void ItemButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (mainSplitView.IsSwipeablePaneOpen)
            {
                mainSplitView.IsSwipeablePaneOpen = false;
            }
        }

        private void AvatarControl_OnAvatarClick(object sender, Domain.Models.UserInfo e)
        {
            mainListView.SelectedIndex = -1;
            if (mainSplitView.IsSwipeablePaneOpen)
            {
                mainSplitView.IsSwipeablePaneOpen = false;
            }
        }
    }
}
