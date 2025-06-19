namespace DesktopApplication.Views;

using DesktopApplication.Services.Navigation;
using DesktopApplication.ViewModels.Home;
using System.Windows;
using System.Windows.Controls;

public partial class HomeUserControl : UserControl
{
    public HomeUserControl(ViewModelsHome ViewModel)
    {
        InitializeComponent();
        try { DataContext = ViewModel; }
        catch (Exception e) { MessageBox.Show($"Error initializing Home: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
}