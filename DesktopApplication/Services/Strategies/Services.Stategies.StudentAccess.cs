namespace DesktopApplication.Services.Strategies.StudentAccess;

using DesktopApplication.Interfaces.AccessStrategy;

public class ServicesStrategiesStudentAccess : InterfacesAccessStrategy
{
    public bool CanGrade() => false;
    public bool CanManageUsers() => false;
}