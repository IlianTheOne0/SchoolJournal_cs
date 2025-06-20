namespace DesktopApplication.Services.Strategies.TeacherAccess;

using DesktopApplication.Interfaces.AccessStrategy;

public class ServicesStrategiesTeacherAccess : InterfacesAccessStrategy
{
    public bool CanGrade() => true;
    public bool CanManageUsers() => false;
}