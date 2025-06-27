namespace Database.Repositories.Supabase;

using global::Supabase.Postgrest.Models;
using static global::Supabase.Postgrest.Constants;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

public partial class RepositoriesSupabase
{
    private async Task ChangeTheSchema(string? Schema = null) => SupabaseConnection!.SupabaseClient.Postgrest.Options.Schema = Schema ?? _defaultSchema;

    public async Task<List<TMethod>> GetAllAsync<TMethod>(string? Schema = null)
        where TMethod : BaseModel, new()
    {
        try
        {
            await ChangeTheSchema(Schema);
            var result = await SupabaseConnection?.SupabaseClient.From<TMethod>().Get()!;

            return result.Models.ToList();
        }
        catch (Exception E) { throw new Exception($"Failed GetAllAsync: {E.Message}", E); }
    }

    public async Task<List<TMethod>> SelectColumnsAsync<TMethod>(Expression<Func<TMethod, object[]>> Columns, string? Schema = null)
        where TMethod : BaseModel, new()
    {
        try
        {
            await ChangeTheSchema(Schema);
            var result = await SupabaseConnection?.SupabaseClient.From<TMethod>().Select(Columns).Get()!;

            return result.Models.ToList();
        }
        catch (Exception E) { throw new Exception($"Failed SelectColumnsAsync: {E.Message}", E); }
    }

    public async Task<List<TMethod>> FilterAsync<TMethod>(string ColumnName, Operator Oper, object Value, string? Schema = null)
        where TMethod : BaseModel, new()
    {
        try
        {
            await ChangeTheSchema(Schema);
            var result = await SupabaseConnection?.SupabaseClient.From<TMethod>().Filter(ColumnName, Oper, Value).Get()!;

            return result.Models;
        }
        catch (Exception E) { throw new Exception($"Failed FilterAsync: {E.Message}", E); }
    }

    public async Task<List<TMethod>> FilterWithInnerJoinAsync<TMethod>(string JoinString, string FilterColumnName, Operator Oper, object Value, string? Schema = null)
        where TMethod : BaseModel, new()
    {
        try
        {
            await ChangeTheSchema(Schema);
            var result = await SupabaseConnection?.SupabaseClient.From<TMethod>().Select(JoinString).Filter(FilterColumnName, Oper, Value).Get()!;

            return result.Models.ToList();
        }
        catch (Exception E) { throw new Exception($"Failed FilterWithInnerJoinAsync: {E.Message}", E); }
    }

    public async Task Insert<TMethod>(TMethod item, string? Schema = null)
    where TMethod : BaseModel, new()
    {
        try
        {
            await ChangeTheSchema(Schema);
            await SupabaseConnection!.SupabaseClient.From<TMethod>().Insert(item);
        }
        catch (Exception E) { throw new Exception($"Failed to insert: {E.Message}", E); }
    }

    public async Task Update<TMethod>(TMethod item, string? Schema = null)
        where TMethod : BaseModel, new()
    {
        try
        {
            await ChangeTheSchema(Schema);
            await SupabaseConnection!.SupabaseClient.From<TMethod>().Update(item);
        }
        catch (Exception E) { throw new Exception($"Failed to update: {E.Message}", E); }
    }
}