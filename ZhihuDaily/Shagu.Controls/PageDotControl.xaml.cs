using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Shagu.Controls
{
    public sealed partial class PageDotControl 
    {
        public PageDotControl()
        {
            FilpViewWidth = 640;
            FilpViewHeight = 300;

            InitializeComponent();
        }

        public double FilpViewWidth { get; set; }
        public double FilpViewHeight { get; set; }

        private void flipView_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var flip = (FlipView)sender;
            flip.Height = e.NewSize.Width * FilpViewHeight / FilpViewWidth;
        }


        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource", typeof (object), typeof (PageDotControl), new PropertyMetadata(default(object)));

        public object ItemsSource
        {
            get { return (object) GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(
            "ItemTemplate", typeof (DataTemplate), typeof (PageDotControl), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate) GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(
            "SelectedIndex", typeof (int), typeof (PageDotControl), new PropertyMetadata(-1));

        public int SelectedIndex
        {
            get { return (int) GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

    }
}
