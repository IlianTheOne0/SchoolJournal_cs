namespace Models.Tables.Grades;

using Models.Tables.Attending;

using System.Runtime.CompilerServices;
using System.ComponentModel;

public class ModelsGradesExtended : ModelsGrades, INotifyPropertyChanged
{
    private string _subjectName;
    public string SubjectName { get => _subjectName; set { _subjectName = value; OnPropertyChanged(); } }

    private bool _isAbsence;
    public bool IsAbsence { get => _isAbsence; set { _isAbsence = value; OnPropertyChanged(); } }

    private bool _sickness;
    public bool Sickness { get => _sickness; set { _sickness = value; OnPropertyChanged(); } }

    public ModelsGradesExtended() { }

    public ModelsGradesExtended(ModelsGrades Grade, string SubjectName)
    {
        this.SubjectName = SubjectName;
        Id = Grade.Id;
        base.Grade = Grade.Grade;
        Description = Grade.Description;
        Date = Grade.Date;
        UserId = Grade.UserId;
        SubjectId = Grade.SubjectId;
        IsAbsence = false;
    }

    public ModelsGradesExtended(ModelsAttending Attendance, string SubjectName)
    {
        this.SubjectName = SubjectName;
        Id = Attendance.Id;
        Grade = 0;
        Description = Attendance.Sickness ? "Sickness" : "Absence";
        Date = Attendance.Date;
        UserId = Attendance.UserId;
        SubjectId = Attendance.SubjectId;
        IsAbsence = true;
        Sickness = Attendance.Sickness;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string PropertyName = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName)); }
}