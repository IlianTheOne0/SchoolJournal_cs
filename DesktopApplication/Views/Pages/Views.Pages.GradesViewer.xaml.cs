namespace DesktopApplication.Views.Pages;

using DesktopApplication.ViewModels.GradesViewer;
using DesktopApplication.Views.UserControls;

using System.Windows;
using System.Windows.Controls;

public partial class PagesGradesViewer : UserControl
{
    public PagesGradesViewer(ViewModelsGradesViewer ViewModel, UserControlsSidebarMenu Sidebar)
    {
        InitializeComponent();

        try { DataContext = ViewModel; SidebarHost.Content = Sidebar; }
        catch (Exception E) { MessageBox.Show($"Error initializing Grade Viewer: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
}