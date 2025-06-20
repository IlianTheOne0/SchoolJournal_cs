namespace DesktopApplication.Services.Navigation;

using Microsoft.Extensions.DependencyInjection;
using Models.BradCrumb;
using System.Windows.Controls;

public class ServicesNavigation
{
    private readonly IServiceProvider _serviceProvider;
    public List<ModelsBreadcrumb> ModelListBreadcrumbs { get; } = new List<ModelsBreadcrumb>();
    
    public event Action<UserControl>? OnNavigate;

    public ServicesNavigation(IServiceProvider ServiceProvider) => _serviceProvider = ServiceProvider;

    public void NavigateTo<TView, TViewModel>()
        where TView : UserControl
        where TViewModel : class
    {
        var view = _serviceProvider.GetRequiredService<TView>();
        var viewModel = _serviceProvider.GetRequiredService<TViewModel>();

        view.DataContext = viewModel;

        var title = typeof(TView).Name.Replace("UserControl", "");

        OnNavigate?.Invoke(view);
    }

    public void NavigateBack(int index)
    {
        if (index < 0 || index >= ModelListBreadcrumbs.Count) { return; }

        ModelListBreadcrumbs.RemoveRange(index + 1, ModelListBreadcrumbs.Count - index - 1);

        var item = ModelListBreadcrumbs[index];

        var view = (UserControl)_serviceProvider.GetRequiredService(item.ViewType);
        view.DataContext = item.ViewModel;

        OnNavigate?.Invoke(view);
    }
}