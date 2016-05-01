using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Caliburn.Micro;

namespace Shagu.UI
{
    public class GlobalInfoManagerBase : PropertyChangedBase
    {
        public event EventHandler<ElementTheme> ThemeChanged;

        private readonly ApplicationDataContainer _globalSettingContainer;

        public GlobalInfoManagerBase()
        {
            _globalSettingContainer = ApplicationData.Current.LocalSettings.CreateContainer("globalsetting", ApplicationDataCreateDisposition.Always);
            OnThemeChanged(Theme);
        }

        #region GlobalSettingInfo

        #region LastVersion

        public string LastVersion
        {
            get { return GetGlobalInfo<string>("LastVersion", string.Empty); }
            set { SetGlobalInfo("LastVersion", value); }
        }


        #endregion

        #region ErrorInfo

        public string ErrorInfo
        {
            get { return GetGlobalInfo<string>("errorInfo", null); }
            set { SetGlobalInfo("errorInfo", value); }
        }


        #endregion

        #region Ad
        public int AdCreateTime
        {
            get { return GetGlobalInfo("AdCreateTime", 0); }
            set { SetGlobalInfo("AdCreateTime", value); }
        }

        /// <summary>
        /// 是否去广告
        /// </summary>
        public bool HasPayForAd
        {
            get { return GetGlobalInfo("HasPayForAd", false); }
            set { SetGlobalInfo("HasPayForAd", value); }
        }


        #endregion

        #region KeepTwoPanel

        public bool KeepTwoPanel
        {
            get { return GetGlobalInfo("keepTwoPanel", false); }
            set
            {
                SetGlobalInfo("keepTwoPanel", value);
                NotifyOfPropertyChange(nameof(KeepTwoPanel));
            }
        }

        #endregion

        #region NoPicture

        public bool NoPicture
        {
            get { return GetGlobalInfo("NoPicture", false); }
            set
            {
                SetGlobalInfo("NoPicture", value);
                NotifyOfPropertyChange(nameof(NoPicture));
            }
        }

        #endregion

        #region NoPicture

        public bool ArticleFullScreen
        {
            get { return GetGlobalInfo("ArticleFullScreen", false); }
            set
            {
                SetGlobalInfo("ArticleFullScreen", value);
                NotifyOfPropertyChange(nameof(ArticleFullScreen));
            }
        }

        #endregion


        #region Theme

        public ElementTheme Theme => IsNightMode ? ElementTheme.Dark : ElementTheme.Light;

        #endregion

        #region NightMode

        public bool IsNightMode
        {
            get { return GetGlobalInfo("IsNightMode", false); }
            set
            {
                SetGlobalInfo("IsNightMode", value);
                NotifyOfPropertyChange(nameof(IsNightMode));
                NotifyOfPropertyChange(nameof(Theme));
                OnThemeChanged(Theme);
            }
        }

        #endregion

        #region ArticleFontSize

        public double ArticleFontSize
        {
            get { return GetGlobalInfo("articleFontSize", 18.0); }
            set
            {
                SetGlobalInfo("articleFontSize", value);
                NotifyOfPropertyChange(nameof(ArticleFontSize));
            }
        }

        public double ArticleLineHeight
        {
            get { return GetGlobalInfo("ArticleLineHeight", 40.0); }
            set
            {
                SetGlobalInfo("ArticleLineHeight", value);
                NotifyOfPropertyChange(nameof(ArticleLineHeight));
            }
        }

        #endregion

        #region Rate

        public int UsedTime
        {
            get { return GetGlobalInfo("UsedTime", 0); }
            set { SetGlobalInfo("UsedTime", value); }
        }

        public bool HasRated
        {
            get { return GetGlobalInfo("HasRated", false); }
            set { SetGlobalInfo("HasRated", value); }
        }

        public bool ShouldRate => (UsedTime + 12) % 15 == 0 && !HasRated;

        #endregion

        #region Helper

        protected T GetGlobalInfo<T>(string key, T defaultValue)
        {
            if (_globalSettingContainer.Values.ContainsKey(key))
            {
                return (T)_globalSettingContainer.Values[key];
            }
            else
            {
                return defaultValue;
            }
        }

        protected void SetGlobalInfo<T>(string key, T value)
        {
            if (value == null)
            {
                _globalSettingContainer.Values.Remove(key);
            }
            _globalSettingContainer.Values[key] = value;
        }

        #endregion

        #endregion

        #region Event

        private void OnThemeChanged(ElementTheme e)
        {
            ThemeChanged?.Invoke(this, e);
        }

        #endregion
    }

}
