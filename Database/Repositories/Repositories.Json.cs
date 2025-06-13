namespace Database.Repositories.Json;

public class RepositoriesJson(string FilePath)
{
    private string FilePath { get; set; } = FilePath;

    public async Task<ModelsSupabaseConfig?> ReadJsonAsync()
    {
        try
        {
            if (!File.Exists(FilePath)) { throw new FileNotFoundException("File not found!", FilePath); }
        
            string json = await reader.ReadToEndAsync();
            var model = JsonConvert.DeserializeObject<ModelsSupabaseConfig>(json);
            
            return model;
        }
        catch (Exception e) { throw new Exception("Error reading JSON file!", e); }
    }
}