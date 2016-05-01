using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Caliburn.Micro;
using Shagu.Utils.Annotations;

namespace Shagu.UI.Utils
{
    public abstract class FrameManagerBase : INotifyPropertyChanged, IFrameManager
    {
        protected WinRTContainer Container { get; }

        /// <summary>
        /// 右边的容器导航改变（改变返回键）改变布局（Adapter）
        /// </summary>
        public event EventHandler<bool> RightFrameContentChange;

        /// <summary>
        /// 返回到主页面（通知UI改变ListView.SelectedIndex）
        /// </summary>
        public event EventHandler<object> Back2MainView;

        public event EventHandler<BackRequestedEventArgs> BackKeyPressing;

        private Frame _mainFrame;
        private Frame _centerFrame;

        protected FrameManagerBase(WinRTContainer container)
        {
            Container = container;
        }

        public Frame MainFrame
        {
            get { return _mainFrame; }
            set
            {
                MainNavService = new FrameAdapter(value);
                Container.RegisterInstance(typeof(INavigationService), "mainFrame", MainNavService);
                _mainFrame = value;
            }
        }
        public Frame CenterFrame
        {
            get { return _centerFrame; }
            set
            {
                CenterNavService = new FrameAdapter(value);
                Container.RegisterInstance(typeof(INavigationService), "centerFrame", CenterNavService);
                _centerFrame = value;
            }
        }

        public abstract Frame RightFrame { get; set; }


        public void RightFrameClearAndNav(Action<INavigationService> action)
        {
            action(RightNavService);
            while (RightNavService.BackStack.Count > 1)
            {
                RightNavService.BackStack.RemoveAt(1);
            }


            RightFrameContentChange?.Invoke(this, true);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        public void RightFrameAndNav(Action<INavigationService> action)
        {
            action(RightNavService);
            RightFrameContentChange?.Invoke(this, true);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        public bool RightFrameGoback()
        {
            if (RightNavService.CanGoBack)
            {
                RightNavService.GoBack();
                RightFrameContentChange?.Invoke(this, RightNavService.CanGoBack);
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = RightNavService.CanGoBack
                    ? AppViewBackButtonVisibility.Visible
                    : AppViewBackButtonVisibility.Collapsed;

                return true;
            }
            else
            {
                return false;
            }
        }

        public abstract void OnBackKeyPressed();


        public INavigationService MainNavService { get; private set; }

        public INavigationService CenterNavService { get; private set; }

        public abstract INavigationService RightNavService { get; protected set; }

        #region EventInvoke

        protected virtual void OnRightFrameContentChange(bool e)
        {
            RightFrameContentChange?.Invoke(this, e);
        }

        protected virtual void OnBack2MainView(object e)
        {
            Back2MainView?.Invoke(this, e);
        }


        protected virtual void OnBackKeyPressing(BackRequestedEventArgs e)
        {
            BackKeyPressing?.Invoke(this, e);
        }

        #endregion


        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
