namespace DesktopApplication;

using Database.Repositories.Supabase;
using DesktopApplication.Services;
using DesktopApplication.Services.Auth;
using DesktopApplication.Services.Converters;
using DesktopApplication.Services.Navigation;
using DesktopApplication.Services.Supabase;
using DesktopApplication.ViewModels.Login;
using DesktopApplication.ViewModels.Profile;
using DesktopApplication.ViewModels.SidebarMenu;
using DesktopApplication.Views.Pages;
using DesktopApplication.Views.UserControls;
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
        services.AddSingleton<ViewModelsProfile>(
            provider => new ViewModelsProfile(
                ServiceUser: provider.GetRequiredService<ServicesUser>()
            )
        );
        services.AddSingleton<ViewModelsSidebarMenu>(
            provider => new ViewModelsSidebarMenu(
                ServiceAuth: provider.GetRequiredService<ServicesAuth>(),
                ServiceNavigation: provider.GetRequiredService<ServicesNavigation>(),
                ViewModelProvider: provider.GetRequiredService<ViewModelsProfile>(),
                ServiceUser: provider.GetRequiredService<ServicesUser>()
            )
        );

        // Services
        services.AddSingleton<ServicesSupabase>();
        services.AddSingleton<ServicesUser>(
            provider =>
            {
                ServicesSupabase serviceSupabase = provider.GetRequiredService<ServicesSupabase>();
                return new ServicesUser(serviceSupabase.RepositorySupabase);
            }
        );
        services.AddSingleton<ServicesAuth>(
            provider =>
            {
                ServicesSupabase serviceSupabase = provider.GetRequiredService<ServicesSupabase>();
                ServicesUser servicesUser = provider.GetRequiredService<ServicesUser>();
                return new ServicesAuth(serviceSupabase.RepositorySupabase, servicesUser);
            }
        );
        services.AddSingleton<ServicesConverterBooleanToBorderBrush>();
        services.AddSingleton<ServicesConvertersBooleanToVisibility>();
        services.AddSingleton<ServicesConvertersBoolToGender>();
        services.AddSingleton<ServicesConvertersUriValidationConverter>();

        // User Controls
        services.AddTransient<UserControlsSidebarMenu>(
            provider => new UserControlsSidebarMenu(
                ViewModel: provider.GetRequiredService<ViewModelsSidebarMenu>()
            )
        );

        // Pages
        services.AddTransient<PageLogin>(
            provider => new PageLogin(
                ViewModel: provider.GetRequiredService<ViewModelsLogin>()
            )
        );
        services.AddTransient<PageHome>(
            provider => new PageHome(
                Sidebar: provider.GetRequiredService<UserControlsSidebarMenu>()
            )
        );
        services.AddTransient<PageProfile>(
            provider => new PageProfile(
                ViewModel: provider.GetRequiredService<ViewModelsProfile>(),
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
