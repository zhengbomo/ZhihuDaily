using System.Collections.Generic;
using Windows.UI.Xaml;
using Shagu.Utils.Models;

namespace ZhihuDaily.Controls
{
    public sealed partial class CategoryListView
    {
        public CategoryListView()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource", typeof (IEnumerable<HeaderInfo<Thickness, CornerRadius>>), typeof (CategoryListView),
            new PropertyMetadata(default(IEnumerable<HeaderInfo<Thickness, CornerRadius>>)));

        public IEnumerable<HeaderInfo<Thickness, CornerRadius>> ItemsSource
        {
            get { return (IEnumerable<HeaderInfo<Thickness, CornerRadius>>) GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(
            "SelectedIndex", typeof (int), typeof (CategoryListView), new PropertyMetadata(default(int)));

        public int SelectedIndex
        {
            get { return (int) GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }
    }
}