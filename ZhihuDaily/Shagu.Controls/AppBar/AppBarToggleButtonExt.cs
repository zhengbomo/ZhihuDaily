using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Shagu.Controls.AppBar
{
    public class AppBarToggleButtonExt : AppBarToggleButton
    {
        public AppBarToggleButtonExt()
        {
            DefaultStyleKey = typeof (AppBarToggleButtonExt);

            Loaded += IconToggleButton_Loaded;
            Checked += IconToggleButton_Checked;
            Unchecked += IconToggleButton_Unchecked;
        }
        public static readonly DependencyProperty CheckedForegroundProperty = DependencyProperty.Register(
            "CheckedForeground", typeof (Brush), typeof (AppBarToggleButtonExt), new PropertyMetadata(default(Brush)));

        public Brush CheckedForeground
        {
            get { return (Brush) GetValue(CheckedForegroundProperty); }
            set { SetValue(CheckedForegroundProperty, value); }
        }

        public static readonly DependencyProperty UncheckedForegroundProperty = DependencyProperty.Register(
            "UncheckedForeground", typeof (Brush), typeof (AppBarToggleButtonExt), new PropertyMetadata(default(Brush)));

        public Brush UncheckedForeground
        {
            get { return (Brush) GetValue(UncheckedForegroundProperty); }
            set { SetValue(UncheckedForegroundProperty, value); }
        }

        public static readonly DependencyProperty CheckedTextProperty = DependencyProperty.Register(
            "CheckedText", typeof (string), typeof (AppBarToggleButtonExt), new PropertyMetadata(default(string)));

        public string CheckedText
        {
            get { return (string) GetValue(CheckedTextProperty); }
            set { SetValue(CheckedTextProperty, value); }
        }

        public static readonly DependencyProperty UncheckedTextProperty = DependencyProperty.Register(
            "UncheckedText", typeof (string), typeof (AppBarToggleButtonExt), new PropertyMetadata(default(string)));

        public string UncheckedText
        {
            get { return (string) GetValue(UncheckedTextProperty); }
            set { SetValue(UncheckedTextProperty, value); }
        }

        public static readonly DependencyProperty CheckedIconProperty = DependencyProperty.Register(
           "CheckedIcon", typeof(Uri), typeof(AppBarToggleButtonExt), new PropertyMetadata(default(Uri)));

        public Uri CheckedIcon
        {
            get { return (Uri)GetValue(CheckedIconProperty); }
            set { SetValue(CheckedIconProperty, value); }
        }

        public static readonly DependencyProperty UncheckedIconProperty = DependencyProperty.Register(
            "UncheckedIcon", typeof(Uri), typeof(AppBarToggleButtonExt), new PropertyMetadata(default(Uri)));

        public Uri UncheckedIcon
        {
            get { return (Uri)GetValue(UncheckedIconProperty); }
            set { SetValue(UncheckedIconProperty, value); }
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
                Label = CheckedText;
                Icon = new BitmapIcon {UriSource = CheckedIcon};
                Foreground = CheckedForeground;
            }
            else
            {
                Label = UncheckedText;
                Icon = new BitmapIcon {UriSource = UncheckedIcon};
                Foreground = UncheckedForeground;
            }
        }


        public bool IsManual { get; set; }
    }
}
