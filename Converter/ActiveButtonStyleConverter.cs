using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace BookingApp.Converter
{
    public class ActiveButtonStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string currentContent = value?.ToString();
                string activeContent = parameter?.ToString();

                    if (currentContent == activeContent)
                    {
                        return (Style)Application.Current.Resources["ActiveButtonStyle"];
                    }
                    else
                    {
                        return (Style)Application.Current.Resources["MenuButton"];
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in Convert: {ex.Message}");
                return null;
            }
        }



        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
