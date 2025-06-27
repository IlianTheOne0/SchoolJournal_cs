namespace DesktopApplication.ViewModels.Login;

using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Input;

public partial class ViewModelsLogin
{
    public ICommand CommandLogIn { get; private set; }

    private void InitializeComamnds() => CommandLogIn = new AsyncRelayCommand(OnLogIn);

    private async Task OnLogIn()
    {
        try { await _serviceAuth.Login(Username, Password); }
        catch (Exception e) { MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
}