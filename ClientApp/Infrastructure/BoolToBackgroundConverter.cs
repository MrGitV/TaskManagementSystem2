using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ClientApp.Infrastructure
{
    // Converts a boolean value to a Brush for UI background styling.
    public class BoolToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool boolValue && boolValue)
                ? Brushes.Transparent
                : Brushes.White;
        }

        // Converting back from a Brush to a boolean is not supported.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}