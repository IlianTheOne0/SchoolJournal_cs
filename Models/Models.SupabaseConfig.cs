namespace Models.SupabaseConfig;

public class ModelsSupabaseConfig
{
    public string? Url { get; set; } = null;
    public string? Key { get; set; } = null;
    public string? DefaultSchema { get; set; } = null;

    public ModelsSupabaseConfig(string Url, string Key, string DefaultSchema) { this.Url = Url; this.Key = Key; this.DefaultSchema = DefaultSchema; }
}