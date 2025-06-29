namespace Database.Repositories.Management;

using Database.Interfaces.Repositories.Management;
using Database.Interfaces.Repositories.Supabase;
using global::Supabase.Postgrest.Models;
using Models.Supports.SupabaseCommands;
using Models.Tables.Classes;
using Models.Tables.EducationalInstitutions;
using Models.Tables.Enrollments;
using Models.Tables.Statuses;
using Models.Tables.Subjects;
using Models.Tables.Users;
using System;
using System.Reflection;
using System.Security.Cryptography;
using static global::Supabase.Postgrest.Constants;

public class RepositoriesManagement : InterfacesRepositoriesManagement
{
    private readonly InterfacesRepositoriesSupabase _repositorySupabase;

    public RepositoriesManagement(InterfacesRepositoriesSupabase RepositorySupabase) => _repositorySupabase = RepositorySupabase;

    public async Task<List<ModelsEducationalInstitutions>> GetAllEdu()
    {
        try { return await _repositorySupabase.GetAllAsync<ModelsEducationalInstitutions>(); }
        catch (Exception E) { throw new Exception($"Failed to get all educational institutions: {E.Message}", E); }
    }

    public async Task<List<ModelsClasses>> GetAllClassesByEduId(int EduId)
    {
        try { return await _repositorySupabase.FilterAsync<ModelsClasses>("EducationalInstitutionId", Operator.Equals, EduId); }
        catch (Exception E) { throw new Exception($"Failed to get all classes by educational institution id: {E.Message}", E); }
    }

    public async Task<List<ModelsUserExtended>> GetAllUsersByClassId(int ClassId)
    {
        try
        {
            var enrollments = await _repositorySupabase.FilterAsync<ModelsEnrollments>("ClassId", Operator.Equals, ClassId);
            if (enrollments == null || enrollments.Count == 0) { return new List<ModelsUserExtended>(); }

            var studentIds = enrollments.Select(enrollmentsProvider => enrollmentsProvider.UserId).Distinct().ToList();
            var allStatuses = await _repositorySupabase.GetAllAsync<ModelsStatuses>();
            var allInstitutions = await _repositorySupabase.GetAllAsync<ModelsEducationalInstitutions>();

            var students = await _repositorySupabase.FilterAsync<ModelsUser>("Id", Operator.In, studentIds);

            var result = students.Select(
                userProvider =>
                {
                    var status = allStatuses.FirstOrDefault(statusProvider => statusProvider.Id == userProvider.StatusId)?.Status ?? "Unknown";
                    var institution = allInstitutions.FirstOrDefault(eduProvider => eduProvider.Id == userProvider.EducationalInstitutionId)?.Name ?? "Unknown";
                    return new ModelsUserExtended(userProvider, status, institution);
                }
            ).ToList();

            return result;
        }
        catch (Exception E) { throw new Exception($"Failed to get users by class ID: {E.Message}", E); }
    }

    public async Task<List<ModelsSubjectsExtended>> GetAllSubjectsByClassId(int ClassId)
    {
        try
        {
            var subjects = await _repositorySupabase.FilterAsync<ModelsSubjects>("ClassId", Operator.Equals, ClassId);
            if (subjects.Count == 0) { return new(); }

            var teacherIds = subjects.Select(subjectsProvider => subjectsProvider.TeacherId).Distinct().ToList();

            var teachers = await _repositorySupabase.FilterAsync<ModelsUser>(
                teacherIds.Select(id => ("Id", Operator.Equals, (object)id))
            );

            var result = new List<ModelsSubjectsExtended>();
            foreach (var subject in subjects)
            {
                var teacher = teachers.FirstOrDefault(teachersProvider => teachersProvider.Id == subject.TeacherId);
                var teacherName = teacher != null ? teacher.FullName : "Unknown";

                result.Add(new ModelsSubjectsExtended(subject, teacherName));
            }

            return result;
        }
        catch (Exception E) { throw new Exception($"Failed to get all subjects by class id: {E.Message}", E); }
    }

    public async Task<List<ModelsUserExtended>> GetAllTeachersByEduId(int EduId)
    {
        try
        {
            var users = await _repositorySupabase.FilterAsync<ModelsUser>("EducationalInstitutionId", Operator.Equals, EduId);
            if (users.Count == 0) { return new(); }

            var teachersIds = users.Select(teachersProvider => teachersProvider.StatusId = 2).Distinct().ToList();
            var allStatuses = await _repositorySupabase.GetAllAsync<ModelsStatuses>();
            var allInstitutions = await _repositorySupabase.GetAllAsync<ModelsEducationalInstitutions>();

            var students = await _repositorySupabase.FilterAsync<ModelsUser>("Id", Operator.In, teachersIds);

            var result = students.Select(
                userProvider =>
                {
                    var status = allStatuses.FirstOrDefault(statusProvider => statusProvider.Id == userProvider.StatusId)?.Status ?? "Unknown";
                    var institution = allInstitutions.FirstOrDefault(eduProvider => eduProvider.Id == userProvider.EducationalInstitutionId)?.Name ?? "Unknown";
                    return new ModelsUserExtended(userProvider, status, institution);
                }
            ).ToList();

            return result;
        }
        catch (Exception E) { throw new Exception($"Failed to get all teachers by educational institution id: {E.Message}", E); }
    }

    public async Task<List<ModelsStatuses>> GetAllStatuses()
    {
        try { return await _repositorySupabase.GetAllAsync<ModelsStatuses>(); }
        catch (Exception E) { throw new Exception($"Failed to get all statuses: {E.Message}", E); }
    }

    public async Task Add<TModel>(TModel Model)
        where TModel : BaseModel, InterfacesModelsWithId, new()
    {
        try { Model.Id = RandomNumberGenerator.GetInt32(1234567890); await _repositorySupabase.Insert(Model); }
        catch (Exception E) { throw new Exception($"Failed to add: {E.Message}", E); }
    }

    public async Task AddUser(ModelsUser User, string Password, int ClassId)
    {
        try {
            await _repositorySupabase.AddUser(User, Password);
            if (User.StatusId == 3) { await _repositorySupabase.Insert(new ModelsEnrollments { UserId = User.Id, ClassId = ClassId }); }
        }
        catch (Exception E) { throw new Exception($"Failed to add: {E.Message}", E); }
    }

    public async Task Edit<TModel>(TModel Model, string[] ConflictColumns)
        where TModel : BaseModel, new()
    {
        try { await _repositorySupabase.Upsert(Model, ConflictColumns); }
        catch (Exception E) { throw new Exception($"Failed to edit: {E.Message}", E); }
    }

    public async Task Delete<TModel>(TModel Model)
        where TModel : BaseModel, InterfacesModelsWithId, new()
    {
        try { await _repositorySupabase.Delete(Model); }
        catch (Exception E) { throw new Exception($"Failed to delete: {E.Message}", E); }
    }
}