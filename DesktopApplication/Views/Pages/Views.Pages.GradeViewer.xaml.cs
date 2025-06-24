namespace DesktopApplication.Views.Pages;

using DesktopApplication.ViewModels.GradeViewer;
using DesktopApplication.Views.UserControls;
using System.Windows;
using System.Windows.Controls;

public partial class PagesGradeViewer : UserControl
{
    private ViewModelsGradeViewer _viewModel;
    private UserControlsSidebarMenu _sidebar;

    public PagesGradeViewer(ViewModelsGradeViewer ViewModel, UserControlsSidebarMenu UserControlSidebarMenu)
    {
        InitializeComponent();
        _viewModel = ViewModel; _sidebar = UserControlSidebarMenu;

        try { DataContext = ViewModel; SidebarHost.Content = _sidebar; }
        catch (Exception e) { MessageBox.Show($"Error initializing Profile: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
}
