namespace Database.Repositories.Supabase;

using global::Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static global::Supabase.Postgrest.Constants;

public partial class RepositoriesSupabase
{
    public async Task<List<TMethod>> GetAllAsync<TMethod>(string? Schema = null)
        where TMethod : BaseModel, new()
    {
        try
        {
            SupabaseConnection.SupabaseClient.Postgrest.Options.Schema = Schema ?? _defaultSchema;
            var result = await SupabaseConnection?.SupabaseClient.From<TMethod>().Get()!;

            return result.Models.ToList();
        }
        catch (Exception e) { throw new Exception($"Failed GetAllAsync: {e.Message}", e); }
    }

    public async Task<List<TMethod>> SelectColumnsAsync<TMethod>(Expression<Func<TMethod, object[]>> Columns, string? Schema = null)
        where TMethod : BaseModel, new()
    {
        try
        {
            SupabaseConnection.SupabaseClient.Postgrest.Options.Schema = Schema ?? _defaultSchema;
            var result = await SupabaseConnection?.SupabaseClient.From<TMethod>().Select(Columns).Get()!;

            return result.Models.ToList();
        }
        catch (Exception e) { throw new Exception($"Failed GetAllAsync: {e.Message}", e); }
    }

    public async Task<List<TMethod>> FilterAsync<TMethod>(string ColumnName, Operator Oper, object Value, string? Schema = null)
        where TMethod : BaseModel, new()
    {
        try
        {
            SupabaseConnection.SupabaseClient.Postgrest.Options.Schema = Schema ?? _defaultSchema;
            var result = await SupabaseConnection?.SupabaseClient.From<TMethod>().Filter(ColumnName, Oper, Value).Get()!;

            return result.Models;
        }
        catch (Exception e) { throw new Exception($"Failed GetAllAsync: {e.Message}", e); }
    }

    public async Task<List<TMethod>> FilterWithInnerJoinAsync<TMethod>(string JoinString, string FilterColumnName, Operator Oper, object Value, string? Schema = null)
        where TMethod : BaseModel, new()
    {
        try
        {
            SupabaseConnection.SupabaseClient.Postgrest.Options.Schema = Schema ?? _defaultSchema;
            var result = await SupabaseConnection?.SupabaseClient.From<TMethod>().Select(JoinString).Filter(FilterColumnName, Oper, Value).Get()!;

            return result.Models.ToList();
        }
        catch (Exception e) { throw new Exception($"Failed GetAllAsync: {e.Message}", e); }
    }
}