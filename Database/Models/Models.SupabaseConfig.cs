namespace Database.Models.SupabaseConfig;

public class ModelsSupabaseConfig
{
	public string? Url { get; set; } = null;
	public string? Key { get; set; } = null;

	public ModelsSupabaseConfig(string url, string key) { Url = url; Key = key; }
}