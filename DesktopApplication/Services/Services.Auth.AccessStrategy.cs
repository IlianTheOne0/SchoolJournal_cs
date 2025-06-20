namespace DesktopApplication.Services.Auth;

using DesktopApplication.Interfaces.AccessStrategy;
using DesktopApplication.Services.Strategies.AdminAccess;
using DesktopApplication.Services.Strategies.StudentAccess;
using DesktopApplication.Services.Strategies.TeacherAccess;
using System.Windows;

public partial class ServicesAuth
{
    private InterfacesAccessStrategy? _interfacesAccessStrategy;
    public InterfacesAccessStrategy? AccessStrategy { get => _interfacesAccessStrategy; set => _interfacesAccessStrategy = value; }

    public void SetupAccessStrategy()
    {
        try
        {
            _interfacesAccessStrategy = RepositorySupabase.UserStatus switch
            {
                "Admin" => new ServicesStrategiesAdminAccess(),
                "Teacher" => new ServicesStrategiesTeacherAccess(),
                "Student" => new ServicesStrategiesStudentAccess(),
                _ => throw new Exception("SetupAccessStrategy failed: Unknown status")
            };
        }
        catch (Exception e) { throw new Exception($"SetupAccessStrategy failed: {e.Message}", e); }
    }
}