using System;
using System.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Shagu.Controls.Converters
{
    /// <summary>
    /// 支持bool，null，zero转换
    /// </summary>
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            var p = (string)parameter;

            switch (p)
            {
                case "bool?":
                    var boolValue = (bool?) value;
                    return boolValue.HasValue && boolValue.Value ? Visibility.Visible : Visibility.Collapsed;
                case "bool?reverse":
                    boolValue = (bool?) value;
                    return boolValue.HasValue && !boolValue.Value ? Visibility.Visible : Visibility.Collapsed;
                case "bool":
                    return (bool)value ? Visibility.Visible : Visibility.Collapsed;
                case "boolreverse":
                    return (bool)value ? Visibility.Collapsed : Visibility.Visible;
                case "zero":
                    return value.Equals(0) ? Visibility.Visible : Visibility.Collapsed;
                case "zeroreverse":
                    return value.Equals(0) ? Visibility.Collapsed : Visibility.Visible;
                case "null":
                    return value == null ? Visibility.Visible : Visibility.Collapsed;
                case "nullreverse":
                    return value == null ? Visibility.Collapsed : Visibility.Visible;
                case "empty":
                    var str = (string) value;
                    return !string.IsNullOrEmpty(str?.Trim()) ? Visibility.Collapsed : Visibility.Visible;
                case "emptyreverse":
                    str = (string) value;
                    return !string.IsNullOrEmpty(str?.Trim()) ? Visibility.Visible : Visibility.Collapsed;
                case "listempty":
                    return value != null && ((IList) value).Count == 0 ? Visibility.Visible : Visibility.Collapsed;
                case "listnullorempty":
                    return value == null || ((IList) value).Count == 0 ? Visibility.Visible : Visibility.Collapsed;
                case "listemptyreverse":
                    return value != null && ((IList) value).Count > 0 ? Visibility.Visible : Visibility.Collapsed;
                case "listone":
                    return value != null && ((IList)value).Count == 1 ? Visibility.Visible : Visibility.Collapsed;
                case "listonereverse":
                    return value != null && ((IList)value).Count != 1 ? Visibility.Visible : Visibility.Collapsed;

                default:
                    if (p.StartsWith("equal(") && p.EndsWith(")"))
                    {
                        return value.ToString().Equals(p.Substring(6, p.Length - 7)) ? Visibility.Visible : Visibility.Collapsed;
                    }
                    else if (p.StartsWith("equalreverse(") && p.EndsWith(")"))
                    {
                        return value.ToString().Equals(p.Substring(13, p.Length - 14)) ? Visibility.Collapsed : Visibility.Visible;
                    }
                    break;
            }

            throw new NotSupportedException("不支持其他值的转换");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotImplementedException();
        }
    }
}
