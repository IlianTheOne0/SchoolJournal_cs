namespace Database.DataSources.Supabase;

using Database.DataSources.Interface;
using global::Supabase;

public class DataSourcesSupabase : IDataSources
{
    public Client SupabaseClient { get; private set; } = null!;

    public void Execute(string url, string key)
    {
        SupabaseOptions options = new SupabaseOptions { AutoConnectRealtime = true };
        SupabaseClient = new Client(url, key, options);
    }
}