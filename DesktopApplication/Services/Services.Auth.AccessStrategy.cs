namespace DesktopApplication.Services.Auth;

using DesktopApplication.Interfaces.Services.Strategies.AccessStrategy;

public partial class ServicesAuth
{
    private InterfacesAccessStrategy? _interfacesAccessStrategy;
    public InterfacesAccessStrategy? AccessStrategy { get => _interfacesAccessStrategy; set => _interfacesAccessStrategy = value; }

    public void SetupAccessStrategy()
    {
        try
        {
            if (_servicesUser is ServicesUser userService && userService.AccessStrategy?.ModelUser != null) { AccessStrategy = userService.AccessStrategy; }
        }
        catch (Exception e) { throw new Exception($"SetupAccessStrategy failed: {e.Message}", e); }
    }
}