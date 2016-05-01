using System;
using System.Text.RegularExpressions;
using Windows.ApplicationModel.Core;
using Windows.Foundation.Collections;
using Windows.Globalization;
using Windows.Security.Authentication.Web;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Caliburn.Micro;
using Shagu.Utils.Utils;
using Shagu.Weibo.Social;
using ZhihuDaily.Domain.Core;
using ZhihuDaily.Domain.Service;
using ZhihuDaily.Util;
using ZhihuDaily.Utils;

namespace ZhihuDaily.ViewModels
{
    public class LoginViewModel : Screen
    {
        private GlobalInfoManager _globalInfoManager;
        public LoginViewModel(GlobalInfoManager globalInfoManager)
        {
            _globalInfoManager = globalInfoManager;

        }


        public async void Login()
        {
           
        }
    }
}
