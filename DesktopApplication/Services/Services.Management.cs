namespace DesktopApplication.Services.Management;

using Database.Interfaces.Repositories.Management;
using Database.Repositories.Management;
using DesktopApplication.Interfaces.Services.Management;
using DesktopApplication.Interfaces.Services.Supabase;
using DesktopApplication.Interfaces.Services.User;
using global::Supabase.Postgrest.Models;
using Models.Supports.SupabaseCommands;
using Models.Tables.Classes;
using Models.Tables.EducationalInstitutions;
using Models.Tables.Statuses;
using Models.Tables.Subjects;
using Models.Tables.Users;
using System;

public class ServicesManagement : InterfacesServicesManagement
{
    private readonly InterfacesRepositoriesManagement _repositoryManagement;
    // private readonly InterfacesServicesUser _serviceUser;

    public List<ModelsEducationalInstitutions> AvailableEdu { get; private set; } = new();
    public List<ModelsClasses> AvailableClasses { get; private set; } = new();
    public List<ModelsUserExtended> AvailableUsers { get; private set; } = new();
    public List<ModelsSubjectsExtended> AvailableSubjects { get; private set; } = new();
    public List<ModelsUserExtended> AvailableTeachers { get; private set; } = new();
    public List<ModelsStatuses> AvailableStatuses { get; private set; } = new();

    public ServicesManagement(InterfacesServicesSupabase ServiceSupabase, InterfacesServicesUser ServiceUser)
    {
        _repositoryManagement = new RepositoriesManagement(ServiceSupabase.RepositorySupabase);
        // _serviceUser = ServiceUser;
    }

    public async Task Load()
    {
        try { AvailableEdu = await _repositoryManagement.GetAllEdu(); }
        catch (Exception E) { throw new Exception($"Loading failed: {E.Message}", E); }
    }

    public async Task<List<ModelsClasses>> GetAllClassesByEduId(int EduId)
    {
        try { AvailableClasses = await _repositoryManagement.GetAllClassesByEduId(EduId); return AvailableClasses; }
        catch (Exception E) { throw new Exception($"Getting class by education institutions failed: {E.Message}", E); }
    }

    public async Task<List<ModelsUserExtended>> GetAllUsersByClassId(int ClassId)
    {
        try { AvailableUsers = await _repositoryManagement.GetAllUsersByClassId(ClassId); return AvailableUsers; }
        catch (Exception E) { throw new Exception($"Getting users by class id failed: {E.Message}", E); }
    }

    public async Task<List<ModelsSubjectsExtended>> GetAllSubjectsByClassId(int ClassId)
    {
        try { AvailableSubjects = await _repositoryManagement.GetAllSubjectsByClassId(ClassId); return AvailableSubjects; }
        catch (Exception E) { throw new Exception($"Getting subjects by class id failed: {E.Message}", E); }
    }

    public async Task<List<ModelsUserExtended>> GetAllTeachersByEduId(int EduId)
    {
        try { AvailableTeachers = await _repositoryManagement.GetAllTeachersByEduId(EduId); return AvailableTeachers; }
        catch (Exception E) { throw new Exception($"Getting subjects by class id failed: {E.Message}", E); }
    }

    public async Task<List<ModelsStatuses>> GetAllStatuses()
    {
        try { AvailableStatuses = await _repositoryManagement.GetAllStatuses(); return AvailableStatuses; }
        catch (Exception E) { throw new Exception($"Getting subjects by class id failed: {E.Message}", E); }
    }

    public async Task<List<ModelsUserExtended>> GetAllUsersByEduId(int EduId)
    {
        try { AvailableUsers = await _repositoryManagement.GetAllUsersByEduId(EduId); return AvailableUsers; }
        catch (Exception E) { throw new Exception($"Getting subjects by class id failed: {E.Message}", E); }
    }

    public async Task Add<TModel>(TModel Model)
        where TModel : BaseModel, InterfacesModelsWithId, new()
    {
        try { await _repositoryManagement.Add(Model); }
        catch (Exception E) { throw new Exception($"Adding the new educational institution failed: {E.Message}", E); }
    }

    public async Task AddUser(ModelsUser User, string Password, int ClassId)
    {
        try { await _repositoryManagement.AddUser(User, Password, ClassId); }
        catch (Exception E) { throw new Exception($"Adding the new user failed: {E.Message}", E); }
    }

    public async Task Edit<TModel>(TModel Model, string[] ConflictColumns)
        where TModel : BaseModel, new()
    {
        try { await _repositoryManagement.Edit(Model, ConflictColumns); }
        catch (Exception E) { throw new Exception($"Edit the new name of the educational institution failed: {E.Message}", E); }
    }

    public async Task Delete<TModel>(TModel Model)
        where TModel : BaseModel, InterfacesModelsWithId, new()
    {
        try { await _repositoryManagement.Delete(Model); }
        catch (Exception E) { throw new Exception($"Deleting the educational institution failed: {E.Message}", E); }
    }
}