namespace Database.Interfaces.Repositories.Management;

using global::Supabase.Postgrest.Models;
using Models.Supports.SupabaseCommands;
using Models.Tables.Classes;
using Models.Tables.EducationalInstitutions;
using Models.Tables.Users;

public interface InterfacesRepositoriesManagement
{
    Task<List<ModelsEducationalInstitutions>> GetAllEdu();
    Task<List<ModelsClasses>> GetAllClassesByEduId(int EduId);
    Task<List<ModelsUserExtended>> GetAllUsersByClassId(int ClassId);

    Task Add<TModel>(TModel Model)
        where TModel : BaseModel, InterfacesModelsWithId, new();
    Task Edit<TModel>(TModel Model, string[] ConflictColumns)
        where TModel : BaseModel, new ();
    Task Delete<TModel>(TModel Model)
        where TModel : BaseModel, InterfacesModelsWithId, new();
}