namespace Contracts.Interfaces.Mediators.Grades;

using Contracts.Models.Tables.Classes;
using Contracts.Models.Tables.Enrollments;
using Contracts.Models.Tables.Grades;
using Contracts.Models.Tables.Subjects;
using Contracts.Models.Tables.Users;

public interface InterfacesMediatorsGrades
{
    Task<List<ModelsClasses>> GetAllClassesAsync();
    Task<List<ModelsSubjects>> GetAllSubjectsAsync();
    Task<List<ModelsEnrollments>> GetEnrollmentsByClassAsync(int classId);
    Task<List<ModelsUser>> GetStudentsByIdsAsync(List<int> studentIds);
    Task<List<ModelsGrades>> GetGradesByStudentAsync(int studentId);
    Task<List<ModelsSubjects>> GetSubjectsByIdsAsync(List<int> subjectIds);
}