namespace DesktopApplication;

using DesktopApplication.Services.Navigation;
using DesktopApplication.Views;
using System.Windows;
using System.Windows.Controls;

public partial class MainWindow : Window
{
    private readonly ServicesNavigation _serviceNavigation;

    public MainWindow(ServicesNavigation ServicesNavigation)
    {
        InitializeComponent();
        _serviceNavigation = ServicesNavigation;
        _serviceNavigation.OnNavigate += SetContent;
        _serviceNavigation.NavigateTo(new LoginUserControl(_serviceNavigation));
    }

    private void SetContent(UserControl Content) => this.Content = Content;
}