namespace DesktopApplication.Services.Navigation;

using System.Windows.Controls;

public class ServicesNavigation
{
    public event Action<UserControl>? OnNavigate;

    public void NavigateTo(UserControl View) => OnNavigate?.Invoke(View);
}