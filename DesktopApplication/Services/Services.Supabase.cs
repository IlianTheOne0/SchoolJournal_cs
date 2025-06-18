namespace DesktopApplication.Services.Supabase;

using Database.Repositories.Supabase;

public class ServicesSupabase
{
    public RepositoriesSupabase RepositorySupabase { get; set; }

    public ServicesSupabase() => RepositorySupabase = new RepositoriesSupabase();
}