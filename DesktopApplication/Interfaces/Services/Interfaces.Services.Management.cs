namespace DesktopApplication.Interfaces.Services.Management;

using Models.Tables.Classes;
using Models.Tables.Users;

public interface InterfacesServicesManagement
{
    public List<ModelsEducationalInstitutions> AvailableEdu { get; }
    public List<ModelsClasses> AvailableClasses { get; }
    public List<ModelsUserExtended> AvailableUsers { get; }

    Task Load();

    Task<List<ModelsClasses>> GetAllClassesByEduId(int EduId);
    Task<List<ModelsUserExtended>> GetAllUsersByClassId(int ClassId);
}