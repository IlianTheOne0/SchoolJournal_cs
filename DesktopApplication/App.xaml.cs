namespace DesktopApplication;

using DesktopApplication.Services.Auth;
using DesktopApplication.Services.Converters;
using DesktopApplication.Services.Navigation;
using DesktopApplication.Services.Supabase;

using DesktopApplication.ViewModels.Home;
using DesktopApplication.ViewModels.Login;
using DesktopApplication.ViewModels.SidebarMenu;

using DesktopApplication.Views.UserControls;
using DesktopApplication.Views.Pages;

using Microsoft.Extensions.DependencyInjection;
using System.Windows;

public partial class App : Application
{
    private IServiceProvider? _serviceProvider;

    protected override void OnStartup(StartupEventArgs e)
    {
        ServiceCollection serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        _serviceProvider = serviceCollection.BuildServiceProvider();

        MainWindow mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging();

        // Navigation service
        services.AddSingleton<ServicesNavigation>();

        // ViewModels
        services.AddSingleton<ViewModelsLogin>(
            provider => new ViewModelsLogin(
                ServiceAuth: provider.GetRequiredService<ServicesAuth>()
            )
        );
        services.AddSingleton<ViewModelsSidebarMenu>(
            provider => new ViewModelsSidebarMenu(
                ServiceAuth: provider.GetRequiredService<ServicesAuth>()
            )
        );
        services.AddSingleton<ViewModelsHome>();

        // Services
        services.AddSingleton<ServicesSupabase>();
        services.AddSingleton<ServicesAuth>(
            provider =>
            {
                ServicesSupabase serviceSupabase = provider.GetRequiredService<ServicesSupabase>();
                return new ServicesAuth(serviceSupabase.RepositorySupabase);
            }
        );
        services.AddSingleton<ServicesConvertersBooleanToVisibility>();
        services.AddSingleton<ServicesConvertersUriValidationConverter>();

        // User Controls
        services.AddSingleton<UserControlsSidebarMenu>(
            provider => new UserControlsSidebarMenu(
                ViewModel: provider.GetRequiredService<ViewModelsSidebarMenu>()
            )
        );

        // Pages
        services.AddSingleton<PageLogin>(
            provider => new PageLogin(
                ViewModel: provider.GetRequiredService<ViewModelsLogin>()
            )
        );
        services.AddSingleton<PageHome>(
            provider => new PageHome(
                ViewModel: provider.GetRequiredService<ViewModelsHome>(),
                Sidebar: provider.GetRequiredService<UserControlsSidebarMenu>()
            )
        );

        // Main Window
        services.AddSingleton<MainWindow>(
            provider =>
            {
                ServicesNavigation serviceNavigation = provider.GetRequiredService<ServicesNavigation>();
                ServicesAuth serviceAuth = provider.GetRequiredService<ServicesAuth>();
                return new MainWindow(serviceNavigation, serviceAuth);
            }
        );
    }

    private void OnExit(object sender, ExitEventArgs e)
    {
        if (_serviceProvider is IDisposable disposable) { disposable.Dispose(); }
    }
}
