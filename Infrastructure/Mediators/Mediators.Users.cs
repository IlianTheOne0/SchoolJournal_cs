namespace Infrastructure.Mediators.Users;

using Contracts.Interfaces.Mediators.Users;
using Database.Interfaces.Repositories.Supabase;
using Database.Repositories.Supabase;
using Contracts.Models.Tables.Classes;
using Contracts.Models.Tables.Statuses;
using Contracts.Models.Tables.Users;
using Contracts.Models.Tables.Users.Extended;
using global::Supabase.Postgrest;

public class MediatorsUsers : InterfacesMediatorsUsers
{
    private readonly InterfacesRepositoriesSupabase _repositorySupabase;

    public MediatorsUsers(RepositoriesSupabase RepositorySupabase) => _repositorySupabase = RepositorySupabase;

    public async Task<ModelsUserExtended?> GetUserById(int userId)
    {
        var user = await _repositorySupabase.GetUserById(userId);
        if (user == null) { return null; }

        var status = (await _repositorySupabase.FilterAsync<ModelsStatuses>("Id", Constants.Operator.Equals, user.StatusId))?.FirstOrDefault();
        var institution = (await _repositorySupabase.FilterAsync<ModelsEducationalInstitutions>("Id", Constants.Operator.Equals, user.EducationalInstitutionId))?.FirstOrDefault();

        return new ModelsUserExtended(user, status?.Status!, institution?.Name!);
    }

    public Task UpdateUser(ModelsUser user) => _repositorySupabase.UpdateUser(user);
    public Task UpdateAvatar(int userId, string filePath) => _repositorySupabase.UpdateAvatar(userId, filePath);
}