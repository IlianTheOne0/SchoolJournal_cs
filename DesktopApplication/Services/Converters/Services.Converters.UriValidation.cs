namespace DesktopApplication.Services.Converters;

using System.Globalization;
using System.Windows.Data;
using System.Windows;

public class ServicesConvertersUriValidationConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString())) { return DependencyProperty.UnsetValue; }

        try { return new Uri(value.ToString()!, UriKind.RelativeOrAbsolute); }
        catch { return DependencyProperty.UnsetValue; }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}