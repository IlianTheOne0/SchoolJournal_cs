namespace DesktopApplication.Services.Converters;

using System.Globalization;
using System.Windows.Data;
using System.Windows;

public class ServicesConvertersUriValidationConverter : IValueConverter
{
    public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
    {
        if (Value == null || string.IsNullOrWhiteSpace(Value.ToString())) { return DependencyProperty.UnsetValue; }

        try { return new Uri(Value.ToString()!, UriKind.RelativeOrAbsolute); }
        catch { return DependencyProperty.UnsetValue; }
    }

    public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture) => throw new NotImplementedException();
}