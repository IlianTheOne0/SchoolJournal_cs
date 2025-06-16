namespace DesktopApplication.Interfaces.Supabase;

using Database.Repositories.Supabase;

public interface IServicesSupabase
{
    public RepositoriesSupabase RepositorySupabase { get; set; }
}