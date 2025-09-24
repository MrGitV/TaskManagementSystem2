using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ClientApp.Infrastructure
{
    // Converts a string to a Visibility value (Visible if not empty, Collapsed if empty).
    public class StringToVisibilityConverter : IValueConverter
    {
        // Converts a string to Visibility. If the string is not empty, returns Visible; otherwise, Collapsed.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty(value as string) ? Visibility.Collapsed : Visibility.Visible;
        }

        // Converting back is not supported.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}