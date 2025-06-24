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
    Task<List<ModelsEnrollments>> GetEnrollmentsByClassAsync(int classId);
    Task<List<ModelsUser>> GetStudentsByIdsAsync(List<int> studentIds);
    Task<List<ModelsGrades>> GetGradesByStudentAsync(int studentId);
    Task<List<ModelsSubjects>> GetSubjectsByIdsAsync(List<int> subjectIds);
}