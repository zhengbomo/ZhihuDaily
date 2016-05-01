using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Shagu.Utils.Controls
{
    public sealed partial class LoadingView
    {
        private Popup _popup;

        public LoadingView()
        {
            InitializeComponent();
        }

       

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof (string), typeof (LoadingView), new PropertyMetadata(default(string)));

        public string Title
        {
            get { return (string) GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        
        public static LoadingView ShowLoading(string text)
        {
            var loadingView = new LoadingView { Title = text };
            
            loadingView._popup = new Popup
            {
                Child = new Border
                {
                    Width = Window.Current.Bounds.Width,
                    Height = Window.Current.Bounds.Height,
                    Child = loadingView
                },
                IsOpen = true
            };
            loadingView.Show();


            WindowSizeChangedEventHandler sizeChangeHalHandler = (s, e) =>
            {
                ((Border)loadingView._popup.Child).Width = e.Size.Width;
                ((Border)loadingView._popup.Child).Height = e.Size.Height;
            };

            Window.Current.SizeChanged += sizeChangeHalHandler;
            loadingView._popup.Closed += (s, e) =>
            {
                Window.Current.SizeChanged -= sizeChangeHalHandler;
            };

            return loadingView;
        }

        private void Show()
        {
            Visibility = Visibility.Visible;
            PopInStoryboard.Begin();
        }

        public void Hide()
        {
            PopOutStoryboard.Begin();
        }

        private void PopOutStoryboard_OnCompleted(object sender, object e)
        {
            Visibility = Visibility.Collapsed;
            _popup.IsOpen = false;
        }

        ~LoadingView()
        {
            
        }
    }
}
