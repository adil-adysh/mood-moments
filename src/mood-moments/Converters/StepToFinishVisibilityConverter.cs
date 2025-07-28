using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace mood_moments.Converters
{
    public class StepToFinishVisibilityConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int step)
            {
                // Only show Finish button on the last step (assumed to be 6)
                return step == 6;
            }
            return false;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
