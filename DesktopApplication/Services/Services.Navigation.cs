namespace DesktopApplication.Services.Navigation;

using DesktopApplication.Interfaces.Services.Navigation;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

public class ServicesNavigation : InterfacesServicesNavigation
{
    private readonly IServiceProvider _serviceProvider;
    
    public event Action<UserControl>? OnNavigate;
    public event Action<Type>? OnPageChanged;

    public ServicesNavigation(IServiceProvider ServiceProvider) => _serviceProvider = ServiceProvider;

    public void NavigateTo<TView, TViewModel>(Action<TViewModel>? initialize = null)
        where TView : UserControl
        where TViewModel : class
    {
        var view = _serviceProvider.GetRequiredService<TView>();
        var viewModel = _serviceProvider.GetRequiredService<TViewModel>();

        initialize?.Invoke(viewModel);

        OnNavigate?.Invoke(view);
        OnPageChanged?.Invoke(typeof(TView));
    }

    public void NavigateTo<TView>()
        where TView : UserControl
    {
        var view = _serviceProvider.GetRequiredService<TView>();

        OnNavigate?.Invoke(view);
        OnPageChanged?.Invoke(typeof(TView));
    }
}