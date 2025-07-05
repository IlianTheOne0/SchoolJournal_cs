namespace DesktopApplication.Views.UserControls;

using DesktopApplication.ViewModels.SidebarMenu;

using System.Windows;
using System.Windows.Controls;

public partial class UserControlsSidebarMenu : UserControl
{
    private readonly ViewModelsSidebarMenu _viewModel;

    public UserControlsSidebarMenu(ViewModelsSidebarMenu ViewModel)
    {
        try
        {
            InitializeComponent();
            _viewModel = ViewModel;

            DataContext = _viewModel;
            Loaded += UserControlSidebarMenu_Loaded;
        }
        catch (Exception e) { MessageBox.Show($"Error initializing Home: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    public void LoadData() => _viewModel?.LoadData();

    private void UserControlSidebarMenu_Loaded(object sender, RoutedEventArgs e) => LoadData();
}