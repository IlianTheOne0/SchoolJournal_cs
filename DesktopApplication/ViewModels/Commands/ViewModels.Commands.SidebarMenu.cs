namespace DesktopApplication.ViewModels.SidebarMenu;

using CommunityToolkit.Mvvm.Input;
using DesktopApplication.ViewModels.GradesAssigner;
using DesktopApplication.ViewModels.GradesViewer;
using DesktopApplication.ViewModels.Profile;
using DesktopApplication.Views.Pages;
using System.Windows.Input;

public partial class ViewModelsSidebarMenu
{
    public ICommand CommandLogOut { get; private set; }
    public ICommand CommandProfile { get; private set; }
    public ICommand CommandGrade { get; private set; }
    public ICommand CommandViewGrades { get; private set; }
    public ICommand CommandManage { get; private set; }
    public ICommand CommandGoToHome { get; private set; }

    private void InitializeComamnds()
    {
        CommandLogOut = new AsyncRelayCommand(OnLogOut);
        CommandProfile = new RelayCommand(OnProfile);
        CommandGrade = new RelayCommand(OnAssignGrades);
        CommandViewGrades = new RelayCommand(OnViewGrades);
        CommandManage = new RelayCommand(OnManage);
        CommandGoToHome = new RelayCommand(OnGoToHome);
    }

    private void OnProfile() { LoadData(); _serviceNavigation.NavigateTo<PagesProfile, ViewModelsProfile>(); }
    private void OnAssignGrades() { LoadData(); _viewModelGradesAssigner.Load(); _serviceNavigation.NavigateTo<PagesGradesAssigner, ViewModelsGradesAssigner>(); }
    private void OnViewGrades() { LoadData(); _viewModelGradesViewer.Reset(); _serviceNavigation.NavigateTo<PagesGradesViewer, ViewModelsGradesViewer>(); }
    private async void OnManage() { LoadData(); await _viewModelManagement.LoadData(); _viewModelManagement.HardReset(); _serviceNavigation.NavigateTo<PagesManagement>(); }
    private void OnGoToHome() { LoadData(); _viewModelProfile.ResetEditingState(); _serviceNavigation.NavigateTo<PagesHome>(); }
}