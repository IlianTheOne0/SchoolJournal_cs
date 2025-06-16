namespace DesktopApplication.Views;

using DesktopApplication.Services.Navigation;
using DesktopApplication.ViewModels.Home;
using DesktopApplication.ViewModels.Login;
using System.Windows;
using System.Windows.Controls;

public partial class LoginUserControl : UserControl
{
    private readonly ServicesNavigation _servicesNavigation;

    public LoginUserControl(ViewModelsLogin ViewModel, ServicesNavigation ServiceNavigation)
    {
        InitializeComponent();
        try { DataContext = ViewModel; _servicesNavigation = ServiceNavigation; }
        catch (Exception e) { MessageBox.Show($"Error initializing Login: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void OnLogInNavigationClick(object sender, RoutedEventArgs e) => _servicesNavigation.NavigateTo<HomeUserControl, ViewModelsHome>();
}