namespace DesktopApplication.Services.Auth;

using DesktopApplication.Interfaces.Auth;
using DesktopApplication.Interfaces.Supabase;
using Microsoft.Extensions.Logging;

public class ServicesAuth : IServicesAuth
{
    private readonly ILogger<ServicesAuth> _logger;
    private readonly IServicesSupabase _serviceSupabase;

    public ServicesAuth(ILogger<ServicesAuth> Logger, IServicesSupabase ServicesSupabase)
    {
        _logger = Logger;
        _logger.LogInformation("ServicesAuth initialized!");

        _serviceSupabase = ServicesSupabase;
    }

    public bool Login(string Username, string Password)
    {
        _logger.LogInformation($"Login in user: {Username}", Username);
        return _serviceSupabase.RepositorySupabase.Login(Username, Password);
    }

    public bool Logout()
    {
        _logger.LogInformation($"Logging out {DateTime.Now.ToLocalTime()}");
        return _serviceSupabase.RepositorySupabase.Logout();
    }
}