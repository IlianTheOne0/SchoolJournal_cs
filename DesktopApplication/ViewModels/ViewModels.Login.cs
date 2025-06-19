namespace DesktopApplication.ViewModels.Login;

using CommunityToolkit.Mvvm.Input;
using DesktopApplication.Services.Navigation;
using DesktopApplication.Services.Auth;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;

public class ViewModelsLogin
{
    public ICommand CommandLogIn { get; }
    private readonly ServicesAuth _serviceAuth;
    public event PropertyChangedEventHandler? PropertyChanged;

    private string _username; public string Username { get => _username; set { _username = value; OnPropertyChanged(); } }
    private string _password; public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }

    public ViewModelsLogin(ServicesAuth ServiceAuth)
    {
        _username = string.Empty;
        _password = string.Empty;

        _serviceAuth = ServiceAuth;

        CommandLogIn = new AsyncRelayCommand(OnLogIn);
    }

    private async Task OnLogIn()
    {
        try { await _serviceAuth.Login(Username, Password); }
        catch (Exception e) { MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
    protected void OnPropertyChanged([CallerMemberName] string? PropertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
}