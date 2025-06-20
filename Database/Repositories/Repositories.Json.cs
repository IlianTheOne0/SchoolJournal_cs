namespace Models.Repositories.Json;

using Models.SupabaseConfig;
using Newtonsoft.Json;

public class RepositoriesJson(string FilePath)
{
    private string _filePath { get; set; } = FilePath;

    public async Task<ModelsSupabaseConfig?> ReadJsonAsync()
    {
        try
        {
            if (!File.Exists(_filePath)) { throw new FileNotFoundException("File not found!", _filePath); }

            string json = await File.ReadAllTextAsync(_filePath);
            var model = JsonConvert.DeserializeObject<ModelsSupabaseConfig>(json);

            return model;
        }
        catch (Exception e) { throw new Exception("Error reading JSON file!", e); }
    }
}