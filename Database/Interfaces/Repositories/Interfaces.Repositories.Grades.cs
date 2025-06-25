namespace Database.Interfaces.Repositories.Grades;

using Models.Tables.Classes;
using Models.Tables.Grades;
using Models.Tables.Subjects;
using Models.Tables.Users;

public interface InterfacesRepositoriesGrades
{
    Task<List<ModelsClasses>> GetAllClassesByEducationalInstitution(int EducationalInstitutionId);
    Task<List<ModelsSubjects>> GetAllSubjectsByEducationalInstitution(bool IsTeacher, int EducationalInstitutionId);
    Task<List<ModelsUserExtended>> GetAllStudentsByClassId(int ClassId);
    Task<List<ModelsGradesExtended>> GetAllGradesByStudentId(int ClassId);
    Task<List<ModelsGradesExtended>> GetAllGradesBySubjectAndStudent(int SubjectId, int StudentId);
}