using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace TodoListApp.Converters;

public class DateTimeToNullableConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is DateTime date ? (DateTime?)date : null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value ?? default(DateTime);
    }
}