namespace DesktopApplication.Services.Converters;

using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

public class ServicesConverterBooleanToBorderBrush : IValueConverter
{
    public Brush TrueValue { get; set; } = Brushes.Transparent;
    public Brush FalseValue { get; set; } = Brushes.Black;
    public Brush FocusedValue { get; set; } = Brushes.Transparent;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isReadOnly) { return isReadOnly ? TrueValue : FalseValue; }
        return TrueValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
}