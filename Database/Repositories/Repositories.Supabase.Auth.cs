namespace Database.Repositories.Supabase;

using Database.Models.Tables.Users;
using Database.Models.Tables.Statuses;
using global::Supabase.Postgrest;
using System;
using System.Threading.Tasks;

public partial class RepositoriesSupabase
{
    public bool IsLoggedIn { get => SupabaseConnection?.SupabaseClient.Auth.CurrentSession != null; }
    public string? UserStatus { get; private set; }
    public int? UserId { get; private set; }

    public async Task Login(string Username, string Password)
    {
        try
        {
            var usersTableResult = await FilterAsync<ModelsUser>("Username", Constants.Operator.Equals, Username)
                ?? throw new Exception($"The username {Username} does not exist!");
            if (usersTableResult.Count == 0) { throw new Exception($"The username {Username} does not exist!"); }
            var currentUser = usersTableResult.FirstOrDefault();
            if (currentUser == null) { throw new Exception($"Could not retrieve user details for username: {Username}"); }

            await SupabaseConnection?.SupabaseClient.Auth.SignIn(currentUser?.Email!, Password)!;

            var statusesTableResult = await FilterAsync<ModelsStatuses>("Id", Constants.Operator.Equals, currentUser?.StatusId!)
                ?? throw new Exception($"The status id {currentUser?.StatusId!} does not exist!");
            if (statusesTableResult.Count == 0) { throw new Exception($"The status id {currentUser?.StatusId!} does not exist!"); }
            var status = statusesTableResult.FirstOrDefault();
            if (currentUser != null) { UserStatus = status?.Status; UserId = currentUser?.Id;  }
            else { throw new Exception($"Could not retrieve status details for status id: {currentUser?.StatusId}"); }
        }
        catch (Exception e) { throw new Exception($"Login failed: {e.Message}", e); }
    }

    public async Task Logout()
    {
        await SupabaseConnection?.SupabaseClient.Auth.SignOut()!;

        UserId = null;
        UserStatus = null;
    }
}