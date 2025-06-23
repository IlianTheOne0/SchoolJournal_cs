namespace DesktopApplication.Services;

using Database.Repositories.Supabase;
using DesktopApplication.Interfaces.AccessStrategy;
using DesktopApplication.Services.Strategies.AdminAccess;
using DesktopApplication.Services.Strategies.StudentAccess;
using DesktopApplication.Services.Strategies.TeacherAccess;
using Models.Tables.Classes;
using Models.Tables.Statuses;
using Models.Tables.Users;
using Models.Tables.Users.Extended;
using global::Supabase.Postgrest;
using System.Threading.Tasks;

public class ServicesUser
{
    private readonly RepositoriesSupabase _repositorySupabase;
    private InterfacesAccessStrategy? _accessStrategy; public InterfacesAccessStrategy? AccessStrategy { get => _accessStrategy; private set => _accessStrategy = value; }

    public ServicesUser(RepositoriesSupabase RepositorySupabase) => _repositorySupabase = RepositorySupabase;

    public async Task<ModelsUserExtended?> GetUserById(int UserId)
    {
        try
        {
            var user = await _repositorySupabase.GetUserById(UserId);
            if (user == null) return null;

            var status = (await _repositorySupabase.FilterAsync<ModelsStatuses>("Id", Constants.Operator.Equals, user.StatusId))?.FirstOrDefault();
            var institution = (await _repositorySupabase.FilterAsync<ModelsEducationalInstitutions>("Id", Constants.Operator.Equals, user.EducationalInstitutionId))?.FirstOrDefault();

            return new ModelsUserExtended(user, status?.Status!, institution?.Name!);
        }
        catch (Exception e) { throw new Exception($"Failed to get user by ID: {e.Message}", e); }
    }

    public async Task UpdateUser(ModelsUser User) => await _repositorySupabase.UpdateUser(User);

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
            await _repositorySupabase.UpdateAvatar(UserId, FilePath);
            if (AccessStrategy != null) { AccessStrategy.ModelUser = await GetUserById(UserId); }
        }
        catch (Exception e) { throw new Exception($"Update avatar failed: {e.Message}", e); }
    }
    public void ClearAccessStrategy() => _accessStrategy = null;
    public async void RefreshTheData() => AccessStrategy.ModelUser = await GetUserById(AccessStrategy.ModelUser.Id)!;
}