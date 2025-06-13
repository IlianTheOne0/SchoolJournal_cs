namespace Database.Repositories.Supabase;

using Database.DataSources;
using Database.Repositories.Interfaces;
using Database.Repositories.Json;
using Database.Models;
using System.Text.Json.Nodes;

public class RepositoriesSupabase
{
    private DataSourcesSupabase? SupabaseConnection { get; set; } = null;
    private JsonRepository? JsonRepo { get; set; } = null;

    public RepositoriesSupabase(string FilePath)
    {
        try
        {
            if (string.IsNullOrEmpty(FilePath)) { throw new ArgumentException("File path cannot be null or empty!"); }
            InitDatabase(FilePath);
        }
        catch (Exception e) { throw new Exception("Error initializing DatabaseRepository!", e); }
    }

    private async void InitDatabase(string FilePath)
    {
        try
        {
            ModelsSupabaseConfig supbaseConfig = await JsonRepository.ReadJsonAsync<ModelsSupabaseConfig>(FilePath);
            if (supbaseConfig == null || supbaseConfig.Url == null || supbaseConfig.Key == null) { throw new Exception("Supabase configs are null!"); }
            SupabaseConnection = new DataSourcesSupabase(supbaseConfig.Url, supbaseConfig.Key);
        }
        catch (Exception e) { throw new Exception("Error initializing Supabase client!", e); }
    }
}