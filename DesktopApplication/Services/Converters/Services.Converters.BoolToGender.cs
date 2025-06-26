namespace DesktopApplication.Services.Converters;

using System.Globalization;
using System.Windows.Data;

public class ServicesConvertersBoolToGender : IValueConverter
{
    public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
    {
        if (Value is bool sex) { return sex ? "Male" : "Female"; }
        return null!;
    }

    public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture) => (string)Value == "Male";
}