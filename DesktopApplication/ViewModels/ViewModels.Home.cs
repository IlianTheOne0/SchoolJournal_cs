namespace DesktopApplication.ViewModels.Home;

using CommunityToolkit.Mvvm.Input;
using DesktopApplication.Services.Auth;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

public class ViewModelsHome : INotifyPropertyChanged
{
    public ICommand CommandLogOut { get; }
    private readonly ServicesAuth _serviceAuth;
    private bool _canGrade; public bool CanGrade { get => _canGrade; set { _canGrade = value; OnPropertyChanged(); } }
    private bool _canManageUsers; public bool CanManageUsers { get => _canManageUsers; set { _canManageUsers = value; OnPropertyChanged(); } }

    public event PropertyChangedEventHandler? PropertyChanged;
    
    public ViewModelsHome(ServicesAuth ServiceAuth)
    {
        _serviceAuth = ServiceAuth;

        CommandLogOut = new AsyncRelayCommand(OnLogOut);
    }

    public void LoadDataAsync()
    {
        try
        {
            CanGrade = _serviceAuth.AccessStrategy.CanGrade();
            CanManageUsers = _serviceAuth.AccessStrategy.CanManageUsers();
        }
        catch (Exception e) { MessageBox.Show($"Load data failed: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private async Task OnLogOut()
    {
        await _serviceAuth.Logout();

        OnPropertyChanged("CanGrade");
        OnPropertyChanged("CanManageUsers");
    }

    protected void OnPropertyChanged([CallerMemberName] string? PropertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
}