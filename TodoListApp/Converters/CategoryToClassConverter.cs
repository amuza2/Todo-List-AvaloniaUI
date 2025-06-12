using System;
using System.Globalization;
using Avalonia.Data.Converters;
using TodoListApp.Models;

namespace TodoListApp.Converters;

public class CategoryToClassConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is TaskCategory category)
        {
            return category switch
            {
                TaskCategory.Academic => "category-academic",
                TaskCategory.Work => "category-work",
                TaskCategory.Personal => "category-personal",
                TaskCategory.Health => "category-health",
                _ => "category-academic"
            };
        }
        return "category-academic";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}