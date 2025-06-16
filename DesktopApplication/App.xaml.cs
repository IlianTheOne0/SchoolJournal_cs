namespace DesktopApplication;

using DesktopApplication.Services.Auth;
using DesktopApplication.Services.Supabase;

using DesktopApplication.ViewModels.Home;
using DesktopApplication.ViewModels.Interface;
using DesktopApplication.ViewModels.Login;

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

        services.AddSingleton<IServicesSupabase, ServicesSupabase>();
        services.AddSingleton<IServicesAuth, ServicesAuth>();

        services.AddSingleton<IViewModels, ViewModelsLogin>();
        services.AddSingleton<IViewModels, ViewModelsHome>();

        services.AddSingleton<MainWindow>();
        services.AddSingleton<Login>();
        services.AddSingleton<Home>();
    }

    private void OnExit(object sender, ExitEventArgs e)
    {
        if (_serviceProvider is IDisposable disposable) { disposable.Dispose(); }
    }
}
