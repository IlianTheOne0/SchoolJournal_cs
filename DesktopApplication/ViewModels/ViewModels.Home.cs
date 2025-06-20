namespace DesktopApplication.ViewModels.Home;

using CommunityToolkit.Mvvm.Input;
using DesktopApplication.Services.Auth;
using DesktopApplication.Services.Navigation;
using DesktopApplication.Views;
using MaterialDesignThemes.Wpf;
using Models.Tables.Users;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

public class ViewModelsHome : INotifyPropertyChanged
{
    public ICommand CommandLogOut { get; }
    public ICommand NavigateToBreadcrumbCommand;

    private readonly ServicesAuth _serviceAuth;
    private readonly ServicesNavigation _serviceNavigation;

    public event PropertyChangedEventHandler? PropertyChanged;

    private bool _canGrade; public bool CanGrade { get => _canGrade; set { _canGrade = value; OnPropertyChanged(); } }
    private bool _canViewGrades; public bool CanViewGrades { get => _canViewGrades; set { _canViewGrades = value; OnPropertyChanged(); } }
    private bool _canManageUsers; public bool CanManageUsers { get => _canManageUsers; set { _canManageUsers = value; OnPropertyChanged(); } }
    private ModelsUser? _modelUser = null; public ModelsUser ModelUser { get => _modelUser!; set { _modelUser = value; OnPropertyChanged(); } }

    public ViewModelsHome(ServicesAuth ServiceAuth, ServicesNavigation ServiceNavigation)
    {
        _serviceAuth = ServiceAuth;
        _serviceNavigation = ServiceNavigation;

        CommandLogOut = new AsyncRelayCommand(OnLogOut);
    }

    public void LoadDataAsync()
    {
        try
        {
            ModelUser = _serviceAuth.AccessStrategy!.ModelUser;

            CanGrade = _serviceAuth.AccessStrategy.CanGrade();
            CanViewGrades = _serviceAuth.AccessStrategy.CanViewGrades();
            CanManageUsers = _serviceAuth.AccessStrategy.CanManageUsers();
        }
        catch (Exception e) { MessageBox.Show($"Load data failed: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private async Task OnLogOut()
    {
        await _serviceAuth.Logout();

        ModelUser = null!;
        OnPropertyChanged(nameof(CanGrade)); OnPropertyChanged(nameof(CanViewGrades)); OnPropertyChanged(nameof(CanManageUsers));
    }

    protected void OnPropertyChanged([CallerMemberName] string? PropertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
}