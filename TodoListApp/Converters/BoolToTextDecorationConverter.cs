using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace TodoListApp.Converters;

public class BoolToTextDecorationConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isCompleted && isCompleted)
            return TextDecorations.Strikethrough;
        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}