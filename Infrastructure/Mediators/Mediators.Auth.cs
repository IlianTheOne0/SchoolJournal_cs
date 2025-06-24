namespace Infrastructure.Mediators.Auth;

using Contracts.Interfaces.Mediators.Auth;
using Database.Interfaces.Repositories.Supabase;
using Database.Repositories.Supabase;

public class MediatorsAuth : InterfacesMediatorsAuth
{
    private readonly InterfacesRepositoriesSupabase _repository;
    
    public bool IsLoggedIn => _repository.IsLoggedIn;

    public MediatorsAuth(RepositoriesSupabase RepositorySupabase) => _repository = RepositorySupabase;

    public Task Login(string username, string password) => _repository.Login(username, password);
    public Task Logout() => _repository.Logout();
}