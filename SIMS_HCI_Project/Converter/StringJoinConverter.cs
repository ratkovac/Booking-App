using System;
using System.Globalization;
using System.Windows.Data;
using System.Collections;
using System.Linq;

namespace BookingApp.View.Driver
{
    public class StringJoinConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable enumerable)
            {
                return string.Join(", ", enumerable.Cast<object>());
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}