namespace DesktopApplication.Services.Converters;

using System;
using System.Globalization;
using System.Windows.Data;

public class ServicesConvertersInverseBoolean : IValueConverter
{
    public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
    {
        if (Value is bool boolValue) { return !boolValue; }
        return true;
    }

    public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture)
    {
        if (Value is bool boolValue) { return !boolValue; }
        return false;
    }
}