using System;
using System.Globalization;
using System.Threading.Tasks;
using Avalonia.Data.Converters;
using Avalonia.Media;
using TodoListApp.Models;

namespace TodoListApp.Converters;

public class PriorityToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is TaskPriority priority)
        {
            return priority switch
            {
                TaskPriority.High => Brushes.IndianRed,
                TaskPriority.Medium => Brushes.DarkOrange,
                TaskPriority.Low => Brushes.GreenYellow,
                _ => Brushes.Black
            };
        }
        return Brushes.Black;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}