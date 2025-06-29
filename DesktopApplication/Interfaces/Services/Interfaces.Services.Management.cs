namespace DesktopApplication.Interfaces.Services.Management;

using global::Supabase.Postgrest.Models;
using Models.Supports.SupabaseCommands;
using Models.Tables.Classes;
using Models.Tables.EducationalInstitutions;
using Models.Tables.Users;

public interface InterfacesServicesManagement
{
    public List<ModelsEducationalInstitutions> AvailableEdu { get; }
    public List<ModelsClasses> AvailableClasses { get; }
    public List<ModelsUserExtended> AvailableUsers { get; }

    Task Load();

    Task<List<ModelsClasses>> GetAllClassesByEduId(int EduId);
    Task<List<ModelsUserExtended>> GetAllUsersByClassId(int ClassId);

    Task Add<TModel>(TModel Model)
        where TModel : BaseModel, InterfacesModelsWithId, new();
    Task Edit<TModel>(TModel Model, string[] ConflictColumns)
        where TModel : BaseModel, new();
    Task Delete<TModel>(TModel Model)
    where TModel : BaseModel, InterfacesModelsWithId, new();
}