namespace DesktopApplication.Services.Supabase;

using Database.Repositories.Supabase;
using System.Web;

public interface IServicesSupabase
{
    public RepositoriesSupabase RepositorySupabase { get; set; }
}

public class ServicesSupabase : IServicesSupabase
{
    public RepositoriesSupabase RepositorySupabase { get; set; }

    public ServicesSupabase() => RepositorySupabase = new RepositoriesSupabase();
}