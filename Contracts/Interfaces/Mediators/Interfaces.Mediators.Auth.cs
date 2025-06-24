namespace Contracts.Interfaces.Mediators.Auth;

public interface InterfacesMediatorsAuth
{
    bool IsLoggedIn { get; }

    Task Login(string username, string password);
    Task Logout();
}