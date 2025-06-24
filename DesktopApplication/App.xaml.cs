namespace DesktopApplication;

using Database.Interfaces.Repositories.Grade;
using Database.Repositories.Grades;
using DesktopApplication.Interfaces.Services.Auth;
using DesktopApplication.Interfaces.Services.Grades;
using DesktopApplication.Interfaces.Services.Navigation;
using DesktopApplication.Interfaces.Services.Strategies.AccessStrategy;
using DesktopApplication.Interfaces.Services.User;

using DesktopApplication.Services;
using DesktopApplication.Services.Auth;
using DesktopApplication.Services.Converters;
using DesktopApplication.Services.Grades;
using DesktopApplication.Services.Navigation;
using DesktopApplication.Services.Supabase;

using DesktopApplication.ViewModels.GradeViewer;
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

    private void ConfigureServices(IServiceCollection Services)
    {
        Services.AddLogging();

        // Navigation service
        Services.AddSingleton<InterfacesServicesNavigation, ServicesNavigation>();

        LoadServices(Services);
        LoadViewModels(Services);
        LoadUserControls(Services);
        LoadPages(Services);

        // Main Window
        Services.AddSingleton<MainWindow>(
            provider =>
            {
                ServicesNavigation serviceNavigation = (ServicesNavigation)provider.GetRequiredService<InterfacesServicesNavigation>();
                ServicesAuth serviceAuth = (ServicesAuth)provider.GetRequiredService<InterfacesServicesAuth>();
                return new MainWindow(serviceNavigation, serviceAuth);
            }
        );
    }

    private void OnExit(object sender, ExitEventArgs e)
    {
        if (_serviceProvider is IDisposable disposable) { disposable.Dispose(); }
    }

    private void LoadServices(IServiceCollection Services)
    {
        Services.AddSingleton<ServicesSupabase>();
        Services.AddSingleton<InterfacesServicesUser, ServicesUser>(
            provider =>
            {
                ServicesSupabase serviceSupabase = provider.GetRequiredService<ServicesSupabase>();
                return new ServicesUser(serviceSupabase.RepositorySupabase);
            }
        );
        Services.AddSingleton<InterfacesServicesAuth, ServicesAuth>(
            provider =>
            {
                ServicesSupabase serviceSupabase = provider.GetRequiredService<ServicesSupabase>();
                ServicesUser servicesUser = (ServicesUser)provider.GetRequiredService<InterfacesServicesUser>();
                return new ServicesAuth(serviceSupabase.RepositorySupabase, servicesUser);
            }
        );
        Services.AddSingleton<InterfacesServicesGrades, ServicesGrade>(
            provider =>
            {

                InterfacesRepositoriesGrades repositoryGrades = new RepositoriesGrades()
                InterfacesAccessStrategy accessStrategy = provider.GetRequiredService<InterfacesAccessStrategy>();
                return new ServicesGrade(repositoryGrades, accessStrategy);
            }
        );
        Services.AddSingleton<ServicesConverterBooleanToBorderBrush>();
        Services.AddSingleton<ServicesConvertersBooleanToVisibility>();
        Services.AddSingleton<ServicesConvertersBoolToGender>();
        Services.AddSingleton<ServicesConvertersUriValidationConverter>();
    }

    private void LoadViewModels(IServiceCollection Services)
    {
        Services.AddSingleton<ViewModelsLogin>(
            provider => new ViewModelsLogin(
                ServiceAuth: (ServicesAuth)provider.GetRequiredService<InterfacesServicesAuth>()
            )
        );
        Services.AddSingleton<ViewModelsProfile>(
            provider => new ViewModelsProfile(
                ServiceUser: (ServicesUser)provider.GetRequiredService<InterfacesServicesUser>()
            )
        );
        Services.AddSingleton<ViewModelsSidebarMenu>(
            provider => new ViewModelsSidebarMenu(
                ServiceAuth: (ServicesAuth)provider.GetRequiredService<InterfacesServicesAuth>(),
                ServiceNavigation: (ServicesNavigation)provider.GetRequiredService<InterfacesServicesNavigation>(),
                ViewModelProvider: provider.GetRequiredService<ViewModelsProfile>(),
                ServiceUser: (ServicesUser)provider.GetRequiredService<InterfacesServicesUser>()
            )
        );
        Services.AddSingleton<ViewModelsGradeViewer>();
    }

    private void LoadUserControls(IServiceCollection services)
    {
        services.AddTransient<UserControlsSidebarMenu>(
            provider => new UserControlsSidebarMenu(
                ViewModel: provider.GetRequiredService<ViewModelsSidebarMenu>()
            )
        );
    }

    private void LoadPages(IServiceCollection Services)
    {
        Services.AddTransient<PagesLogin>(
            provider => new PagesLogin(
                ViewModel: provider.GetRequiredService<ViewModelsLogin>()
            )
        );
        Services.AddTransient<PagesHome>(
            provider => new PagesHome(
                Sidebar: provider.GetRequiredService<UserControlsSidebarMenu>()
            )
        );
        Services.AddTransient<PagesProfile>(
            provider => new PagesProfile(
                ViewModel: provider.GetRequiredService<ViewModelsProfile>(),
                Sidebar: provider.GetRequiredService<UserControlsSidebarMenu>()
            )
        );
        Services.AddTransient<PagesGradeViewer>(
            provider => new PagesGradeViewer(
                ViewModel: provider.GetRequiredService<ViewModelsGradeViewer>(),
                UserControlSidebarMenu: provider.GetRequiredService<UserControlsSidebarMenu>()
            )
        );
    }
}
