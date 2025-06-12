using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace TodoListApp.Converters;

public class ProgressToWidthConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double percentage)
        {
            // Assuming a container width, you might want to bind this to the actual container width
            return percentage * 4; // Adjust multiplier based on your container width
        }
        return 0.0;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}