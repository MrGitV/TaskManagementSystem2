using System;
using System.Globalization;
using System.Windows.Data;

namespace ClientApp.Infrastructure
{
    // Converts an object reference to a boolean (true if not null, false if null).
    public class NotNullToBoolConverter : IValueConverter
    {
        // Converts a null/non-null object to a boolean.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        // Converting back is not supported.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}