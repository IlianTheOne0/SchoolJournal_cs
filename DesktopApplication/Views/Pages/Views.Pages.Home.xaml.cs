namespace DesktopApplication.Views.Pages;

using DesktopApplication.Services.Navigation;
using DesktopApplication.ViewModels.Home;
using DesktopApplication.Views.UserControls;
using System.Windows;
using System.Windows.Controls;

public partial class PageHome : UserControl
{
    private readonly ViewModelsHome _viewModel;
    private readonly UserControlsSidebarMenu _sidebar;

    public PageHome(ViewModelsHome ViewModel, UserControlsSidebarMenu Sidebar)
    {
        InitializeComponent();
        _viewModel = ViewModel;
        _sidebar = Sidebar;

        try { DataContext = ViewModel; SidebarHost.Content = _sidebar; }
        catch (Exception e) { MessageBox.Show($"Error initializing Home: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
}