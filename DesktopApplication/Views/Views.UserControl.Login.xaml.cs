namespace DesktopApplication.Views;

using DesktopApplication.Services.Navigation;
using DesktopApplication.ViewModels.Home;
using DesktopApplication.ViewModels.Login;
using System.Windows;
using System.Windows.Controls;

public partial class LoginUserControl : UserControl
{
    public LoginUserControl(ViewModelsLogin ViewModel)
    {
        InitializeComponent();
        try { DataContext = ViewModel; }
        catch (Exception e) { MessageBox.Show($"Error initializing Login: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
}