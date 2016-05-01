using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Shagu.Utils.Controls.ContentDialog
{
    public enum SignInResult
    {
        SignInOk,
        SignInCancel,
    }

    public sealed partial class ConfirmDialog
    {
        public SignInResult Result { get; private set; }


        public ConfirmDialog()
        {
            InitializeComponent();
            Result = SignInResult.SignInCancel;
        }

       

        private void ContentDialog_PrimaryButtonClick(Windows.UI.Xaml.Controls.ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var deferral = args.GetDeferral();
            Result = SignInResult.SignInOk;
            deferral.Complete();
        }

        private void ContentDialog_SecondaryButtonClick(Windows.UI.Xaml.Controls.ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Result = SignInResult.SignInCancel;
        }


        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
           "Text", typeof(string), typeof(ConfirmDialog), new PropertyMetadata(string.Empty));


        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}
