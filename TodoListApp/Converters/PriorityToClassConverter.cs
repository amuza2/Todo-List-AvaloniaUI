using System;
using System.Globalization;
using Avalonia.Data.Converters;
using TodoListApp.Models;

namespace TodoListApp.Converters;

public class PriorityToClassConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is TaskPriority priority)
        {
            return priority switch
            {
                TaskPriority.High => "priority-high",
                TaskPriority.Medium => "priority-medium",
                TaskPriority.Low => "priority-low",
                _ => "priority-medium"
            };
        }
        return "priority-medium";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}