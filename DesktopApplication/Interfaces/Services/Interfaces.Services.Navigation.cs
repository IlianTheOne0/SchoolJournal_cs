namespace DesktopApplication.Interfaces.Services.Navigation;

using System.Windows.Controls;

public interface InterfacesServicesNavigation
{
    event Action<UserControl>? OnNavigate;
    event Action<Type>? OnPageChanged;

    void NavigateTo<TView, TViewModel>()
        where TView : UserControl
        where TViewModel : class;
    void NavigateTo<TView>()
       where TView : UserControl;
}