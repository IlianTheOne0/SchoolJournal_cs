namespace Models.Support.GradesAssigner;

public class StudentGradeRow
{
    public int UserId { get; set; }
    public string StudentName { get; set; }
    public List<DayGrade> DayGrades { get; set; } = new();

    public IEnumerable<DayGrade> NonEmptyGrades => DayGrades.Where(dayGradesProvider => !string.IsNullOrEmpty(dayGradesProvider.Grade));
}

public class DayGrade
{
    public string Grade { get; set; }
    public string Comment { get; set; }
    public DateTime Date { get; set; }
    public string DisplayText => string.IsNullOrEmpty(Grade) ? "" : Grade;
}