namespace Models.Tables.Grades;

using System.Runtime.CompilerServices;
using System.ComponentModel;

public class ModelsGradesExtended : ModelsGrades, INotifyPropertyChanged
{
    private string _subjectName; public string SubjectName { get => _subjectName; set { _subjectName = value; OnPropertyChanged(); } }
    public ModelsGradesExtended() { }

    public ModelsGradesExtended(ModelsGrades Subject, string SubjectName)
    {
        this.SubjectName = SubjectName;
        Id = Subject.Id;
        Grade = Subject.Grade;
        Description = Subject.Description;
        Date = Subject.Date;
        UserId = Subject.UserId;
        SubjectId = Subject.SubjectId;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
}