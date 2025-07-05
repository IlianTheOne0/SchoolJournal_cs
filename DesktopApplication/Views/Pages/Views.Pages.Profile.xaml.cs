namespace DesktopApplication.Views.Pages;

using DesktopApplication.ViewModels.Profile;
using DesktopApplication.Views.UserControls;

using System.Windows;
using System.Windows.Controls;

public partial class PagesProfile : UserControl
{
    private readonly ViewModelsProfile _viewModel;

    public PagesProfile(ViewModelsProfile ViewModel, UserControlsSidebarMenu Sidebar)
    {
        InitializeComponent();
        _viewModel = ViewModel;

        try { DataContext = ViewModel; SidebarHost.Content = Sidebar; Loaded += PageProfile_Loaded; }
        catch (Exception E) { MessageBox.Show($"Error initializing Profile: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private async void PageProfile_Loaded(object Sender, RoutedEventArgs E) { await _viewModel.LoadDataAsync(); }
}