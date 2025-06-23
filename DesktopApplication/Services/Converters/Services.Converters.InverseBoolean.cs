namespace DesktopApplication.Services.Converters;

using System;
using System.Globalization;
using System.Windows.Data;

public class ServicesConvertersInverseBoolean : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue) { return !boolValue; }
        return true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue) { return !boolValue; }
        return false;
    }
}