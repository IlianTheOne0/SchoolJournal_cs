namespace DesktopApplication.ViewModels.SidebarMenu;

using CommunityToolkit.Mvvm.Input;
using DesktopApplication.ViewModels.GradesAssigner;
using DesktopApplication.ViewModels.GradesViewer;
using DesktopApplication.ViewModels.Profile;
using DesktopApplication.Views.Pages;
using System.Windows;
using System.Windows.Input;

public partial class ViewModelsSidebarMenu
{
    public ICommand CommandLogOut { get; private set; }
    public ICommand CommandProfile { get; private set; }
    public ICommand CommandGrade { get; private set; }
    public ICommand CommandViewGrades { get; private set; }
    public ICommand CommandManageUsers { get; private set; }
    public ICommand CommandGoToHome { get; private set; }

    private void InitializeComamnds()
    {
        CommandLogOut = new AsyncRelayCommand(OnLogOut);
        CommandProfile = new RelayCommand(OnProfile);
        CommandGrade = new RelayCommand(OnAssignGrades);
        CommandViewGrades = new RelayCommand(OnViewGrades);
        CommandManageUsers = new RelayCommand(OnManageUsers);
        CommandGoToHome = new RelayCommand(OnGoToHome);
    }

    public void OnProfile() { LoadData(); _serviceNavigation.NavigateTo<PagesProfile, ViewModelsProfile>(); }
    public void OnAssignGrades() { LoadData(); _serviceNavigation.NavigateTo<PagesGradesAssigner, ViewModelsGradesAssigner>(); }
    public async void OnViewGrades() { LoadData(); _viewModelGradesViewer.Refresh(); _serviceNavigation.NavigateTo<PagesGradesViewer, ViewModelsGradesViewer>(); }
    public void OnManageUsers() => MessageBox.Show($"Manage users Page do not implemented", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    public void OnGoToHome() { LoadData(); _viewModelProfile.ResetEditingState(); _serviceNavigation.NavigateTo<PagesHome>(); }
}