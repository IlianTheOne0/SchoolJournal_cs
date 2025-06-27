namespace DesktopApplication.ViewModels.SidebarMenu;

using DesktopApplication.Interfaces.Services.Auth;
using DesktopApplication.Interfaces.Services.Navigation;
using DesktopApplication.Interfaces.Services.User;
using DesktopApplication.ViewModels.GradeViewer;
using DesktopApplication.ViewModels.Profile;
using DesktopApplication.ViewModels.GradesAssigner;
using DesktopApplication.Views.Pages;

using Models.Tables.Users;

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

public class ViewModelsSidebarMenu : INotifyPropertyChanged
{
    public ICommand CommandLogOut { get; }
    public ICommand CommandProfile { get; }
    public ICommand CommandGrade { get; }
    public ICommand CommandViewGrades { get; }
    public ICommand CommandManageUsers { get; }
    public ICommand CommandGoToHome { get; }

    private readonly InterfacesServicesAuth _serviceAuth;
    private readonly InterfacesServicesNavigation _serviceNavigation;
    private readonly InterfacesServicesUser _serviceUser;
    private readonly ViewModelsProfile _viewModelProfile;
    private readonly ViewModelsGradesViewer _viewModelGradeViewer;
    private readonly ViewModelsGradesAssigner _viewModelGradeAssigner;
    private Type _currentPageType; public Type CurrentPageType { get => _currentPageType; set { if (_currentPageType != value) { _currentPageType = value; OnPropertyChanged(); UpdateCanGoToHome(); } } }

    public event PropertyChangedEventHandler? PropertyChanged;

    private bool _canGrade; public bool CanGrade { get => _canGrade; set { _canGrade = value; OnPropertyChanged(); } }
    private bool _canViewGrades; public bool CanViewGrades { get => _canViewGrades; set { _canViewGrades = value; OnPropertyChanged(); } }
    private bool _canManageUsers; public bool CanManageUsers { get => _canManageUsers; set { _canManageUsers = value; OnPropertyChanged(); } }
    private bool _canGoToHome; public bool CanGoToHome { get => _canGoToHome; private set { if (_canGoToHome != value) { _canGoToHome = value; OnPropertyChanged(); } } }
    private ModelsUser? _modelUser = null; public ModelsUser ModelUser { get => _modelUser!; set { _modelUser = value; OnPropertyChanged(); OnPropertyChanged("AvatarUrl"); } }

    public ViewModelsSidebarMenu(InterfacesServicesAuth ServiceAuth, InterfacesServicesNavigation ServiceNavigation, InterfacesServicesUser ServiceUser, ViewModelsProfile ViewModelProvider, ViewModelsGradesViewer ViewModelGradesViewer, ViewModelsGradesAssigner ViewModelGradesAssigner)
    {
        _serviceAuth = ServiceAuth; _serviceNavigation = ServiceNavigation; _serviceUser = ServiceUser;
        _viewModelProfile = ViewModelProvider; _viewModelGradeViewer = ViewModelGradesViewer; _viewModelGradeAssigner = ViewModelGradesAssigner;
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
        catch (Exception E)
        {
            MessageBox.Show($"Load data failed: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task OnLogOut()
    {
        await _serviceAuth.Logout();
        _serviceNavigation.OnPageChanged -= pageType => CurrentPageType = pageType;

        ModelUser = null!;
        OnPropertyChanged(nameof(ModelUser)); OnPropertyChanged(nameof(CanGrade)); OnPropertyChanged(nameof(CanViewGrades)); OnPropertyChanged(nameof(CanManageUsers));
    }

    public void OnProfile() { LoadData(); _serviceNavigation.NavigateTo<PagesProfile, ViewModelsProfile>(); }
    public void OnGrade() { LoadData(); _viewModelGradeAssigner.Refresh(); _serviceNavigation.NavigateTo<PagesGradesAssigner, ViewModelsGradesAssigner>(); }
    public async void OnViewGrades() { LoadData(); _viewModelGradeViewer.Refresh(); _serviceNavigation.NavigateTo<PagesGradesViewer, ViewModelsGradesViewer>(); }
    public void OnManageUsers() => MessageBox.Show($"Manage users Page do not implemented", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    public void OnGoToHome() { LoadData(); _viewModelProfile.ResetEditingState(); _serviceNavigation.NavigateTo<PagesHome>(); }

    private void UpdateCanGoToHome() => CanGoToHome = CurrentPageType != typeof(PagesHome);
    private void OnPropertyChanged([CallerMemberName] string? PropertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
}