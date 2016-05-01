using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Shagu.Utils.Controls.ContentDialog;

namespace Shagu.Utils.Utils
{
    public class MessageBox
    {
        private static bool _isShowed;

        public static async Task<ContentDialogResult> ShowAsync(string messageBoxText, string caption, string primaryText = "确定", string secondaryText = "取消")
        {
            if (!_isShowed)
            {
                _isShowed = true;
                var dialog = new ConfirmDialog
                {
                    Title = caption,
                    PrimaryButtonText = primaryText,
                    SecondaryButtonText = secondaryText,
                    Text = messageBoxText
                };
                var result = await dialog.ShowAsync();
                _isShowed = false;

                return result;
            }
            return ContentDialogResult.None;
        }

        public static async Task ShowAsync(string messageBoxText)
        {
            if (!_isShowed)
            {
                _isShowed = true;
                MessageDialog dialog = new MessageDialog(messageBoxText);
                await dialog.ShowAsync();
                _isShowed = false;
            }
        }
    }
}
