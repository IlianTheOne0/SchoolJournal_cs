namespace Database.Repositories.Grades;

using Database.Interfaces.Repositories.Grades;
using Database.Interfaces.Repositories.Supabase;
using Database.Repositories.Supabase;
using Models.Tables.Classes;
using Models.Tables.Enrollments;
using Models.Tables.Grades;
using Models.Tables.Subjects;
using Models.Tables.Users;
using global::Supabase.Postgrest;

public class RepositoriesGrades : InterfacesRepositoriesGrades 
{
    private readonly InterfacesRepositoriesSupabase _repositorySupabase;

    public RepositoriesGrades(RepositoriesSupabase repository) => _repositorySupabase = repository;

    public async Task<List<ModelsClasses>> GetAllClassesAsync() => await _repositorySupabase.GetAllAsync<ModelsClasses>();
    public async Task<List<ModelsSubjects>> GetAllSubjectsAsync() => await _repositorySupabase.GetAllAsync<ModelsSubjects>();
    public async Task<List<ModelsEnrollments>> GetEnrollmentsByClassAsync(int classId) => await _repositorySupabase.FilterAsync<ModelsEnrollments>("ClassId", Constants.Operator.Equals, classId);
    public async Task<List<ModelsUser>> GetStudentsByIdsAsync(List<int> studentIds) => await _repositorySupabase.FilterAsync<ModelsUser>("Id", Constants.Operator.In, studentIds);
    public async Task<List<ModelsGrades>> GetGradesByStudentAsync(int studentId) => await _repositorySupabase.FilterAsync<ModelsGrades>("UserId", Constants.Operator.Equals, studentId);
    public async Task<List<ModelsSubjects>> GetSubjectsByIdsAsync(List<int> subjectIds) => await _repositorySupabase.FilterAsync<ModelsSubjects>("Id", Constants.Operator.In, subjectIds);
}