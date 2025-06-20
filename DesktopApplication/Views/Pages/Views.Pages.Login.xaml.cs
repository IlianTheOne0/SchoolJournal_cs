namespace DesktopApplication.Views.Pages;

using DesktopApplication.ViewModels.Login;
using System.Windows;
using System.Windows.Controls;

public partial class PageLogin : UserControl
{
    public PageLogin(ViewModelsLogin ViewModel)
    {
        InitializeComponent();
        try { DataContext = ViewModel; }
        catch (Exception e) { MessageBox.Show($"Error initializing Login: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
}