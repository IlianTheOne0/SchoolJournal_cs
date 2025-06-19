namespace DesktopApplication.Services.Navigation;

using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

public class ServicesNavigation
{
    private readonly IServiceProvider _serviceProvider;
    public event Action<UserControl>? OnNavigate;

    public ServicesNavigation(IServiceProvider ServiceProvider) => _serviceProvider = ServiceProvider;

    public void NavigateTo<TView, TViewModel>()
        where TView : UserControl
        where TViewModel : class
    {
        var view = _serviceProvider.GetRequiredService<TView>();
        var viewModel = _serviceProvider.GetRequiredService<TViewModel>();

        view.DataContext = viewModel;
        OnNavigate?.Invoke(view);
    }
}