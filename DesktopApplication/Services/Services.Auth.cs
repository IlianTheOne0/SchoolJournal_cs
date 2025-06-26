namespace DesktopApplication.Services.Auth;

using DesktopApplication.Interfaces.Services.Auth;
using DesktopApplication.Interfaces.Services.User;

using Database.Interfaces.Repositories.Supabase;

using System.ComponentModel;
using System.Runtime.CompilerServices;

public partial class ServicesAuth : INotifyPropertyChanged, InterfacesServicesAuth
{
    private readonly InterfacesRepositoriesSupabase _repositorySupabase;
    private readonly InterfacesServicesUser _servicesUser;

    private bool _isLoggedIn;
    bool InterfacesServicesAuth.IsLoggedIn { get => _isLoggedIn; set => IsLoggedIn = value; }
    public bool IsLoggedIn { get => _isLoggedIn; private set { if (_isLoggedIn != value) { _isLoggedIn = value; OnPropertyChanged("IsLoggedIn"); } } }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ServicesAuth(InterfacesRepositoriesSupabase RepositorySupabase, InterfacesServicesUser servicesUser)
    {
        _repositorySupabase = RepositorySupabase;
        _servicesUser = servicesUser;
        
        _isLoggedIn = _repositorySupabase.IsLoggedIn;
    }

    public async Task Login(string Username, string Password)
    {
        await _repositorySupabase.Login(Username, Password);

        if (_repositorySupabase.ModelUser != null) { _servicesUser.SetupAccessStrategy(_repositorySupabase.ModelUser); }

        IsLoggedIn = _repositorySupabase.IsLoggedIn;
    }

    public async Task Logout()
    {
        await _repositorySupabase.Logout();
        
        _servicesUser.ClearAccessStrategy();
        IsLoggedIn = _repositorySupabase.IsLoggedIn;
    }

    private void OnPropertyChanged([CallerMemberName] string PropertyName = null!) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
}