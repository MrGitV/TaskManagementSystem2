using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ClientApp.Infrastructure
{
    // Converts a string representing a color name into a Brush object.
    public class StringToBrushConverter : IValueConverter
    {
        // Converts a color name string to a Brush.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string colorName)
            {
                try
                {
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorName));
                }
                catch
                {
                    return Brushes.Black;
                }
            }
            return Brushes.Black;
        }

        // Converting back is not supported.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}