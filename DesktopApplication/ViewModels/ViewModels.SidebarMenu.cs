namespace DesktopApplication.ViewModels.SidebarMenu;

using DesktopApplication.Interfaces.Services.Auth;
using DesktopApplication.Interfaces.Services.Navigation;
using DesktopApplication.Interfaces.Services.User;
using DesktopApplication.ViewModels.GradesViewer;
using DesktopApplication.ViewModels.Profile;
using DesktopApplication.Views.Pages;

using Models.Tables.Users;

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

public partial class ViewModelsSidebarMenu : INotifyPropertyChanged
{
    private readonly InterfacesServicesAuth _serviceAuth;
    private readonly InterfacesServicesNavigation _serviceNavigation;
    private readonly InterfacesServicesUser _serviceUser;
    private readonly ViewModelsProfile _viewModelProfile;
    private readonly ViewModelsGradesViewer _viewModelGradesViewer;
    private Type _currentPageType; public Type CurrentPageType { get => _currentPageType; set { if (_currentPageType != value) { _currentPageType = value; OnPropertyChanged(); UpdateCanGoToHome(); } } }

    public event PropertyChangedEventHandler? PropertyChanged;

    private bool _canGrade; public bool CanGrade { get => _canGrade; set { _canGrade = value; OnPropertyChanged(); } }
    private bool _canViewGrades; public bool CanViewGrades { get => _canViewGrades; set { _canViewGrades = value; OnPropertyChanged(); } }
    private bool _canManageUsers; public bool CanManageUsers { get => _canManageUsers; set { _canManageUsers = value; OnPropertyChanged(); } }
    private bool _canGoToHome; public bool CanGoToHome { get => _canGoToHome; private set { if (_canGoToHome != value) { _canGoToHome = value; OnPropertyChanged(); } } }
    private ModelsUser? _modelUser = null; public ModelsUser ModelUser { get => _modelUser!; set { _modelUser = value; OnPropertyChanged(); OnPropertyChanged("AvatarUrl"); } }

    public ViewModelsSidebarMenu(InterfacesServicesAuth ServiceAuth, InterfacesServicesNavigation ServiceNavigation, InterfacesServicesUser ServiceUser, ViewModelsProfile ViewModelProvider, ViewModelsGradesViewer ViewModelGradesViewer)
    {
        _serviceAuth = ServiceAuth; _serviceNavigation = ServiceNavigation; _serviceUser = ServiceUser;
        _viewModelProfile = ViewModelProvider; _viewModelGradesViewer = ViewModelGradesViewer;

        _serviceNavigation.OnPageChanged += pageType => { CurrentPageType = pageType; };
        InitializeComamnds();
    }

    public void LoadData()
    {
        try
        {
            _serviceUser.RefreshTheData();
            ModelUser = _serviceUser.AccessStrategy?.ModelUser!;
            CanGrade = _serviceUser.AccessStrategy?.CanGrade() ?? false;
            CanViewGrades = _serviceUser.AccessStrategy?.CanViewGrades() ?? false;
            CanManageUsers = _serviceUser.AccessStrategy?.CanManageUsers() ?? false;

            OnPropertyChanged(nameof(ModelUser));
            OnPropertyChanged(nameof(CanGrade));
            OnPropertyChanged(nameof(CanViewGrades));
            OnPropertyChanged(nameof(CanManageUsers));
        }
        catch (Exception e)
        {
            MessageBox.Show($"Load data failed: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task OnLogOut()
    {
        await _serviceAuth.Logout();
        _serviceNavigation.OnPageChanged -= pageType => CurrentPageType = pageType;

        ModelUser = null!;
        OnPropertyChanged(nameof(ModelUser)); OnPropertyChanged(nameof(CanGrade)); OnPropertyChanged(nameof(CanViewGrades)); OnPropertyChanged(nameof(CanManageUsers));
    }

    private void UpdateCanGoToHome() => CanGoToHome = CurrentPageType != typeof(PagesHome);
    protected void OnPropertyChanged([CallerMemberName] string? PropertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
}