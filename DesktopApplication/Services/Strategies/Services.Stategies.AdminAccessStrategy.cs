namespace DesktopApplication.Services.Strategies.AdminAccess;

using DesktopApplication.Interfaces.AccessStrategy;

public class ServicesStrategiesAdminAccess : InterfacesAccessStrategy
{
    public bool CanGrade() => false;
    public bool CanManageUsers() => true;
}