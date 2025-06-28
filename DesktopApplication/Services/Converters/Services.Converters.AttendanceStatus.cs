namespace DesktopApplication.Services.Converters;

using Models.Tables.Attending;
using System.Globalization;
using System.Windows.Data;

public class ServicesConvertersAttendanceStatus : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ModelsAttending attendance) { return attendance.Sickness ? "S" : "A"; }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}