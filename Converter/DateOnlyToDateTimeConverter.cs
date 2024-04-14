using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BookingApp.Converter
{
    public class DateOnlyToDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is DateOnly dateOnly ? new DateTime(dateOnly.Year, dateOnly.Month, dateOnly.Day) : Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is DateTime dateTime ? DateOnly.FromDateTime(dateTime) : Binding.DoNothing;
        }
    }
}
