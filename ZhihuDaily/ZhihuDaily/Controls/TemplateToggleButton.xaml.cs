using Windows.UI.Xaml;

namespace ZhihuDaily.Controls
{
    public sealed partial class TemplateToggleButton
    {
        public TemplateToggleButton()
        {
            InitializeComponent();
            Loaded += IconToggleButton_Loaded;
            Checked += IconToggleButton_Checked;
            Unchecked += IconToggleButton_Unchecked;
        }

        protected override void OnToggle()
        {
            if (!IsManual)
            {
                base.OnToggle();
            }
        }

        private void IconToggleButton_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateState();
        }

        private void IconToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateState();
        }

        private void IconToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            UpdateState();
        }

        private void UpdateState()
        {
            if (IsChecked.HasValue && IsChecked.Value)
            {
                Content = CheckedContent;
            }
            else
            {
                Content = UncheckedContent;
            }
        }

        public static readonly DependencyProperty IsManualProperty = DependencyProperty.Register(
            "IsManual", typeof (bool), typeof (TemplateToggleButton), new PropertyMetadata(default(bool)));

        //是否手动设值
        public bool IsManual
        {
            get { return (bool) GetValue(IsManualProperty); }
            set { SetValue(IsManualProperty, value); }
        }


        public static readonly DependencyProperty CheckedContentProperty = DependencyProperty.Register(
            "CheckedContent", typeof (object), typeof (TemplateToggleButton), new PropertyMetadata(default(object)));

        public object CheckedContent
        {
            get { return (object) GetValue(CheckedContentProperty); }
            set { SetValue(CheckedContentProperty, value); }
        }

        public static readonly DependencyProperty UncheckedContentProperty = DependencyProperty.Register(
            "UncheckedContent", typeof (object), typeof (TemplateToggleButton), new PropertyMetadata(default(object)));

        public object UncheckedContent
        {
            get { return (object) GetValue(UncheckedContentProperty); }
            set { SetValue(UncheckedContentProperty, value); }
        }
    }
}
