namespace DesktopApplication.Views;

using DesktopApplication.ViewModels.Interface;
using System.Windows;
using System.Windows.Controls;

public partial class Login : UserControl
{
    public Login(IViewModels? ViewModel)
    {
        InitializeComponent();
        this.DataContext = ViewModel;
    }

    private void OnLogInNavigationClick(object sender, RoutedEventArgs e)
    {
        Home homePage = new Home((IViewModels)Application.Current.Resources["ViewModelsHome"]);
        MainWindow mainWindow = (Window.GetWindow(this) as DesktopApplication.MainWindow);
        
        if (mainWindow != null)
        {
            UserControl currentPage = mainWindow.Content as UserControl;
        
            if (currentPage != null) { currentPage.Content = null; }
            mainWindow.Content = homePage;
        }
    }
}