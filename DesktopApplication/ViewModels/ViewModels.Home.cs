namespace DesktopApplication.ViewModels.Home;

using CommunityToolkit.Mvvm.Input;
using DesktopApplication.Services.Navigation;
using DesktopApplication.Services.Auth;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

public class ViewModelsHome
{
    public ICommand CommandLogOut { get; }
    private readonly ServicesAuth _serviceAuth;
    public event PropertyChangedEventHandler? PropertyChanged;

    public ViewModelsHome(ServicesAuth ServiceAuth)
    {
        _serviceAuth = ServiceAuth;

        CommandLogOut = new AsyncRelayCommand(OnLogOut);
    }

    private async Task OnLogOut() => await _serviceAuth.Logout();
    protected void OnPropertyChanged([CallerMemberName] string? PropertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
}