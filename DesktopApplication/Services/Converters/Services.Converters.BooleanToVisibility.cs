﻿namespace DesktopApplication.Services.Converters;

using System.Globalization;
using System.Windows;
using System.Windows.Data;

public class ServicesConvertersBooleanToVisibility : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (bool)value ? Visibility.Visible : Visibility.Collapsed;
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => (Visibility)value == Visibility.Visible;
}