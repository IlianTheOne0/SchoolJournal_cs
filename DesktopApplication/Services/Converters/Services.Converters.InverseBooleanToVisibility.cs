namespace DesktopApplication.Services.Converters;

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

public class ServicesConvertersInverseBooleanToVisibility : IValueConverter
{
    public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture) => (Value is bool boolValue && boolValue) ? Visibility.Collapsed : Visibility.Visible;

    public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture) => (Visibility)Value != Visibility.Visible;
}