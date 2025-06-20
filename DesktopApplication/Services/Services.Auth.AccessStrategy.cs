namespace DesktopApplication.Services.Auth;

using DesktopApplication.Interfaces.AccessStrategy;
using DesktopApplication.Services.Strategies.AdminAccess;
using DesktopApplication.Services.Strategies.StudentAccess;
using DesktopApplication.Services.Strategies.TeacherAccess;
using Models.Tables.Users;

public partial class ServicesAuth
{
    private InterfacesAccessStrategy? _interfacesAccessStrategy;
    public InterfacesAccessStrategy? AccessStrategy { get => _interfacesAccessStrategy; set => _interfacesAccessStrategy = value; }

    public void SetupAccessStrategy()
    {
        try
        {
            ModelsUser modelUser = _repositorySupabase.ModelUser!;
            if (modelUser == null) { throw new Exception("SetupAccessStrategy failed: The model of user is empty!"); }

            _interfacesAccessStrategy = modelUser.StatusName switch
            {
                "Admin" => new ServicesStrategiesAdminAccess(modelUser),
                "Teacher" => new ServicesStrategiesTeacherAccess(modelUser),
                "Student" => new ServicesStrategiesStudentAccess(modelUser),
                _ => throw new Exception("SetupAccessStrategy failed: Unknown status")
            };
        }
        catch (Exception e) { throw new Exception($"SetupAccessStrategy failed: {e.Message}", e); }
    }
}