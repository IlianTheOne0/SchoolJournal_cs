namespace Contracts.Interfaces.Mediators.Users;

using Contracts.Models.Tables.Users;
using Contracts.Models.Tables.Users.Extended;

public interface InterfacesMediatorsUsers
{
    Task<ModelsUserExtended?> GetUserById(int userId);
    Task UpdateUser(ModelsUser user);
    Task UpdateAvatar(int userId, string filePath);
}