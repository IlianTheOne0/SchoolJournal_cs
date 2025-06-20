namespace DesktopApplication.Services.Supabase;

using Models.Repositories.Supabase;

public class ServicesSupabase
{
    public RepositoriesSupabase RepositorySupabase { get; set; }

    public ServicesSupabase() => RepositorySupabase = new RepositoriesSupabase();
}