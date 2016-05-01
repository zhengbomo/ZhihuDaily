using System;
using Windows.UI.Xaml.Data;

namespace Shagu.Controls.Converters
{
    public class DateConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var date = (double) value;

            var time = new DateTime(1970, 1, 1) + TimeSpan.FromSeconds(date);
            return time.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
