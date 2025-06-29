namespace Database.Interfaces.Repositories.Supabase;

using Models.Tables.Users;
using Models.Supports.SupabaseCommands;

using static global::Supabase.Postgrest.Constants;
using global::Supabase.Postgrest.Models;
using System.Linq.Expressions;

public interface InterfacesRepositoriesSupabase
{
    bool IsLoggedIn { get; }
    ModelsUserExtended? ModelUser { get; }

    Task<List<TModel>> GetAllAsync<TModel>(string? Schema = null)
        where TModel : BaseModel, new();
    Task<List<TModel>> SelectColumnsAsync<TModel>(Expression<Func<TModel, object[]>> Columns, string? Schema = null)
        where TModel : BaseModel, new();
    Task<List<TModel>> FilterAsync<TModel>(string ColumnName, Operator Oper, object Value, string? Schema = null)
        where TModel : BaseModel, new();
    Task<List<TModel>> FilterAsync<TModel>(IEnumerable<(string ColumnName, Operator Operator, object Value)> Conditions, string? Schema = null)
        where TModel : BaseModel, new();
    Task<List<TModel>> FilterWithInnerJoinAsync<TModel>(string JoinString, string FilterColumnName, Operator Oper, object Value, string? Schema = null)
        where TModel : BaseModel, new();

    Task Insert<TModel>(TModel Item, string? Schema = null)
        where TModel : BaseModel, new();
    Task Upsert<TModel>(TModel Item, string[] ConflictColumns, string? Schema = null)
        where TModel : BaseModel, new();
    Task Delete<TModel>(TModel Item, string? Schema = null)
        where TModel : BaseModel, InterfacesModelsWithId, new();

    Task Login(string Username, string Password);
    Task Logout();

    Task<ModelsUser?> GetUserById(int UserId);
    Task UpdateUser(ModelsUser User);
    Task UpdateAvatar(int UserId, string FilePath);
}