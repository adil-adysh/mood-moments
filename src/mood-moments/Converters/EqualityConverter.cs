using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace mood_moments.Converters
{
    public class EqualityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null && parameter == null)
                return true;
            if (value == null || parameter == null)
                return false;
            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Not used for one-way binding
            throw new NotImplementedException();
        }
    }
}
