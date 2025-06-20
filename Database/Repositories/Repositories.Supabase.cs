namespace Models.Repositories.Supabase;

using Models.DataSources.Supabase;
using Models.Interfaces.Repositories.Database;
using Models.SupabaseConfig;
using Models.Repositories.Json;

public partial class RepositoriesSupabase : InterfacesDatabaseRepositories
{
    private DataSourcesSupabase? SupabaseConnection { get; set; } = null;
    private RepositoriesJson? _repositoryJson { get; set; } = null;
    
    private string _defaultSchema { get; set; } = null!;

    public RepositoriesSupabase()
    {
        string solutionDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\"));
        string filePath = solutionDirectory + @"Database\.env\supabase_keys.json";

        try
        {
            if (string.IsNullOrEmpty(filePath)) { throw new ArgumentException("File path cannot be null or empty!"); }
            InitDatabase(filePath);
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

            _defaultSchema = supbaseConfig.DefaultSchema!;
            SupabaseConnection = new DataSourcesSupabase(supbaseConfig.Url, supbaseConfig.Key);
        }
        catch (Exception e) { throw new Exception("Error initializing Supabase client!", e); }
    }
}