namespace DesktopApplication.Views.Pages;

using DesktopApplication.ViewModels.GradeViewer;
using DesktopApplication.Views.UserControls;

using System.Windows;
using System.Windows.Controls;

public partial class PagesGradeViewer : UserControl
{
    public PagesGradeViewer(ViewModelsGradeViewer ViewModel, UserControlsSidebarMenu UserControlSidebarMenu)
    {
        InitializeComponent();

        try { DataContext = ViewModel; SidebarHost.Content = UserControlSidebarMenu; }
        catch (Exception e) { MessageBox.Show($"Error initializing Grade Viewer: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
}