namespace Infrastructure.Mediators.Grades;

using Database.Interfaces.Repositories.Grades;
using Database.Repositories.Grades;
using Contracts.Interfaces.Mediators.Grades;
using Models.Tables.Classes;
using Models.Tables.Enrollments;
using Models.Tables.Grades;
using Models.Tables.Subjects;
using Models.Tables.Users;

public class MediatorsGrades : InterfacesMediatorsGrades
{
    private readonly InterfacesRepositoriesGrades _repositoryGrades;

    public MediatorsGrades(RepositoriesGrades RepositoryGrades) => _repositoryGrades = RepositoryGrades;

    public Task<List<ModelsClasses>> GetAllClassesAsync() => _repositoryGrades.GetAllClassesAsync();
    public Task<List<ModelsSubjects>> GetAllSubjectsAsync() => _repositoryGrades.GetAllSubjectsAsync();
    public Task<List<ModelsEnrollments>> GetEnrollmentsByClassAsync(int classId) => _repositoryGrades.GetEnrollmentsByClassAsync(classId);
    public Task<List<ModelsUser>> GetStudentsByIdsAsync(List<int> studentIds) => _repositoryGrades.GetStudentsByIdsAsync(studentIds);
    public Task<List<ModelsGrades>> GetGradesByStudentAsync(int studentId) => _repositoryGrades.GetGradesByStudentAsync(studentId);
    public Task<List<ModelsSubjects>> GetSubjectsByIdsAsync(List<int> subjectIds) => _repositoryGrades.GetSubjectsByIdsAsync(subjectIds);
}