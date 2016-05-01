using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Microsoft.Xaml.Interactivity;

namespace ZhihuDaily.Actions
{
    /// <summary>
    /// 弹出浮动窗口，用法如下
    /// Behaviors SDK (XAML) 
    /// </summary>
    /// <example>
    /// <i:Interaction.Behaviors>
    ///    <ic:EventTriggerBehavior EventName="Holding">
    ///        <ia:OpenFlyoutAction />
    ///    </ic:EventTriggerBehavior>
    /// </i:Interaction.Behaviors>
    /// </example>

    public class OpenFlyoutAction : DependencyObject, IAction
    {
        
        public object Execute(object sender, object parameter)
        {
            var element = sender as FrameworkElement;
            var flyout = FlyoutBase.GetAttachedFlyout(element);

            if (flyout != null)
            {
                flyout.ShowAt(element);
            }

            return null;
        }
    }

}
