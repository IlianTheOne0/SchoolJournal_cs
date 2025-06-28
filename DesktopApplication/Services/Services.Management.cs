namespace DesktopApplication.Services.Management;

using Database.Interfaces.Repositories.Management;
using Database.Repositories.Management;
using DesktopApplication.Interfaces.Services.Management;
using DesktopApplication.Interfaces.Services.Supabase;
using DesktopApplication.Interfaces.Services.User;
using Models.Tables.Classes;
using Models.Tables.Users;
using System;

public class ServicesManagement : InterfacesServicesManagement
{
    private readonly InterfacesRepositoriesManagement _repositoryManagement;
    // private readonly InterfacesServicesUser _serviceUser;

    public List<ModelsEducationalInstitutions> AvailableEdu { get; private set; } = new();
    public List<ModelsClasses> AvailableClasses { get; private set; } = new();
    public List<ModelsUserExtended> AvailableUsers { get; private set; } = new();

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
}