namespace DesktopApplication.Views.UserControls;

using DesktopApplication.ViewModels.SidebarMenu;
using System.Windows;
using System.Windows.Controls;

public partial class UserControlsSidebarMenu : UserControl
{
    public ViewModelsSidebarMenu _viewModel;

    public UserControlsSidebarMenu(ViewModelsSidebarMenu ViewModel)
    {
        InitializeComponent();
        _viewModel = ViewModel;

        try { DataContext = ViewModel; Loaded += UserControlSidebarMenu_Loaded; }
        catch (Exception e) { MessageBox.Show($"Error initializing Home: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    public UserControlsSidebarMenu() => InitializeComponent();

    private void UserControlSidebarMenu_Loaded(object sender, RoutedEventArgs e) { _viewModel.LoadDataAsync(); }
}