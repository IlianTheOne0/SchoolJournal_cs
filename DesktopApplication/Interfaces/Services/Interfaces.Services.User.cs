namespace DesktopApplication.Interfaces.Services.User;

using DesktopApplication.Interfaces.Services.Strategies.AccessStrategy;
using Models.Tables.Users;

public interface InterfacesServicesUser
{
    InterfacesAccessStrategy? AccessStrategy { get; set; }

    Task<ModelsUserExtended?> GetUserById(int UserId);
    Task UpdateUser(ModelsUser User);
    void SetupAccessStrategy(ModelsUserExtended ModelUser);
    Task UpdateAvatar(int UserId, string FilePath);
    void ClearAccessStrategy();
    Task RefreshTheData();
}