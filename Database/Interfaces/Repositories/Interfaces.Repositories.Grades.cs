namespace Database.Interfaces.Repositories.Grade;

using Models.Tables.Classes;
using Models.Tables.Enrollments;
using Models.Tables.Grades;
using Models.Tables.Subjects;
using Models.Tables.Users;

public interface InterfacesRepositoriesGrades
{
    Task<List<ModelsClasses>> GetAllClassesAsync();
    Task<List<ModelsSubjects>> GetAllSubjectsAsync();
    Task<List<ModelsEnrollments>> GetEnrollmentsByClassAsync(int ClassId);
    Task<List<ModelsUser>> GetStudentsByIdsAsync(List<int> StudentIds);
    Task<List<ModelsGradesExtended>> GetGradesByStudentAsync(int StudentId);
    Task<List<ModelsSubjects>> GetSubjectsByIdsAsync(List<int> SubjectIds);
}