using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Shagu.Controls.Converters
{
    /// <summary>
    /// 布尔值转换：支持null, bool, visibility
    /// </summary>
    public class BooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            var p = (string)parameter;
            switch (p)
            {
                case "null":
                    return value == null;
                case "nullreverse":
                    return value != null;
                case "visible":
                    return value.Equals(Visibility.Visible);
                case "collapsed":
                    return value.Equals(Visibility.Collapsed);
                case "bool":
                    return value;
                case "boolreverse":
                    return !(bool)value;
                case "nullablebool":
                    return (value as bool?) == true;
                case "nullableboolreverse":
                    return (value as bool?) != true;
                case "zero":
                    return (int)value == 0;
                case "zeroreverse":
                    return (int)value != 0;
                case "empty":
                    return string.IsNullOrEmpty((string)value);
                case "emptyreverse":
                    return !string.IsNullOrEmpty((string)value);
                default:
                    if (p.StartsWith("equal(") && p.EndsWith(")"))
                    {
                        return value.ToString().Equals(p.Substring(6, p.Length - 7));
                    }
                    if (p.StartsWith("equalreverse(") && p.EndsWith(")"))
                    {
                        return !value.ToString().Equals(p.Substring(6, p.Length - 7));
                    }
                    break;
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotImplementedException();
        }
    }
}
