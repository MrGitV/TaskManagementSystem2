using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ClientApp.Infrastructure
{
    // Converts a boolean value to a FontWeight (e.g., Bold or Normal).
    public class BoolToFontWeightConverter : IValueConverter
    {
        // Converts a boolean to a FontWeight.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool boolValue && boolValue) ? FontWeights.Bold : FontWeights.Normal;
        }

        // Converting back from a FontWeight to a boolean is not supported.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}