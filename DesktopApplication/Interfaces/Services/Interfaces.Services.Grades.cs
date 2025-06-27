namespace DesktopApplication.Interfaces.Services.Grades;

using Models.Supports.GradesAssigner;
using Models.Tables.Classes;
using Models.Tables.Grades;
using Models.Tables.Subjects;
using Models.Tables.Users;

using System.Threading.Tasks;

public interface InterfacesServicesGrades
{
    int CurrentUserId { get; }

    List<ModelsClasses> AvailableClasses { get; }
    List<ModelsUserExtended> AvailableStudents { get; }
    List<ModelsSubjects> AvailableSubjects { get; }
    List<ModelsGradesExtended> AvailableGrades { get;  }

    bool GetIsTeacherMode();
    Task Initialize();
    Task Refresh();

    Task<List<ModelsUserExtended>> GetAllStudentsByClassId(int ClassId);
    Task<List<ModelsGradesExtended>> GetAllGradesByStudentId(int ClassId);
    Task<List<ModelsGradesExtended>> GetAllGradesBySubjectAndStudent(int SubjectId, int StudentId);

    Task<List<ModelsClasses>> GetAvailableClasses();
    Task<List<ModelsSubjects>> GetSubjectsByClass(int ClassId);
    Task<List<StudentGradeAssignment>> GetStudentAssignments(int ClassId, int SubjectId, int Month, int Year);
    Task UpdateGrade(int StudentId, int SubjectId, DateTime Date, int GradeValue, string Description);
}