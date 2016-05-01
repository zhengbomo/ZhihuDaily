using Windows.UI.Xaml;

namespace Shagu.Utils.Controls
{
    public sealed partial class ToastView
    {
        public ToastView(string info, bool isNightMode)
        {
            InitializeComponent();
            lightInfo.Text = darkInfo.Text = info;


            VisualStateManager.GoToState(this, isNightMode?"Light": "Dark", true);
        }


    }
}
