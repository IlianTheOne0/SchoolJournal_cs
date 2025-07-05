namespace DesktopApplication.Services.Supabase;

using DesktopApplication.Interfaces.Services.Supabase;

using Database.Repositories.Supabase;

public class ServicesSupabase : InterfacesServicesSupabase
{
    public RepositoriesSupabase RepositorySupabase { get; private set; }

    public ServicesSupabase() => RepositorySupabase = new RepositoriesSupabase();
}