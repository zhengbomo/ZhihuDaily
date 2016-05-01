using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace Shagu.Controls
{
    public class TemplatedListBox : ListBox
    {
        private const string ListBoxItemStyleXaml =
            @"<Style TargetType=""ListBoxItem"" xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
	            <Setter Property=""Background"" Value=""Transparent"" />
	            <Setter Property=""TabNavigation"" Value=""Local"" />
	            <Setter Property=""Padding"" Value=""6.5,8"" />
	            <Setter Property=""HorizontalContentAlignment"" Value=""Stretch"" />
                <Setter Property=""VerticalContentAlignment"" Value=""Stretch""/>
	            <Setter Property=""Template"">
		            <Setter.Value>
			            <ControlTemplate TargetType=""ListBoxItem"">
				            <Border Background=""{TemplateBinding Background}""
						            BorderBrush=""{TemplateBinding BorderBrush}""
						            BorderThickness=""{TemplateBinding BorderThickness}""
						            x:Name=""LayoutRoot"">
					            <VisualStateManager.VisualStateGroups>
						            <VisualStateGroup x:Name=""CommonStates"">
							            <VisualStateGroup.Transitions>
								            <VisualTransition From=""Pressed"" To=""Normal"">
									            <Storyboard>
										            
									            </Storyboard>
								            </VisualTransition>
							            </VisualStateGroup.Transitions>
							            <VisualState x:Name=""Normal"" />
							            <VisualState x:Name=""PointerOver"" />
							            <VisualState x:Name=""Disabled"" />
							            <VisualState x:Name=""Pressed"">
								            <Storyboard>
									            
								            </Storyboard>
							            </VisualState>
						            </VisualStateGroup>
						            <VisualStateGroup x:Name=""SelectionStates"">
							            <VisualState x:Name=""Unselected"" />
							            <VisualState x:Name=""Selected"" />
							            <VisualState x:Name=""SelectedUnfocused"" />
							            <VisualState x:Name=""SelectedDisabled"" />
							            <VisualState x:Name=""SelectedPointerOver"" />
							            <VisualState x:Name=""SelectedPressed"" />
						            </VisualStateGroup>
					            </VisualStateManager.VisualStateGroups>
					            <Grid Background=""Transparent"" x:Name=""InnerGrid"">
						            <ContentPresenter Content=""{TemplateBinding Content}""
										              ContentTemplate=""{TemplateBinding ContentTemplate}""
										              ContentTransitions=""{TemplateBinding ContentTransitions}""
										              HorizontalAlignment=""{TemplateBinding HorizontalContentAlignment}""
										              Margin=""{TemplateBinding Padding}""
										              VerticalAlignment=""{TemplateBinding VerticalContentAlignment}"" />
					            </Grid>
				            </Border>
			            </ControlTemplate>
		            </Setter.Value>
	            </Setter>
            </Style>";


        public TemplatedListBox()
        {
            SelectionMode = SelectionMode.Single;
            ItemContainerStyle = (Style)XamlReader.Load(ListBoxItemStyleXaml);
            Loaded += TemplatedListBox_Loaded;
        }

        private void TemplatedListBox_Loaded(object sender, RoutedEventArgs e)
        {
            SelectionChanged += TemplatedListBox_SelectionChanged;

            if (SelectedIndex != -1)
            {
                var selectedListBoxItem = (ListBoxItem)ContainerFromIndex(SelectedIndex);
                var items = FindVisualChild<ContentPresenter>(selectedListBoxItem);
                items.ContentTemplate = SelectedTemplate;
            }
        }

        void TemplatedListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var addedItem in e.AddedItems)
            {
                var selectedListBoxItem = (ListBoxItem)ContainerFromItem(addedItem);
                var items = FindVisualChild<ContentPresenter>(selectedListBoxItem);
                items.ContentTemplate = SelectedTemplate;
            }

            foreach (var removedItem in e.RemovedItems)
            {
                var selectedListBoxItem = (ListBoxItem)ContainerFromItem(removedItem);
                var items = FindVisualChild<ContentPresenter>(selectedListBoxItem);
                items.ContentTemplate = ItemTemplate;
            }
        }

        public static readonly DependencyProperty SelectedTemplateProperty = DependencyProperty.Register(
            "SelectedTemplate", typeof(DataTemplate), typeof(TemplatedListBox), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate SelectedTemplate
        {
            get { return (DataTemplate)GetValue(SelectedTemplateProperty); }
            set { SetValue(SelectedTemplateProperty, value); }
        }

        //查找可视化树某个类型的元素
        private T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }


    }
}