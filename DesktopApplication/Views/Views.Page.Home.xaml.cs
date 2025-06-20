namespace DesktopApplication.Views;

using DesktopApplication.Services.Navigation;
using DesktopApplication.ViewModels.Home;
using System.Windows;
using System.Windows.Controls;

public partial class HomePage : UserControl
{
    private readonly ViewModelsHome _viewModel;

    public HomePage(ViewModelsHome ViewModel)
    {
        InitializeComponent();
        _viewModel = ViewModel;

        try { DataContext = ViewModel; Loaded += HomeUserControl_Loaded; }
        catch (Exception e) { MessageBox.Show($"Error initializing Home: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void HomeUserControl_Loaded(object sender, RoutedEventArgs e) { _viewModel.LoadDataAsync(); }
}