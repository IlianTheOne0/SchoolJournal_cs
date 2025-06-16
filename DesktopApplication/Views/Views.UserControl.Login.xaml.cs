namespace DesktopApplication.Views;

using DesktopApplication.Services.Navigation;
using System.Windows;
using System.Windows.Controls;

public partial class LoginUserControl : UserControl
{
    private readonly ServicesNavigation _servicesNavigation;

    public LoginUserControl(ServicesNavigation ServiceNavigation)
    {
        InitializeComponent();
        try { _servicesNavigation = ServiceNavigation; }
        catch (Exception e) { MessageBox.Show($"Error initializing Login: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void OnLogInNavigationClick(object sender, RoutedEventArgs e)
    {
        var homeUserControl = new HomeUserControl();
        _servicesNavigation.NavigateTo(homeUserControl);
    }
}