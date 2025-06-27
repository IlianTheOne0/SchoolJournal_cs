namespace Database.Interfaces.Repositories.Supabase;

using Models.Supports.SupabaseCommands;
using static global::Supabase.Postgrest.Constants;
using global::Supabase.Postgrest.Models;
using Models.Tables.Users;
using System.Linq.Expressions;

public interface InterfacesRepositoriesSupabase
{
    bool IsLoggedIn { get; }
    ModelsUserExtended? ModelUser { get; }

    Task<List<TMethod>> GetAllAsync<TMethod>(string? Schema = null)
        where TMethod : BaseModel, new();
    Task<List<TMethod>> SelectColumnsAsync<TMethod>(Expression<Func<TMethod, object[]>> Columns, string? Schema = null)
        where TMethod : BaseModel, new();
    Task<List<TMethod>> FilterAsync<TMethod>(string ColumnName, Operator Oper, object Value, string? Schema = null)
        where TMethod : BaseModel, new();
    Task<List<TMethod>> FilterWithInnerJoinAsync<TMethod>(string JoinString, string FilterColumnName, Operator Oper, object Value, string? Schema = null)
        where TMethod : BaseModel, new();

    Task Insert<TMethod>(TMethod Item, string? Schema = null)
        where TMethod : BaseModel, new();
    Task Upsert<TMethod>(TMethod Item, string[] ConflictColumns, string? Schema = null)
        where TMethod : BaseModel, new();
    Task Delete<TMethod>(TMethod Item, string? Schema = null)
        where TMethod : BaseModel, InterfacesModelsWithId, new();

    Task Login(string Username, string Password);
    Task Logout();

    Task<ModelsUser?> GetUserById(int UserId);
    Task UpdateUser(ModelsUser User);
    Task UpdateAvatar(int UserId, string FilePath);
}