namespace Database.Repositories.Supabase.Extensions;

using global::Supabase.Postgrest.Interfaces;
using global::Supabase.Postgrest.Models;
using static global::Supabase.Postgrest.Constants;

public static class SupabaseExtensions
{
    public static IPostgrestTable<T> ApplyConditions<T>(this IPostgrestTable<T> query, IEnumerable<(string ColumnName, Operator Operator, object Value)> conditions)
        where T : BaseModel, new()
    {
        foreach (var condition in conditions) { query = query.Filter(condition.ColumnName, condition.Operator, condition.Value); }
        return query;
    }
}
