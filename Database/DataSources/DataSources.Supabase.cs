namespace Database.DataSources.Supabase;

using Database.Interfaces.DataSources.Supabase;

using global::Supabase;

public class DataSourcesSupabase : InterfacesDataSourcesSupabase
{
    public Client SupabaseClient { get; private set; } = null!;

    public DataSourcesSupabase(string Url, string Key)
    {
        SupabaseOptions options = new SupabaseOptions { AutoConnectRealtime = true };
        SupabaseClient = new Client(Url, Key, options);
    }
}