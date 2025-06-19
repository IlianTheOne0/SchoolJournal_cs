namespace Database.Repositories.Supabase;

using Database.Models.Users;
using global::Supabase.Postgrest;
using System;
using System.Threading.Tasks;

public partial class RepositoriesSupabase
{
    public bool IsLoggedIn { get => SupabaseConnection?.SupabaseClient.Auth.CurrentSession != null; }

    public async Task Login(string Username, string Password)
    {
        try
        {
            var usersTableResult = await FilterAsync<ModelsUser>("Username", Constants.Operator.Equals, Username)
                ?? throw new Exception($"The username {Username} does not exist!");
            if (usersTableResult.Count == 0) { throw new Exception($"The username {Username} does not exist!"); }

            await SupabaseConnection?.SupabaseClient.Auth.SignIn(usersTableResult[0].Email, Password)!;
        }
        catch (Exception e) { throw new Exception($"Login failed: {e.Message}", e); }
    }

    public async Task Logout() => await SupabaseConnection?.SupabaseClient.Auth.SignOut()!;
}