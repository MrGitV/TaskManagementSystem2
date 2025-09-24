using System;
using System.Globalization;
using System.Windows.Data;

namespace ClientApp.Infrastructure
{
    // Converts an integer value to a boolean.
    public class IntToBoolConverter : IValueConverter
    {
        // Converts an integer to a boolean.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int i)
            {
                return i > 0;
            }
            return false;
        }

        // Converting back is not supported.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}