namespace DesktopApplication.Services.Converters;

using System.Globalization;
using System.Windows.Data;

public class ServicesConvertersUriValidationConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString())) { return null!; }

        try { return new Uri(value.ToString()!, UriKind.RelativeOrAbsolute); }
        catch { return null!; }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}