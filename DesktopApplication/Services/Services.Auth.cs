namespace DesktopApplication.Services.Auth;

using DesktopApplication.Services.Supabase;
using Microsoft.Extensions.Logging;

public interface IServicesAuth
{
    public bool Login(string username, string password);
    public bool Logout();
}

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

    public bool Login(string username, string password)
    {
        _logger.LogInformation($"Login in user: {username}", username);
        return _serviceSupabase.RepositorySupabase.Login(username, password);
    }

    public bool Logout()
    {
        _logger.LogInformation($"Logging out {DateTime.Now.ToLocalTime()}");
        return _serviceSupabase.RepositorySupabase.Logout();
    }
}