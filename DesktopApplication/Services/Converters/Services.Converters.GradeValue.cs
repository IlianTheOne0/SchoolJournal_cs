namespace DesktopApplication.Services.Converters;

using Models.Supports.GradesAssigner;
using System;
using System.Globalization;
using System.Windows.Data;

public class ServicesConvertersGradeValue : IValueConverter
{
    public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
    {
        if (Value is GradeAssignment assignment) { return assignment.Grade?.ToString() ?? string.Empty; }
        return string.Empty;
    }
    
    public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture) => throw new NotImplementedException();
}