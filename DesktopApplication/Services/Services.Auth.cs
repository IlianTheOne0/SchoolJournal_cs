namespace DesktopApplication.Services.Auth;

using Infrastructure.Interfaces.Mediators.Auth;
using DesktopApplication.Interfaces.Services.Auth;
using DesktopApplication.Interfaces.Services.User;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public partial class ServicesAuth : INotifyPropertyChanged, InterfacesServicesAuth
{
    private readonly InterfacesMediatorsAuth _mediatorAuth;
    private readonly InterfacesServicesUser _servicesUser;

    private bool _isLoggedIn;
    bool InterfacesServicesAuth.IsLoggedIn { get => _isLoggedIn; set => IsLoggedIn = value; }
    public bool IsLoggedIn { get => _isLoggedIn; private set { if (_isLoggedIn != value) { _isLoggedIn = value; OnPropertyChanged("IsLoggedIn"); } } }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ServicesAuth(InterfacesMediatorsAuth MediatorAuth, InterfacesServicesUser ServiceUser)
    {
        _mediatorAuth = MediatorAuth; _servicesUser = ServiceUser;

        _isLoggedIn = _mediatorAuth.IsLoggedIn;
    }


    public async Task Login(string Username, string Password)
    {
        try
        {
            await _mediatorAuth.Login(Username, Password);
            IsLoggedIn = _mediatorAuth.IsLoggedIn;

            if (_servicesUser is ServicesUser userService && IsLoggedIn)
            {
                var currentUser = await userService.GetUserById(userService.AccessStrategy?.ModelUser.Id ?? 0);
                if (currentUser != null) { userService.SetupAccessStrategy(currentUser); }
            }
        }
        catch (Exception e) { throw new Exception($"Login failed: {e.Message}", e); }
    }

    public async Task Logout()
    {
        try
        {
            await _mediatorAuth.Logout();
            _servicesUser.ClearAccessStrategy();
            IsLoggedIn = _mediatorAuth.IsLoggedIn;
        }
        catch (Exception e) { throw new Exception($"Logout failed: {e.Message}", e); }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}