namespace DesktopApplication.Services.Supabase;

using DesktopApplication.Interfaces.Supabase;
using Database.Repositories.Supabase;

public class ServicesSupabase : IServicesSupabase
{
    public RepositoriesSupabase RepositorySupabase { get; set; }

    public ServicesSupabase() => RepositorySupabase = new RepositoriesSupabase();
}