namespace DesktopApplication.Interfaces.Auth;

public interface IServicesAuth
{
    public bool Login(string username, string password);
    public bool Logout();
}
