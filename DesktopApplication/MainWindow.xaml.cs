namespace DesktopApplication;

using DesktopApplication.Services.Auth;
using DesktopApplication.Services.Navigation;
using DesktopApplication.ViewModels.Home;
using DesktopApplication.ViewModels.Login;
using DesktopApplication.Views;
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
        if (e.PropertyName == nameof(ServicesAuth.IsLoggedIn) && _serviceAuth.IsLoggedIn) { _serviceNavigation.NavigateTo<HomeUserControl, ViewModelsHome>(); }
        if (e.PropertyName == nameof(ServicesAuth.IsLoggedIn) && !_serviceAuth.IsLoggedIn) { _serviceNavigation.NavigateTo<LoginUserControl, ViewModelsLogin>(); }
    }

    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (_serviceAuth.IsLoggedIn) { _serviceNavigation.NavigateTo<HomeUserControl, ViewModelsHome>(); }
        else { _serviceNavigation.NavigateTo<LoginUserControl, ViewModelsLogin>(); }
    }

    private void SetContent(UserControl Content) => this.Content = Content;
}