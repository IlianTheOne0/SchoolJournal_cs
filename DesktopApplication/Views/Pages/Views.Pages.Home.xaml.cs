namespace DesktopApplication.Views.Pages;

using DesktopApplication.Views.UserControls;

using System.Windows;
using System.Windows.Controls;

public partial class PagesHome : UserControl
{
    private readonly UserControlsSidebarMenu _sidebar;

    public PagesHome(UserControlsSidebarMenu Sidebar)
    {
        InitializeComponent();
        _sidebar = Sidebar;
        LoadSidebar();
        Loaded += (s, e) => LoadSidebar();

        try { SidebarHost.Content = _sidebar; }
        catch (Exception E) { MessageBox.Show($"Error initializing Home: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void LoadSidebar()
    {
        try { SidebarHost.Content = _sidebar; _sidebar.LoadData(); }
        catch (Exception E) { MessageBox.Show($"Error loading sidebar: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
}