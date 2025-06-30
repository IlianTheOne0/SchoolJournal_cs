namespace Database.Repositories.Supabase;

using global::Supabase.Postgrest;
using Models.Tables.Classes;
using Models.Tables.EducationalInstitutions;
using Models.Tables.Statuses;
using Models.Tables.Users;
using System;
using System.Threading.Tasks;

public partial class RepositoriesSupabase
{
    public bool IsLoggedIn { get => SupabaseConnection?.SupabaseClient.Auth.CurrentSession != null; }
    public ModelsUserExtended? ModelUser { get; private set; } = null;

    public async Task Login(string Username, string Password)
    {
        try
        {
            var usersTableResult = await FilterAsync<ModelsUser>("Username", Constants.Operator.Equals, Username)
                ?? throw new Exception($"The username {Username} does not exist!");
            if (usersTableResult.Count == 0) { throw new Exception($"The username {Username} does not exist!"); }
            var user = usersTableResult.FirstOrDefault()
                ?? throw new Exception($"Could not retrieve user details for username: {Username}");

            await SupabaseConnection?.SupabaseClient.Auth.SignIn(user.Email, Password)!;

            var statusesTableResult = await FilterAsync<ModelsStatuses>("Id", Constants.Operator.Equals, user.StatusId)
                ?? throw new Exception($"The status id {user.StatusId} does not exist!");
            if (statusesTableResult.Count == 0) { throw new Exception($"The status id {user.StatusId} does not exist!"); }
            var status = statusesTableResult.FirstOrDefault()
                ?? throw new Exception($"Could not retrieve status details for status id: {user.StatusId}");

            var educationalInstitutionsTableResult = await FilterAsync<ModelsEducationalInstitutions>("Id", Constants.Operator.Equals, user.EducationalInstitutionId)
                ?? throw new Exception($"The educational institution id {user.EducationalInstitutionId} does not exist!");
            if (educationalInstitutionsTableResult.Count == 0) { throw new Exception($"The educational institution id {user.EducationalInstitutionId} does not exist!"); }
            var educationalInstitution = educationalInstitutionsTableResult.FirstOrDefault()
                ?? throw new Exception($"Could not retrieve educational institution details for educational institution id: {user.EducationalInstitutionId}");

            user.DateOfTheLastVisitToTheJournal = DateTime.Now;
            await UpdateUser(user);

            ModelUser = new ModelsUserExtended(user, status.Status, educationalInstitution.Name);
        }
        catch (Exception e) { throw new Exception($"Login failed: {e.Message}", e); }
    }

    public async Task Logout()
    {
        await SupabaseConnection?.SupabaseClient.Auth.SignOut()!;

        ModelUser = null;
    }
}