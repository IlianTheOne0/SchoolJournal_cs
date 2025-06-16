namespace DesktopApplication;

using DesktopApplication.Interfaces.Auth;
using DesktopApplication.Interfaces.Supabase;

using DesktopApplication.Services.Auth;
using DesktopApplication.Services.Supabase;
using DesktopApplication.Services.Navigation;

using DesktopApplication.ViewModels.Login;
using DesktopApplication.ViewModels.Home;

using DesktopApplication.Views;

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

        services.AddSingleton<ServicesNavigation>();

        services.AddSingleton<ViewModelsLogin>();
        services.AddSingleton<ViewModelsHome>();

        //services.AddSingleton<ServicesSupabase>();
        //services.AddSingleton<ServicesAuth>();

        services.AddSingleton<LoginUserControl>();
        services.AddSingleton<HomeUserControl>();

        services.AddSingleton<MainWindow>();
    }

    private void OnExit(object sender, ExitEventArgs e)
    {
        if (_serviceProvider is IDisposable disposable) { disposable.Dispose(); }
    }
}
