using System;
using Windows.UI.Xaml;
using Caliburn.Micro;
using Shagu.UI.Utils;
using ZhihuDaily.Domain.Models;

namespace ZhihuDaily.Controls
{
    public sealed partial class AvatarControl
    {
        private IFrameManager _frameManager => IoC.Get<IFrameManager>();

        public event EventHandler<UserInfo> AvatarClick; 

        public AvatarControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty UserInfoProperty = DependencyProperty.Register(
            "UserInfo", typeof(UserInfo), typeof(AvatarControl), new PropertyMetadata(default(UserInfo)));

        public UserInfo UserInfo
        {
            get { return (UserInfo)GetValue(UserInfoProperty); }
            set { SetValue(UserInfoProperty, value); }
        }

        public static readonly DependencyProperty LoginResultProperty = DependencyProperty.Register(
            "LoginResult", typeof (LoginResult), typeof (AvatarControl), new PropertyMetadata(default(LoginResult), LoginResultChangedCallback));

        private static void LoginResultChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var avatarControl = (AvatarControl) dependencyObject;
            if (avatarControl.LoginResult != null)
            {
                avatarControl.UserInfo = new UserInfo
                {
                    Avatar = avatarControl.LoginResult.Avatar,
                    Name = avatarControl.LoginResult.Name
                };
            }
        }

        public LoginResult LoginResult
        {
            get { return (LoginResult) GetValue(LoginResultProperty); }
            set { SetValue(LoginResultProperty, value); }
        }

        private void Avatar_OnClick(object sender, RoutedEventArgs e)
        {
            if (UserInfo != null)
            {
                OnAvatarClick(UserInfo);
            }
        }

        private void OnAvatarClick(UserInfo e)
        {
            AvatarClick?.Invoke(this, e);
        }
    }
}
