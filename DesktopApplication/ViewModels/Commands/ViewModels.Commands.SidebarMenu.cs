namespace DesktopApplication.ViewModels.SidebarMenu;

using DesktopApplication.ViewModels.GradesAssigner;
using DesktopApplication.ViewModels.GradesViewer;
using DesktopApplication.ViewModels.Profile;
using DesktopApplication.ViewModels.MarkAbsences;
using DesktopApplication.Views.Pages;

using CommunityToolkit.Mvvm.Input;
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
    public ICommand CommandMarkAbsence { get; private set; }

    private void InitializeComamnds()
    {
        CommandLogOut = new AsyncRelayCommand(OnLogOut);
        CommandProfile = new RelayCommand(OnProfile);
        CommandGrade = new RelayCommand(OnAssignGrades);
        CommandViewGrades = new RelayCommand(OnViewGrades);
        CommandManageUsers = new RelayCommand(OnManageUsers);
        CommandGoToHome = new RelayCommand(OnGoToHome);
        CommandMarkAbsence = new RelayCommand(OnMarkAbsence);
    }

    private void OnProfile() { LoadData(); _serviceNavigation.NavigateTo<PagesProfile, ViewModelsProfile>(); }
    private void OnAssignGrades() { LoadData(); _serviceNavigation.NavigateTo<PagesGradesAssigner, ViewModelsGradesAssigner>(); }
    private void OnViewGrades() { LoadData(); _viewModelGradesViewer.Reset(); _serviceNavigation.NavigateTo<PagesGradesViewer, ViewModelsGradesViewer>(); }
    private void OnManageUsers() => MessageBox.Show($"Manage users Page do not implemented", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    private void OnMarkAbsence() { LoadData(); _serviceNavigation.NavigateTo<PagesMarkAbsences, ViewModelsMarkAbsences>(); }
    private void OnGoToHome() { LoadData(); _viewModelProfile.ResetEditingState(); _serviceNavigation.NavigateTo<PagesHome>(); }
}