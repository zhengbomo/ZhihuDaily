using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Brain.Animate;
using Shagu.Utils.Controls;

namespace Shagu.Utils.Utils
{
    public class ToastHelper
    {
        public static void ToastInfo(string message, bool isNightMode, Action action = null)
        {
            if (DeviceInfoHelper.DeviceType == DeviceType.Mobile)
            {
                ToastCenterView(message, "info", isNightMode, action);
            }
            else
            {
                ToastRightView(message, "info", action);
            }
        }

        public static void ToastError(string error, bool isNightMode = false)
        {
            if (DeviceInfoHelper.DeviceType == DeviceType.Mobile)
            {
                ToastCenterView(error, "error", isNightMode);
            }
            else
            {
                ToastRightView(error, "error");
            }
        }

        private static async void ToastRightView(string text, string type, Action action = null)
        {
            var toastView = new RightToastView(text, type);

            var popup = new Popup
            {
                Child = new Border
                {
                    Width = Window.Current.Bounds.Width,
                    Height = Window.Current.Bounds.Height,
                    Child = toastView
                },
                IsOpen = true
            };

            await Task.WhenAll(toastView.AnimateAsync(new BounceInLeftAnimation()));
            await toastView.AnimateAsync(new BounceOutRightAnimation { Delay = 1.5 });

            popup.IsOpen = false;

            action?.Invoke();
        }

        private static async void ToastCenterView(string text, string type, bool isNightMode, Action action = null)
        {

            var toastView = new ToastView(text, isNightMode);


            var popup = new Popup
            {
                Child = new Border
                {
                    Width = Window.Current.Bounds.Width,
                    Height = Window.Current.Bounds.Height,
                    Child = toastView
                },
                IsOpen = true
            };

            await Task.WhenAll(toastView.AnimateAsync(new FadeInAnimation()));
            await toastView.AnimateAsync(new FadeOutAnimation { Delay = 1.5 });

            popup.IsOpen = false;

            action?.Invoke();
        }
    }
}
