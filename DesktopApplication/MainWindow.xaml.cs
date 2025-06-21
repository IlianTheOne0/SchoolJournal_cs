namespace DesktopApplication;

using DesktopApplication.Services.Auth;
using DesktopApplication.Services.Navigation;
using DesktopApplication.ViewModels.Login;
using DesktopApplication.Views.Pages;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

public partial class MainWindow : Window
{
    private readonly ServicesNavigation _serviceNavigation;
    private readonly ServicesAuth _serviceAuth;

    public MainWindow(ServicesNavigation ServicesNavigation, ServicesAuth authService)
    {
        InitializeComponent();
        
        _serviceNavigation = ServicesNavigation;
        _serviceAuth = authService;

        _serviceNavigation.OnNavigate += SetContent;
        _serviceAuth.PropertyChanged += OnAuthServicePropertyChanged!;
    }

    public void OnAuthServicePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ServicesAuth.IsLoggedIn))
        {
            if (_serviceAuth.IsLoggedIn) { _serviceNavigation.NavigateTo<PageHome>(); }
            else { _serviceNavigation.NavigateTo<PageLogin, ViewModelsLogin>(); }
        }
    }

    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e) => _serviceNavigation.NavigateTo<PageLogin, ViewModelsLogin>();

    private void SetContent(UserControl Content) => this.Content = Content;
}