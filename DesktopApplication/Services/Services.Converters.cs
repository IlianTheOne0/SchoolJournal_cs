namespace DesktopApplication.Services.Converters;

using Models.Supports.GradesAssigner;
using Models.Tables.Attending;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

public class ServicesConvertersAttendanceStatus : IValueConverter
{
    public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
    {
        if (Value is ModelsAttending attendance) { return attendance.Sickness ? "S" : "A"; }
        return string.Empty;
    }

    public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture) => throw new NotImplementedException();
}

public class ServicesConverterBooleanToBorderBrush : IValueConverter
{
    public Brush TrueValue { get; set; } = Brushes.Transparent;
    public Brush FalseValue { get; set; } = Brushes.Black;
    public Brush FocusedValue { get; set; } = Brushes.Transparent;

    public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
    {
        if (Value is bool isReadOnly) { return isReadOnly ? TrueValue : FalseValue; }
        return TrueValue;
    }

    public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture) => throw new NotSupportedException();
}

public class ServicesConvertersBooleanToVisibility : IValueConverter
{
    public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture) => (bool)Value ? Visibility.Visible : Visibility.Collapsed;
    public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture) => (Visibility)Value == Visibility.Visible;
}

public class ServicesConvertersBoolToGender : IValueConverter
{
    public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
    {
        if (Value is bool sex) { return sex ? "Male" : "Female"; }
        return null!;
    }

    public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture) => (string)Value == "Male";
}

public class ServicesConvertersGradeValue : IValueConverter
{
    public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
    {
        if (Value is GradeAssignment assignment) { return assignment.Grade?.ToString() ?? string.Empty; }
        return "N/A";
    }

    public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture) => throw new NotImplementedException();
}

public class ServicesConvertersInverseBoolean : IValueConverter
{
    public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
    {
        if (Value is bool boolValue) { return !boolValue; }
        return true;
    }

    public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture)
    {
        if (Value is bool boolValue) { return !boolValue; }
        return false;
    }
}

public class ServicesConvertersInverseBooleanToVisibility : IValueConverter
{
    public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture) => (Value is bool boolValue && boolValue) ? Visibility.Collapsed : Visibility.Visible;

    public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture) => (Visibility)Value != Visibility.Visible;
}

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

public class ServicesConvertersNotAddNewOrNone : IValueConverter
{
    public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
    {
        if (Value is int id) { return id > 0; }
        return false;
    }
    public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture) => throw new NotImplementedException();
}

public class ServicesConvertersAddNewOrNone : IValueConverter
{
    public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
    {
        if (Value is int id) { return id <= 0; }
        return false;
    }
    public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture) => throw new NotImplementedException();
}