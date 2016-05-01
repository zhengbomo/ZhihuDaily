using System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Caliburn.Micro;

namespace Shagu.UI.Utils
{
    public interface IFrameManager
    {
        /// <summary>
        /// 右边的容器导航改变（改变返回键）改变布局（Adapter）
        /// </summary>
        event EventHandler<bool> RightFrameContentChange;

        /// <summary>
        /// 返回到主页面（通知UI改变ListView.SelectedIndex）
        /// </summary>
        event EventHandler<object> Back2MainView;

        /// <summary>
        /// 返回键
        /// </summary>
        event EventHandler<BackRequestedEventArgs> BackKeyPressing;

        Frame MainFrame { get; set; }
        Frame CenterFrame { get; set; }
        Frame RightFrame { get; set; }

        void RightFrameClearAndNav(Action<INavigationService> action);
        void RightFrameAndNav(Action<INavigationService> action);
        bool RightFrameGoback();

        //给外部手动调用返回的逻辑
        void OnBackKeyPressed();

        INavigationService MainNavService { get; }

        INavigationService CenterNavService { get; }

        INavigationService RightNavService { get; }
    }
}