namespace Models.Repositories.Supabase;

using Models.Tables.Users;
using Models.Tables.Statuses;
using global::Supabase.Postgrest;
using System;
using System.Threading.Tasks;

public partial class RepositoriesSupabase
{
    public bool IsLoggedIn { get => SupabaseConnection?.SupabaseClient.Auth.CurrentSession != null; }
    public ModelsUser? ModelUser { get; private set; } = null;

    public async Task Login(string Username, string Password)
    {
        try
        {
            var usersTableResult = await FilterAsync<ModelsUser>("Username", Constants.Operator.Equals, Username)
                ?? throw new Exception($"The username {Username} does not exist!");
            if (usersTableResult.Count == 0) { throw new Exception($"The username {Username} does not exist!"); }
            ModelUser = usersTableResult.FirstOrDefault()
                ?? throw new Exception($"Could not retrieve user details for username: {Username}");

            await SupabaseConnection?.SupabaseClient.Auth.SignIn(ModelUser.Email, Password)!;

            var statusesTableResult = await FilterAsync<ModelsStatuses>("Id", Constants.Operator.Equals, ModelUser.StatusId)
                ?? throw new Exception($"The status id {ModelUser.StatusId} does not exist!");
            if (statusesTableResult.Count == 0) { throw new Exception($"The status id {ModelUser.StatusId} does not exist!"); }
            var status = statusesTableResult.FirstOrDefault()
                ?? throw new Exception($"Could not retrieve status details for status id: {ModelUser.StatusId}");
            
            ModelUser.StatusName = status.Status;
        }
        catch (Exception e) { throw new Exception($"Login failed: {e.Message}", e); }
    }

    public async Task Logout()
    {
        await SupabaseConnection?.SupabaseClient.Auth.SignOut()!;

        ModelUser = null;
    }

    //public async Task UpdateAvatar(int UserId, string FilePath)
    //{
    //    try
    //    {
    //        var bucket = SupabaseConnection?.SupabaseClient.Storage.From("avatars");
    //        var fileName = $"avatar_{UserId}_{DateTime.Now.Ticks}.jpg";

    //        await bucket!.Update(fileName, FilePath);
    //        var url = bucket.GetPublicUrl(fileName);

    //        await SupabaseConnection?.SupabaseClient
    //            .From<ModelsUser>()
    //            .Where(user => user.Id == UserId)
    //            .Set(user => user.AvatarUrl!, url)
    //            .Update()!;
    //    }
    //    catch (Exception e) { throw new Exception($"Update avatar failed: {e.Message}", e ); }
    //}
}