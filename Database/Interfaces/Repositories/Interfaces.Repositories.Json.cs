namespace Database.Interfaces.Repositories.Json;

public interface InterfacesRepositoriesJson
{
    Task<TMethod?> ReadJsonAsync<TMethod>();
}