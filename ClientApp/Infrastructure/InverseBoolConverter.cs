using System;
using System.Globalization;
using System.Windows.Data;

namespace ClientApp.Infrastructure
{
    // Inverts a boolean value.
    public class InverseBoolConverter : IValueConverter
    {
        // Converts a boolean to its opposite value.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return false;
        }

        // Converts an inverted boolean back to its original value.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return false;
        }
    }
}