namespace Database.Interfaces.Repositories.Grades;

using Models.Tables.Attending;
using Models.Tables.Classes;
using Models.Tables.Grades;
using Models.Tables.Subjects;
using Models.Tables.Users;
using static global::Supabase.Postgrest.Constants;

public interface InterfacesRepositoriesGrades
{
    Task<List<ModelsClasses>> GetAllClassesByEducationalInstitution(int EducationalInstitutionId);
    Task<List<ModelsSubjects>> GetAllSubjectsByEducationalInstitution(bool IsTeacher, int EducationalInstitutionId);
    Task<List<ModelsUserExtended>> GetAllStudentsByClassId(int ClassId);
    Task<List<ModelsGradesExtended>> GetAllGradesByStudentId(int ClassId);
    Task<List<ModelsGradesExtended>> GetAllGradesBySubjectAndStudent(int SubjectId, int StudentId);

    Task<List<ModelsSubjects>> GetAllSubjectsByClass(int ClassId);
    Task<List<ModelsGrades>> GetExistingGrades(int StudentId);
    Task Insert(ModelsGrades Item);
    Task Update(ModelsGrades Item);
    Task DeleteGrade(int StudentId, int SubjectId, DateTime Date);

    Task DeleteAttendance(int StudentId, int SubjectId, DateTime Date);
    Task InsertAttendance(ModelsAttending Attendance);
    Task<List<ModelsAttending>> GetAttendanceByClass(int ClassId, int SubjectId, int Month, int Year);
}