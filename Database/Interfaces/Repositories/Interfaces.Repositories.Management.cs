namespace Database.Interfaces.Repositories.Management;

using Models.Tables.Classes;
using Models.Tables.Users;

public interface InterfacesRepositoriesManagement
{
    Task<List<ModelsEducationalInstitutions>> GetAllEdu();
    Task<List<ModelsClasses>> GetAllClassesByEduId(int EduId);
    Task<List<ModelsUserExtended>> GetAllUsersByClassId(int ClassId);
}