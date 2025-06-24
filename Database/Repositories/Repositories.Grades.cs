namespace Database.Repositories.Grades;

using Database.Interfaces.Repositories.Grade;
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
    public async Task<List<ModelsEnrollments>> GetEnrollmentsByClassAsync(int ClassId) => await _repositorySupabase.FilterAsync<ModelsEnrollments>("ClassId", Constants.Operator.Equals, ClassId);
    public async Task<List<ModelsUser>> GetStudentsByIdsAsync(List<int> StudentIds) => await _repositorySupabase.FilterAsync<ModelsUser>("Id", Constants.Operator.In, StudentIds);
    
    public async Task<List<ModelsGradesExtended>> GetGradesByStudentAsync(int studentId)
    {
        var gradesTableResult = await _repositorySupabase.FilterAsync<ModelsGrades>("UserId", Constants.Operator.Equals, studentId);

        List<ModelsGradesExtended>? result = new List<ModelsGradesExtended>();
        foreach (var gradeModel in gradesTableResult)
        {
            var subjectsTableResult = await _repositorySupabase.FilterAsync<ModelsSubjects>("Id", Constants.Operator.Equals, gradeModel.SubjectId);
            var subject = subjectsTableResult.FirstOrDefault();

            result.Add(new ModelsGradesExtended(gradeModel, subject?.Name ?? "Unknown Subject"));
        }

        return result;
    }

    public async Task<List<ModelsSubjects>> GetSubjectsByIdsAsync(List<int> SubjectIds) => await _repositorySupabase.FilterAsync<ModelsSubjects>("Id", Constants.Operator.In, SubjectIds);
}