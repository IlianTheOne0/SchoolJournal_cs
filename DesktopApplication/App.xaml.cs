namespace DesktopApplication;

using DesktopApplication.Services.Auth;
using DesktopApplication.Services.Navigation;
using DesktopApplication.Services.Supabase;
using DesktopApplication.ViewModels.Home;
using DesktopApplication.ViewModels.Login;
using DesktopApplication.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Windows;
using System.Windows.Navigation;

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
        services.AddSingleton<ViewModelsHome>(
            provider => new ViewModelsHome(
                ServiceAuth: provider.GetRequiredService<ServicesAuth>()
            )
        );

        // Services
        services.AddSingleton<ServicesSupabase>();
        services.AddSingleton<ServicesAuth>(
            provider =>
            {
                ServicesSupabase serviceSupabase = provider.GetRequiredService<ServicesSupabase>();
                return new ServicesAuth(serviceSupabase.RepositorySupabase);
            }
        );

        // User Controls
        services.AddSingleton<LoginUserControl>(
            provider => new LoginUserControl(
                ViewModel: provider.GetRequiredService<ViewModelsLogin>()
            )
        );
        services.AddSingleton<HomeUserControl>(
            provider => new HomeUserControl(
                ViewModel: provider.GetRequiredService<ViewModelsHome>()
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
