using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace mood_moments.Converters
{
    public class CategoryToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value?.ToString() switch
            {
                "Positive" => Colors.LightGreen,
                "Neutral" => Colors.LightYellow,
                "Negative" => Colors.LightPink,
                _ => Colors.Transparent
            };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
