namespace DesktopApplication.ViewModels.Login;

using Microsoft.Extensions.Logging;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

public class ViewModelsLogin
{
    public ICommand CommandLognIn { get; }
    private readonly ILogger<ViewModelsLogin> _logger;

    public ViewModelsLogin(ILogger<ViewModelsLogin> Logger)
    {
        _logger = Logger;
        CommandLognIn = new AsyncRelayCommand(OnLogIn);
    }

    private Task OnLogIn()
    {
        _logger.LogInformation($"LognIn command executed [{DateTime.Now.ToLocalTime()}]");
        return Task.CompletedTask;
    }
}