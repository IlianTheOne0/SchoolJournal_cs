namespace DesktopApplication.Services;

using DesktopApplication.Interfaces.AccessStrategy;
using DesktopApplication.Services.Strategies.AdminAccess;
using DesktopApplication.Services.Strategies.StudentAccess;
using DesktopApplication.Services.Strategies.TeacherAccess;
using Database.Repositories.Supabase;
using Models.Tables.Users;
using System.Threading.Tasks;

public class ServicesUser
{
    private readonly RepositoriesSupabase _repositorySupabase;
    private InterfacesAccessStrategy? _accessStrategy; public InterfacesAccessStrategy? AccessStrategy { get => _accessStrategy; private set => _accessStrategy = value; }

    public ServicesUser(RepositoriesSupabase RepositorySupabase) => _repositorySupabase = RepositorySupabase;

    public async Task<ModelsUser?> GetUserById(int userId) => await _repositorySupabase.GetUserById(userId);
    public async Task UpdateUser(ModelsUser user) => await _repositorySupabase.UpdateUser(user);

    public void SetupAccessStrategy(ModelsUser modelUser)
    {
        try
        {
            if (modelUser == null) { throw new Exception("SetupAccessStrategy failed: The model of user is empty!"); }

            _accessStrategy = modelUser.StatusName switch
            {
                "Admin" => new ServicesStrategiesAdminAccess(modelUser),
                "Teacher" => new ServicesStrategiesTeacherAccess(modelUser),
                "Student" => new ServicesStrategiesStudentAccess(modelUser),
                _ => throw new Exception("SetupAccessStrategy failed: Unknown status")
            };
        }
        catch (Exception e) { throw new Exception($"SetupAccessStrategy failed: {e.Message}", e); }
    }

    public void ClearAccessStrategy() => _accessStrategy = null;
}