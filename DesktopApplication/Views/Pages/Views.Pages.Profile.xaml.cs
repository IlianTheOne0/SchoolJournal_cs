namespace DesktopApplication.Views.Pages;

using DesktopApplication.ViewModels.Profile;
using DesktopApplication.Views.UserControls;

using System.Windows;
using System.Windows.Controls;

public partial class PagesProfile : UserControl
{
    public ViewModelsProfile _viewModel;

    public PagesProfile(ViewModelsProfile ViewModel, UserControlsSidebarMenu Sidebar)
    {
        InitializeComponent();
        _viewModel = ViewModel;

        try { DataContext = ViewModel; SidebarHost.Content = Sidebar; Loaded += PageProfile_Loaded; }
        catch (Exception e) { MessageBox.Show($"Error initializing Profile: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private async void PageProfile_Loaded(object sender, RoutedEventArgs e) { await _viewModel.LoadDataAsync(); }
}