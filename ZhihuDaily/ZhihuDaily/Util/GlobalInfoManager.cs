using Windows.ApplicationModel.Store;
using Windows.Storage;
using Windows.UI.Xaml;
using Newtonsoft.Json;
using Shagu.UI;
using Shagu.Weibo.Social;
using ZhihuDaily.Domain.Core;
using ZhihuDaily.Domain.Models;

namespace ZhihuDaily.Util
{
    public class GlobalInfoManager : GlobalInfoManagerBase
    {
        private readonly ApplicationDataContainer _userSettingContainer;

        public GlobalInfoManager()
        {
            _userSettingContainer = ApplicationData.Current.LocalSettings.CreateContainer("userinfo",
                ApplicationDataCreateDisposition.Always);
        }

        public bool IsStartImageReady
        {
            get { return GetUserInfo<bool>("IsStartImageReady", false); }
            set { SetUserInfo("IsStartImageReady", value); }
        }

        public string StartImage
        {
            get { return GetUserInfo<string>("StartImage", null); }
            set { SetUserInfo("StartImage", value); }
        }

        public string StartText
        {
            get { return GetUserInfo<string>("StartText", string.Empty); }
            set { SetUserInfo("StartText", value); }
        }

        #region Ad
        public int AdCreateTime
        {
            get { return GetUserInfo("AdCreateTime", 0); }
            set { SetUserInfo("AdCreateTime", value); }
        }

        /// <summary>
        /// 是否去广告
        /// </summary>
        public bool HasPayForAd
        {
            get { return GetUserInfo("HasPayForAd", false); }
            set { SetUserInfo("HasPayForAd", value); }
        }


        #endregion

        #region UserInfo

        private LoginResult _loginResult;

        public LoginResult LoginResult
        {
            get
            {
                if (_loginResult == null)
                {
                    if (_userSettingContainer.Values.ContainsKey("LoginResult"))
                    {
                        var value = _userSettingContainer.Values["LoginResult"];
                        _loginResult = JsonConvert.DeserializeObject<LoginResult>((string)value);
                    }
                    else
                    {
                        _loginResult = new LoginResult { Name = "未登录" };
                    }
                }

                return _loginResult;
            }
            set
            {
                if (value == null)
                {
                    _userSettingContainer.Values.Remove("LoginResult");
                }
                else
                {
                    _userSettingContainer.Values["LoginResult"] = JsonConvert.SerializeObject(value);
                }
                _loginResult = value;
                NotifyOfPropertyChange(nameof(LoginResult));
                NotifyOfPropertyChange(nameof(UserInfo));
                NotifyOfPropertyChange(nameof(IsLogin));
            }
        }

        public UserInfo UserInfo => new UserInfo
        {
            Name = LoginResult?.Name,
            Avatar = LoginResult?.Avatar
        };

        public string UserName
        {
            set
            {
                LoginResult.Name = value;
                LoginResult = LoginResult;
            }
        }

        public string Avatar
        {
            set
            {
                LoginResult.Avatar = value;
                LoginResult = LoginResult;
            }
        }

        //是否已登录
        public bool IsLogin => LoginResult?.AccessToken != null;

        public string AccessToken => LoginResult == null ? string.Empty : LoginResult.AccessToken;


        private TokenResult _weiboToken;

        public TokenResult WeiboToken
        {
            get
            {
                if (_weiboToken == null)
                {
                    if (_userSettingContainer.Values.ContainsKey("WeiboToken"))
                    {
                        var value = _userSettingContainer.Values["WeiboToken"];
                        _weiboToken = JsonConvert.DeserializeObject<TokenResult>((string) value);
                    }
                }

                return _weiboToken;
            }
            set { _userSettingContainer.Values["WeiboToken"] = JsonConvert.SerializeObject(value); }
        }

        #endregion

        #region Helper

        protected T GetUserInfo<T>(string key, T defaultValue)
        {
            if (_userSettingContainer.Values.ContainsKey(key))
            {
                return (T)_userSettingContainer.Values[key];
            }
            else
            {
                return defaultValue;
            }
        }

        protected void SetUserInfo<T>(string key, T value)
        {
            if (value == null)
            {
                _userSettingContainer.Values.Remove(key);
            }
            _userSettingContainer.Values[key] = value;
        }

        #endregion

    }
}
