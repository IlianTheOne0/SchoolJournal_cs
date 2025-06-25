namespace Database.Repositories.Supabase;

using Database.DataSources.Supabase;
using Database.Interfaces.Repositories.Database;
using Database.Interfaces.Repositories.Json;
using Database.Interfaces.Repositories.Supabase;
using Database.Repositories.Json;
using Models.Repositories.SupabaseConfig;

public partial class RepositoriesSupabase : InterfacesRepositoriesDatabase, InterfacesRepositoriesSupabase
{
    public DataSourcesSupabase? SupabaseConnection { get; set; } = null;
    private InterfacesRepositoriesJson? _repositoryJson { get; set; } = null;
    
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
            ModelsSupabaseConfig? supbaseConfig = await _repositoryJson.ReadJsonAsync<ModelsSupabaseConfig>();
            if (supbaseConfig == null || supbaseConfig.Url == null || supbaseConfig.Key == null) { throw new Exception("Supabase configs are null!"); }

            _defaultSchema = supbaseConfig.DefaultSchema!;
            SupabaseConnection = new DataSourcesSupabase(supbaseConfig.Url, supbaseConfig.Key);
        }
        catch (Exception e) { throw new Exception("Error initializing Supabase client!", e); }
    }
}