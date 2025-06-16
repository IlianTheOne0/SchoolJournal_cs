namespace DesktopApplication;

using DesktopApplication.ViewModels.Interface;
using DesktopApplication.Views;
using System.Windows;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        this.Content = new Login((IViewModels)Application.Current.Resources["ViewModelsLogin"]);
    }
}