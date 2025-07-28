using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace mood_moments.Converters
{
    public class SelectedToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return (value is bool b && b) ? Colors.LightBlue : Colors.Transparent;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
