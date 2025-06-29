namespace Database.Repositories.Supabase;

using Database.Repositories.Supabase.Extensions;
using Models.Supports.SupabaseCommands;

using global::Supabase.Postgrest.Models;
using static global::Supabase.Postgrest.Constants;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

public partial class RepositoriesSupabase
{
    private async Task ChangeTheSchema(string? Schema = null) => SupabaseConnection!.SupabaseClient.Postgrest.Options.Schema = Schema ?? _defaultSchema;

    public async Task<List<TModel>> GetAllAsync<TModel>(string? Schema = null)
        where TModel : BaseModel, new()
    {
        try
        {
            await ChangeTheSchema(Schema);
            var result = await SupabaseConnection?.SupabaseClient.From<TModel>().Get()!;

            return result.Models.ToList();
        }
        catch (Exception E) { throw new Exception($"Failed GetAllAsync: {E.Message}", E); }
    }

    public async Task<List<TModel>> SelectColumnsAsync<TModel>(Expression<Func<TModel, object[]>> Columns, string? Schema = null)
        where TModel : BaseModel, new()
    {
        try
        {
            await ChangeTheSchema(Schema);
            var result = await SupabaseConnection?.SupabaseClient.From<TModel>().Select(Columns).Get()!;

            return result.Models.ToList();
        }
        catch (Exception E) { throw new Exception($"Failed SelectColumnsAsync: {E.Message}", E); }
    }

    public async Task<List<TModel>> FilterAsync<TModel>(string ColumnName, Operator Oper, object Value, string? Schema = null)
        where TModel : BaseModel, new()
    {
        try
        {
            await ChangeTheSchema(Schema);
            var result = await SupabaseConnection?.SupabaseClient.From<TModel>().Filter(ColumnName, Oper, Value).Get()!;

            return result.Models;
        }
        catch (Exception E) { throw new Exception($"Failed FilterAsync: {E.Message}", E); }
    }

    public async Task<List<TModel>> FilterAsync<TModel>(IEnumerable<(string ColumnName, Operator Operator, object Value)> Conditions, string? Schema = null)
        where TModel : BaseModel, new()
    {
        try
        {
            await ChangeTheSchema(Schema);

            var result = await SupabaseConnection!
                .SupabaseClient
                .From<TModel>()
                .ApplyConditions(Conditions)
                .Get();

            return result.Models;
        }
        catch (Exception E) { throw new Exception($"Failed FilterAsync (multiple): {E.Message}", E); }
    }

    public async Task<List<TModel>> FilterWithInnerJoinAsync<TModel>(string JoinString, string FilterColumnName, Operator Oper, object Value, string? Schema = null)
        where TModel : BaseModel, new()
    {
        try
        {
            await ChangeTheSchema(Schema);
            var result = await SupabaseConnection?.SupabaseClient.From<TModel>().Select(JoinString).Filter(FilterColumnName, Oper, Value).Get()!;

            return result.Models.ToList();
        }
        catch (Exception E) { throw new Exception($"Failed FilterWithInnerJoinAsync: {E.Message}", E); }
    }

    public async Task Insert<TModel>(TModel Item, string? Schema = null)
    where TModel : BaseModel, new()
    {
        try
        {
            await ChangeTheSchema(Schema);
            await SupabaseConnection!.SupabaseClient.From<TModel>().Insert(Item);
        }
        catch (Exception E) { throw new Exception($"Failed to insert: {E.Message}", E); }
    }

    public async Task Upsert<TModel>(TModel Item, string[] ConflictColumns, string? Schema = null)
        where TModel : BaseModel, new()
    {
        try
        {
            await ChangeTheSchema(Schema);
            var conflictString = string.Join(",", ConflictColumns);

            await SupabaseConnection!.SupabaseClient.From<TModel>().OnConflict(conflictString).Upsert(Item);
        }
        catch (Exception E) { throw new Exception($"Failed to upsert: {E.Message}", E); }
    }

    public async Task Delete<TModel>(TModel item, string? Schema = null)
        where TModel : BaseModel, InterfacesModelsWithId, new()
    {
        try
        {
            await ChangeTheSchema(Schema);
            await SupabaseConnection!.SupabaseClient.From<TModel>().Where(provider => provider.Id == item.Id).Delete();
        }
        catch (Exception E) { throw new Exception($"Failed to delete: {E.Message}", E); }
    }
}