namespace DesktopApplication.Interfaces.Services.Auth;

using System.ComponentModel;

public interface InterfacesServicesAuth
{
    bool IsLoggedIn { get; set; }
    event PropertyChangedEventHandler? PropertyChanged;

    Task Login(string Username, string Password);
    Task Logout();

    void SetupAccessStrategy();
}