namespace DesktopApplication.Interfaces.Services.Management;

using global::Supabase.Postgrest.Models;
using Models.Supports.SupabaseCommands;
using Models.Tables.Classes;
using Models.Tables.EducationalInstitutions;
using Models.Tables.Statuses;
using Models.Tables.Subjects;
using Models.Tables.Users;

public interface InterfacesServicesManagement
{
    List<ModelsEducationalInstitutions> AvailableEdu { get; }
    List<ModelsClasses> AvailableClasses { get; }
    List<ModelsUserExtended> AvailableUsers { get; }
    List<ModelsSubjectsExtended> AvailableSubjects { get; }
    List<ModelsUserExtended> AvailableTeachers { get; }
    List<ModelsStatuses> AvailableStatuses { get; }

    Task Load();

    Task<List<ModelsClasses>> GetAllClassesByEduId(int EduId);
    Task<List<ModelsUserExtended>> GetAllUsersByClassId(int ClassId);
    Task<List<ModelsSubjectsExtended>> GetAllSubjectsByClassId(int ClassId);
    Task<List<ModelsUserExtended>> GetAllTeachersByEduId(int EduId);
    Task<List<ModelsStatuses>> GetAllStatuses();
    Task<List<ModelsUserExtended>> GetAllUsersByEduId(int EduId);

    Task Add<TModel>(TModel Model)
        where TModel : BaseModel, InterfacesModelsWithId, new();
    Task AddUser(ModelsUser User, string Password, int ClassId);
    Task Edit<TModel>(TModel Model, string[] ConflictColumns)
        where TModel : BaseModel, new();
    Task Delete<TModel>(TModel Model)
    where TModel : BaseModel, InterfacesModelsWithId, new();
}