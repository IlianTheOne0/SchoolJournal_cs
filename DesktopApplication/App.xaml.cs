namespace DesktopApplication;

using DesktopApplication.Interfaces.Services.Auth;
using DesktopApplication.Interfaces.Services.Grades;
using DesktopApplication.Interfaces.Services.Management;
using DesktopApplication.Interfaces.Services.Navigation;
using DesktopApplication.Interfaces.Services.Supabase;
using DesktopApplication.Interfaces.Services.User;

using DesktopApplication.Services;
using DesktopApplication.Services.Auth;
using DesktopApplication.Services.Converters;
using DesktopApplication.Services.Grades;
using DesktopApplication.Services.Management;
using DesktopApplication.Services.Navigation;
using DesktopApplication.Services.Supabase;

using DesktopApplication.ViewModels.GradesAssigner;
using DesktopApplication.ViewModels.GradesViewer;
using DesktopApplication.ViewModels.Login;
using DesktopApplication.ViewModels.Management;
using DesktopApplication.ViewModels.Profile;
using DesktopApplication.ViewModels.SidebarMenu;

using DesktopApplication.Views.Pages;
using DesktopApplication.Views.UserControls;

using Microsoft.Extensions.DependencyInjection;
using System.Windows;

public partial class App : Application
{
    private IServiceProvider? _serviceProvider;

    protected override void OnStartup(StartupEventArgs E)
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
                InterfacesServicesNavigation serviceNavigation = provider.GetRequiredService<InterfacesServicesNavigation>();
                InterfacesServicesAuth serviceAuth = provider.GetRequiredService<InterfacesServicesAuth>();
                return new MainWindow(serviceNavigation, serviceAuth);
            }
        );
    }

    private void OnExit(object Sender, ExitEventArgs E)
    {
        if (_serviceProvider is IDisposable disposable) { disposable.Dispose(); }
    }

    private void LoadServices(IServiceCollection Services)
    {
        Services.AddSingleton<InterfacesServicesSupabase, ServicesSupabase>();
        Services.AddSingleton<InterfacesServicesUser, ServicesUser>(
            provider =>
            {
                InterfacesServicesSupabase serviceSupabase = provider.GetRequiredService<InterfacesServicesSupabase>();
                return new ServicesUser(serviceSupabase.RepositorySupabase);
            }
        );
        Services.AddSingleton<InterfacesServicesAuth, ServicesAuth>(
            provider =>
            {
                InterfacesServicesSupabase serviceSupabase = provider.GetRequiredService<InterfacesServicesSupabase>();
                InterfacesServicesUser servicesUser = provider.GetRequiredService<InterfacesServicesUser>();
                return new ServicesAuth(serviceSupabase.RepositorySupabase, servicesUser);
            }
        );
        Services.AddSingleton<InterfacesServicesGrades, ServicesGrades>(
            provider => new ServicesGrades(
                ServiceSupabase: provider.GetRequiredService<InterfacesServicesSupabase>(),
                ServiceUser: provider.GetRequiredService<InterfacesServicesUser>()
            )
        );
        Services.AddSingleton<InterfacesServicesManagement, ServicesManagement>(
            provider => new ServicesManagement(
                ServiceSupabase: provider.GetRequiredService<InterfacesServicesSupabase>(),
                ServiceUser: provider.GetRequiredService<InterfacesServicesUser>()
            )
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
                ServiceAuth: provider.GetRequiredService<InterfacesServicesAuth>()
            )
        );
        Services.AddSingleton<ViewModelsProfile>(
            provider => new ViewModelsProfile(
                ServiceUser: provider.GetRequiredService<InterfacesServicesUser>()
            )
        );
        Services.AddSingleton<ViewModelsSidebarMenu>(
            provider => new ViewModelsSidebarMenu(
                ServiceAuth: provider.GetRequiredService<InterfacesServicesAuth>(),
                ServiceNavigation: provider.GetRequiredService<InterfacesServicesNavigation>(),
                ServiceUser: provider.GetRequiredService<InterfacesServicesUser>(),
                ViewModelProvider: provider.GetRequiredService<ViewModelsProfile>(),
                ViewModelGradesViewer: provider.GetRequiredService<ViewModelsGradesViewer>(),
                ViewModelGradesAssigner: provider.GetRequiredService<ViewModelsGradesAssigner>(),
                ViewModelManagement: provider.GetRequiredService<ViewModelsManagement>()
            )
        );
        Services.AddSingleton<ViewModelsGradesViewer>(
            provider => new ViewModelsGradesViewer(
                ServiceGrades: provider.GetRequiredService<InterfacesServicesGrades>()
            )
        );
        Services.AddSingleton<ViewModelsGradesAssigner>(
            provider => new ViewModelsGradesAssigner(
                ServiceGrades: provider.GetRequiredService<InterfacesServicesGrades>()
            )
        );
        Services.AddSingleton<ViewModelsManagement>(
            provider => new ViewModelsManagement(
                ServiceGrades: provider.GetRequiredService<InterfacesServicesManagement>()
            )
        );
    }

    private void LoadUserControls(IServiceCollection Services)
    {
        Services.AddTransient<UserControlsSidebarMenu>(
            provider => new UserControlsSidebarMenu(
                ViewModel: provider.GetRequiredService<ViewModelsSidebarMenu>()
            )
        );
        Services.AddTransient<UserControlsManagementComboBoxes>(
            provider => new UserControlsManagementComboBoxes(
                ViewModel: provider.GetRequiredService<ViewModelsManagement>()
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
        Services.AddTransient<PagesGradesViewer>(
            provider => new PagesGradesViewer(
                ViewModel: provider.GetRequiredService<ViewModelsGradesViewer>(),
                Sidebar: provider.GetRequiredService<UserControlsSidebarMenu>()
            )
        );
        Services.AddTransient<PagesGradesAssigner>(
            provider => new PagesGradesAssigner(
                ViewModel: provider.GetRequiredService<ViewModelsGradesAssigner>(),
                Sidebar: provider.GetRequiredService<UserControlsSidebarMenu>()
            )
        );
        Services.AddTransient<PagesManagement>(
            provider => new PagesManagement(
                ViewModel: provider.GetRequiredService<ViewModelsManagement>(),
                Sidebar: provider.GetRequiredService<UserControlsSidebarMenu>(),
                Comboboxes: provider.GetRequiredService<UserControlsManagementComboBoxes>()
            )
        );
    }
}
