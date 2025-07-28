using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace mood_moments.Converters
{
    public class NullOrEmptyToBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string str)
                return !string.IsNullOrEmpty(str);
            return value != null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // No-op for one-way binding
            return Binding.DoNothing;
        }
    }
}
