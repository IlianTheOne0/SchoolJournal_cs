namespace DesktopApplication.ViewModels.SidebarMenu;

using CommunityToolkit.Mvvm.Input;
using DesktopApplication.Services.Auth;
using DesktopApplication.Services.Navigation;
using DesktopApplication.ViewModels.Profile;
using DesktopApplication.Views.Pages;
using Models.Tables.Users;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

public class ViewModelsSidebarMenu : INotifyPropertyChanged
{
    public ICommand CommandLogOut { get; }
    public ICommand CommandProfile { get; }
    public ICommand CommandGrade { get; }
    public ICommand CommandViewGrades { get; }
    public ICommand CommandManageUsers { get; }
    public ICommand CommandGoToHome { get; }

    private readonly ServicesAuth _serviceAuth;
    private readonly ServicesNavigation _serviceNavigation;
    private readonly ViewModelsProfile _viewModelsProfile;

    private Type _currentPageType; public Type CurrentPageType { get => _currentPageType; set { if (_currentPageType != value) { _currentPageType = value; OnPropertyChanged(); UpdateCanGoToHome(); } } }

    public event PropertyChangedEventHandler? PropertyChanged;

    private bool _canGrade; public bool CanGrade { get => _canGrade; set { _canGrade = value; OnPropertyChanged(); } }
    private bool _canViewGrades; public bool CanViewGrades { get => _canViewGrades; set { _canViewGrades = value; OnPropertyChanged(); } }
    private bool _canManageUsers; public bool CanManageUsers { get => _canManageUsers; set { _canManageUsers = value; OnPropertyChanged(); } }
    private bool _canGoToHome; public bool CanGoToHome { get => _canGoToHome; private set { if (_canGoToHome != value) { _canGoToHome = value; OnPropertyChanged(); } } }
    private ModelsUser? _modelUser = null; public ModelsUser ModelUser { get => _modelUser!; set { _modelUser = value; OnPropertyChanged(); OnPropertyChanged("AvatarUrl"); } }

    public ViewModelsSidebarMenu(ServicesAuth ServiceAuth, ServicesNavigation ServiceNavigation, ViewModelsProfile ViewModelProvider)
    {
        _serviceAuth = ServiceAuth; _serviceNavigation = ServiceNavigation; _viewModelsProfile = ViewModelProvider;
        _serviceNavigation.OnPageChanged += pageType => { CurrentPageType = pageType; };

        CommandLogOut = new AsyncRelayCommand(OnLogOut);
        CommandProfile = new RelayCommand(OnProfile);
        CommandGrade = new RelayCommand(OnGrade);
        CommandViewGrades = new RelayCommand(OnViewGrades);
        CommandManageUsers = new RelayCommand(OnManageUsers);
        CommandGoToHome = new RelayCommand(OnGoToHome);
    }

    public void LoadData()
    {
        try
        {
            ModelUser = _serviceAuth.AccessStrategy!.ModelUser;
            CanGrade = _serviceAuth.AccessStrategy.CanGrade();
            CanViewGrades = _serviceAuth.AccessStrategy.CanViewGrades();
            CanManageUsers = _serviceAuth.AccessStrategy.CanManageUsers();

            OnPropertyChanged(nameof(ModelUser));
            OnPropertyChanged(nameof(CanGrade));
            OnPropertyChanged(nameof(CanViewGrades));
            OnPropertyChanged(nameof(CanManageUsers));
        }
        catch (Exception e) { MessageBox.Show($"Load data failed: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private async Task OnLogOut()
    {
        await _serviceAuth.Logout();
        _serviceNavigation.OnPageChanged -= pageType => CurrentPageType = pageType;

        ModelUser = null!;
        OnPropertyChanged(nameof(ModelUser)); OnPropertyChanged(nameof(CanGrade)); OnPropertyChanged(nameof(CanViewGrades)); OnPropertyChanged(nameof(CanManageUsers));
    }

    public void OnProfile() { _viewModelsProfile.ResetEditingState(); _serviceNavigation.NavigateTo<PageProfile, ViewModelsProfile>(); }
    public void OnGrade() => MessageBox.Show($"Grade Page do not implemented", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    public void OnViewGrades() => MessageBox.Show($"View grades Page do not implemented", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    public void OnManageUsers() => MessageBox.Show($"Manage users Page do not implemented", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    public void OnGoToHome()
    {
        if (_viewModelsProfile.IsEditing) { _viewModelsProfile.ResetEditingState(); }
        _serviceNavigation.NavigateTo<PageHome>();
    }

    private void UpdateCanGoToHome() => CanGoToHome = CurrentPageType != typeof(PageHome);
    protected void OnPropertyChanged([CallerMemberName] string? PropertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
}