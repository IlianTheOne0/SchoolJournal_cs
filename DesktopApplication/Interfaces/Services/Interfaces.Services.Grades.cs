namespace DesktopApplication.Interfaces.Services.Grades;

using Models.Tables.Classes;
using Models.Tables.Grades;
using Models.Tables.Subjects;
using Models.Tables.Users;
using System.Threading.Tasks;

public interface InterfacesServicesGrades
{
    event Action PropertyChanged;
    bool IsTeacherMode { get; }
    bool HasClassSelected { get; }

    List<ModelsClasses> AvailableClasses { get; }
    ModelsClasses SelectedClass { get; set; }
    List<ModelsUser> StudentsInClass { get; }
    ModelsUser SelectedStudent { get; set; }
    List<ModelsSubjects> AvailableSubjects { get; }
    ModelsSubjects SelectedSubject { get; set; }
    List<ModelsGradesExtended> FilteredGrades { get; }

    Task Initialize();
}