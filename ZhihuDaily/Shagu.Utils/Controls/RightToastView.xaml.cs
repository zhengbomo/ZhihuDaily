using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Shagu.Utils.Controls
{
    public sealed partial class RightToastView 
    {
        public static Brush InfoBackground { get; set; }
        public static Brush ErrorBackground { get; set; }

        static RightToastView()
        {

            //蓝色
//            InfoBackground = new SolidColorBrush(Color.FromArgb(255, 104, 136, 118));
            InfoBackground = new SolidColorBrush(Colors.Red);
            //红色
            ErrorBackground = new SolidColorBrush(Color.FromArgb(255, 248, 78, 78));
        }

        public RightToastView()
        {
            InitializeComponent();
        }


        public RightToastView(string text, string type):this()
        {
            infoTextBlock.Text = text;
            switch (type)
            {
                case "info":
                    rootBorder.Background = InfoBackground;
                    break;
                case "error":
                    rootBorder.Background = ErrorBackground;
                    break;
            }
        }
    }
}
