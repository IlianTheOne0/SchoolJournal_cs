namespace Database.Interfaces.Repositories.Supabase;

using Models.Tables.Users;

using global::Supabase.Postgrest.Models;
using static global::Supabase.Postgrest.Constants;
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
    Task Insert<TMehod>(TMehod Model, string? Schema = null)
        where TMehod : BaseModel, new();

    Task Login(string Username, string Password);
    Task Logout();

    Task<ModelsUser?> GetUserById(int UserId);
    Task UpdateUser(ModelsUser User);
    Task UpdateAvatar(int UserId, string FilePath);
}