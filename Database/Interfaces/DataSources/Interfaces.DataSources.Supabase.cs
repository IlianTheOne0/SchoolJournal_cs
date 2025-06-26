namespace Database.Interfaces.DataSources.Supabase;

using global::Supabase;

public interface InterfacesDataSourcesSupabase
{
    Client SupabaseClient { get; }
}