namespace Models.Tables.Subjects;

using System.Runtime.CompilerServices;
using System.ComponentModel;

public class ModelsSubjectsExtended : ModelsSubjects, INotifyPropertyChanged
{
    private string _teacherName; public string TeacherName { get => _teacherName; set { _teacherName = value; OnPropertyChanged(); } }

    public override string Name { get => base.Name; set { base.Name = value; OnPropertyChanged(); } }

    public ModelsSubjectsExtended() { }

    public ModelsSubjectsExtended(ModelsSubjects Subjects, string TeacherName)
    {
        Id = Subjects.Id;
        Name = Subjects.Name;
        TeacherId = Subjects.TeacherId;
        ClassId = Subjects.ClassId;
        
        this.TeacherName = TeacherName;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public ModelsSubjects ToBase()
    {
        return new ModelsSubjects
        {
            Id = this.Id,
            Name = this.Name,
            TeacherId = this.TeacherId,
            ClassId = this.ClassId
        };
    }

    private void OnPropertyChanged([CallerMemberName] string PropertyName = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName)); }
}