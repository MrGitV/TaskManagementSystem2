using System;
using System.Globalization;
using System.Windows.Data;

namespace ClientApp.Infrastructure
{
    // A value converter that checks if an object is of a specific type.
    public class TypeCheckConverter : IValueConverter
    {
        // Checks if the value's type matches the type name passed as a parameter.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            return value.GetType() == Type.GetType(parameter.ToString(), false);
        }

        // Converting back is not supported.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}