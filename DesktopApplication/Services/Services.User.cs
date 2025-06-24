namespace DesktopApplication.Services;

using Infrastructure.Interfaces.Mediators.Users;
using DesktopApplication.Interfaces.Services.Strategies.AccessStrategy;
using DesktopApplication.Interfaces.Services.User;
using DesktopApplication.Services.Strategies.AdminAccess;
using DesktopApplication.Services.Strategies.StudentAccess;
using DesktopApplication.Services.Strategies.TeacherAccess;
using Infrastructure.Models.Tables.Users;
using Infrastructure.Models.Tables.Users.Extended;
using System.Threading.Tasks;

public class ServicesUser : InterfacesServicesUser
{
    private readonly InterfacesMediatorsUsers _mediatorUser;

    private InterfacesAccessStrategy? _accessStrategy;
    InterfacesAccessStrategy? InterfacesServicesUser.AccessStrategy { get => _accessStrategy; set => AccessStrategy = value; }
    public InterfacesAccessStrategy? AccessStrategy { get => _accessStrategy; private set => _accessStrategy = value; }

    public ServicesUser(InterfacesMediatorsUsers UserMediator) => _mediatorUser = UserMediator;

    public async Task<ModelsUserExtended?> GetUserById(int UserId)
    {
        try { return await _mediatorUser.GetUserById(UserId); }
        catch (Exception e) { throw new Exception($"Failed to get user by ID: {e.Message}", e); }
    }

    public async Task UpdateUser(ModelsUser User)
    {
        try
        {
            await _mediatorUser.UpdateUser(User);
            if (AccessStrategy != null) { AccessStrategy.ModelUser = await GetUserById(User.Id); }
        }
        catch (Exception e) { throw new Exception($"Failed to update user: {e.Message}", e); }
    }

    public void SetupAccessStrategy(ModelsUserExtended ModelUser)
    {
        try
        {
            if (ModelUser == null) { throw new Exception("SetupAccessStrategy failed: The model of user is empty!"); }

            _accessStrategy = ModelUser.StatusName switch
            {
                "Admin" => new ServicesStrategiesAdminAccess(ModelUser),
                "Teacher" => new ServicesStrategiesTeacherAccess(ModelUser),
                "Student" => new ServicesStrategiesStudentAccess(ModelUser),
                _ => throw new Exception("SetupAccessStrategy failed: Unknown status")
            };
        }
        catch (Exception e) { throw new Exception($"SetupAccessStrategy failed: {e.Message}", e); }
    }

    public async Task UpdateAvatar(int UserId, string FilePath)
    {
        try {
            await _mediatorUser.UpdateAvatar(UserId, FilePath);
            if (AccessStrategy != null) { AccessStrategy.ModelUser = await GetUserById(UserId); }
        }
        catch (Exception e) { throw new Exception($"Update avatar failed: {e.Message}", e); }
    }
    public void ClearAccessStrategy() => _accessStrategy = null;
    public async Task RefreshTheData()
    {
        if (AccessStrategy != null) { AccessStrategy.ModelUser = await GetUserById(AccessStrategy.ModelUser.Id); }
    }
}