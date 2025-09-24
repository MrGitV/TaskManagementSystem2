using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ClientApp.Infrastructure
{
    // Converts a boolean (is today) to a specific foreground color.
    public class BoolToTodayColorConverter : IValueConverter
    {
        // Converts a boolean indicating if a day is 'today' to a Brush.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool isToday && isToday) ? Brushes.Red : Brushes.Black;
        }

        // Converting back is not supported.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}