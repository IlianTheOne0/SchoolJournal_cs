namespace Database.Repositories.Management;

using Database.Interfaces.Repositories.Management;
using Database.Interfaces.Repositories.Supabase;
using global::Supabase.Postgrest.Models;
using Models.Supports.SupabaseCommands;
using Models.Tables.Classes;
using Models.Tables.EducationalInstitutions;
using Models.Tables.Enrollments;
using Models.Tables.Statuses;
using Models.Tables.Users;
using System;
using System.Security.Cryptography;
using static global::Supabase.Postgrest.Constants;

public class RepositoriesManagement : InterfacesRepositoriesManagement
{
    private readonly InterfacesRepositoriesSupabase _repositorySupabase;

    public RepositoriesManagement(InterfacesRepositoriesSupabase RepositorySupabase) => _repositorySupabase = RepositorySupabase;

    public async Task<List<ModelsEducationalInstitutions>> GetAllEdu()
    {
        try { return await _repositorySupabase.GetAllAsync<ModelsEducationalInstitutions>(); }
        catch (Exception E) { throw new Exception($"Failed to get all classe by educational institution id: {E.Message}", E); }
    }

    public async Task<List<ModelsClasses>> GetAllClassesByEduId(int EduId)
    {
        try { return await _repositorySupabase.FilterAsync<ModelsClasses>("EducationalInstitutionId", Operator.Equals, EduId); }
        catch (Exception E) { throw new Exception($"Failed to get all classe by educational institution id: {E.Message}", E); }
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

    public async Task Add<TModel>(TModel Model)
        where TModel : BaseModel, InterfacesModelsWithId, new()
    {
        try { Model.Id = RandomNumberGenerator.GetInt32(1234567890); await _repositorySupabase.Insert(Model); }
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