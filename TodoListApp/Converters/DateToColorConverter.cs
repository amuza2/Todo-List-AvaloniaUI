using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace TodoListApp.Converters;

public class DateToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is DateTime dateTime)
        {
            var today = DateTime.Today;

            if (dateTime.Date >= today) // today or future
                return Brushes.Green;
            
            if (dateTime.Date == today.AddDays(-1)) // yesterday
                return Brushes.Orange;
            
            if(dateTime.Date < today.AddDays(-1)) // before yesterday.
                return Brushes.Red;
                
        }
        return Brushes.Gray;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}