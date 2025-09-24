using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ClientApp.Infrastructure
{
    // Converts a boolean value to an inverted UI Visibility state.
    public class InverseBoolToVisibilityConverter : IValueConverter
    {
        // Converts a boolean to an inverted Visibility value.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool boolValue && !boolValue
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        // Converting back is not supported.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}