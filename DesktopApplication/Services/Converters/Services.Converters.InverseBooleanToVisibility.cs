namespace DesktopApplication.Services.Converters;

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

public class ServicesConvertersInverseBooleanToVisibility : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) { return (value is bool boolValue && boolValue) ? Visibility.Collapsed : Visibility.Visible; }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { return (Visibility)value != Visibility.Visible; }
}