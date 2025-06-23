namespace Database.Repositories.Supabase;

using Models.Tables.Users;
using global::Supabase.Postgrest;
using System.Threading.Tasks;

public partial class RepositoriesSupabase
{
    public async Task<ModelsUser?> GetUserById(int UserId)
    {
        try
        {
            var result = await FilterAsync<ModelsUser>("Id", Constants.Operator.Equals, UserId);
            return result?.FirstOrDefault();
        }
        catch (Exception e) { throw new Exception($"Failed to get user by ID: {e.Message}", e); }
    }

    public async Task UpdateUser(ModelsUser User)
    {
        try
        {

            await SupabaseConnection?.SupabaseClient.From<ModelsUser>().Upsert(User)!;
        }
        catch (Exception e) { throw new Exception($"Failed to update user: {e.Message}", e); }
    }
}