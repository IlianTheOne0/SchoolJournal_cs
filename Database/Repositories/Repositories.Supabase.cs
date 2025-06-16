namespace Database.Repositories.Supabase;

using Database.DataSources.Supabase;
using Database.Repositories.DatabaseInterface;
using Database.Repositories.Json;
using Database.Models.SupabaseConfig;

public partial class RepositoriesSupabase : IDatabaseRepositories
{
    private DataSourcesSupabase? SupabaseConnection { get; set; } = null;
    private RepositoriesJson? _repositoryJson { get; set; } = null;

    public RepositoriesSupabase()
    {
        const string FilePath = @"../.env/supabase_keys.json";

        try
        {
            if (string.IsNullOrEmpty(FilePath)) { throw new ArgumentException("File path cannot be null or empty!"); }
            InitDatabase(FilePath);
        }
        catch (Exception e) { throw new Exception("Error initializing DatabaseRepository!", e); }
    }

    public async void InitDatabase(string FilePath)
    {
        try
        {
            _repositoryJson = new RepositoriesJson(FilePath);
            ModelsSupabaseConfig? supbaseConfig = await _repositoryJson.ReadJsonAsync();
            if (supbaseConfig == null || supbaseConfig.Url == null || supbaseConfig.Key == null) { throw new Exception("Supabase configs are null!"); }

            SupabaseConnection = new DataSourcesSupabase();
            SupabaseConnection.Execute(supbaseConfig.Url, supbaseConfig.Key);
        }
        catch (Exception e) { throw new Exception("Error initializing Supabase client!", e); }
    }
}