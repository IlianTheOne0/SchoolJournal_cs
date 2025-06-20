namespace Models.DataSources.Supabase;

using Models.Interfaces.DataSources;
using global::Supabase;

public class DataSourcesSupabase : InterfacesDataSources
{
    public Client SupabaseClient { get; private set; } = null!;

    public DataSourcesSupabase(string Url, string Key)
    {
        SupabaseOptions options = new SupabaseOptions { AutoConnectRealtime = true };
        SupabaseClient = new Client(Url, Key, options);
    }
}