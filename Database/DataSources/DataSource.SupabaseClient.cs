namespace Database.DataSources.Supabase;

using Database.DataSources.Interface;

public class DataSourcesSupabase : IDataSources
{
	public Supabase.Client SupabaseClient { get; private set; }

	public Execute(string Url, string Key)
	{
		Supabase.SupabaseOptions options = new Supabase.SupabaseOptions { AutoConnectRealtime = true };

		SupabaseClient = new Supabase.Client(Url, Key, options);
	}
}