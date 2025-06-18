namespace Database.Repositories.Supabase;

public partial class RepositoriesSupabase
{
    public bool IsLoggedIn { get => SupabaseConnection?.SupabaseClient.Auth.CurrentSession != null; }

    public async Task Login(string Username, string Password) => await SupabaseConnection?.SupabaseClient.Auth.SignIn(Username, Password)!;
    public async Task Logout() => await SupabaseConnection?.SupabaseClient.Auth.SignOut()!;
}