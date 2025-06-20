namespace DesktopApplication.Views;

using DesktopApplication.ViewModels.Home;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

public partial class HomeUserControl : UserControl
{
    private readonly ViewModelsHome _viewModel;

    public HomeUserControl(ViewModelsHome ViewModel)
    {
        InitializeComponent();
        _viewModel = ViewModel;

        try { DataContext = ViewModel; Loaded += HomeUserControl_Loaded; }
        catch (Exception e) { MessageBox.Show($"Error initializing Home: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void HomeUserControl_Loaded(object sender, RoutedEventArgs e) { _viewModel.LoadDataAsync(); }
}