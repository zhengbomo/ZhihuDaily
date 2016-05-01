using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Caliburn.Micro;
using Shagu.Utils;
using Shagu.Utils.Utils;
using ZhihuDaily.Domain.Core;
using ZhihuDaily.Util;

namespace ZhihuDaily.Controls
{
    public sealed partial class ExtendedSplash
    {
        private readonly GlobalInfoManager globalInfoManager;
        public ExtendedSplash(SplashScreen splashscreen, GlobalInfoManager globalInfoManager)
        {
            InitializeComponent();
            this.globalInfoManager = globalInfoManager;

            imageInfo.Text = globalInfoManager.StartText;
        }

        public async Task DoSomeThing()
        {
            if (globalInfoManager.IsStartImageReady)
            {
                try
                {
                    sourceImage.Source = new BitmapImage(new Uri(string.Format("ms-appdata:///local/{0}",
                        Constants.StartImageSourcePath.Replace("\\", "/"))));

                    blurImage.Source = new BitmapImage(new Uri(string.Format("ms-appdata:///local/{0}",
                        Constants.StartImageBluePath.Replace("\\", "/"))));
                }
                catch (Exception e)
                {
                    Debug.WriteLine("闪屏图片错误", e);
                }
            }

#if DEBUG
                await Task.Delay(1500);
#else
            if (DeviceInfoHelper.DeviceType == DeviceType.Mobile)
            {
                await Task.Delay(1000);
            }
            else
            {
                await Task.Delay(1500);
            }
#endif


        }


    }
}
