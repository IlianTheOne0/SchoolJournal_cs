namespace Database.DataSources.Supabase;

using Database.Interfaces.DataSources;
using global::Supabase;

public class DataSourcesSupabase : IDataSources
{
    public Client SupabaseClient { get; private set; } = null!;

    public DataSourcesSupabase(string Url, string Key)
    {
        SupabaseOptions options = new SupabaseOptions { AutoConnectRealtime = true };
        SupabaseClient = new Client(Url, Key, options);
    }
}