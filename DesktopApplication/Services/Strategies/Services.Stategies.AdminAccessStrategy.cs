namespace DesktopApplication.Services.Strategies.AdminAccess;

using DesktopApplication.Interfaces.AccessStrategy;
using Models.Tables.Users;

public class ServicesStrategiesAdminAccess : InterfacesAccessStrategy
{
    public ModelsUser ModelUser { get; set; }

    public ServicesStrategiesAdminAccess(ModelsUser ModelUser) { this.ModelUser = ModelUser; }

    public bool CanGrade() => false;
    public bool CanViewGrades() => false;
    public bool CanManageUsers() => true;
}