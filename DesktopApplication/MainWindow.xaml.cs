namespace DesktopApplication;

using DesktopApplication.Interfaces.Services.Auth;
using DesktopApplication.Interfaces.Services.Navigation;
using DesktopApplication.Services.Auth;
using DesktopApplication.Services.Navigation;
using DesktopApplication.ViewModels.Login;
using DesktopApplication.Views.Pages;

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

public partial class MainWindow : Window
{
    private readonly InterfacesServicesNavigation _serviceNavigation;
    private readonly InterfacesServicesAuth _serviceAuth;

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
        if (e.PropertyName == nameof(InterfacesServicesAuth.IsLoggedIn))
        {
            if (_serviceAuth.IsLoggedIn) { _serviceNavigation.NavigateTo<PagesHome>(); }
            else { _serviceNavigation.NavigateTo<PagesLogin, ViewModelsLogin>(); }
        }
    }

    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e) => _serviceNavigation.NavigateTo<PagesLogin, ViewModelsLogin>();

    private void SetContent(UserControl Content) => this.Content = Content;
}