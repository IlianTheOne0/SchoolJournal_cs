namespace DesktopApplication.Services.Converters;

using System.Globalization;
using System.Windows.Data;

public class ServicesConvertersBoolToGender : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool sex) { return sex ? "Male" : "Female"; }
        return null!;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => (string)value == "Male";
}