namespace Models.Supports.GradesAssigner;

public class StudentGradeAssignment
{
    public int StudentId { get; set; }
    public string StudentName { get; set; }
    public Dictionary<DateTime, GradeAssignment> Grades { get; set; } = new();
}

public class GradeAssignment
{
    public int? Grade { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
}

public class DateColumn
{
    public DateTime Date { get; set; }
    public string Header { get; set; } = string.Empty;
}