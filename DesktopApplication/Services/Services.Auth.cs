namespace DesktopApplication.Services.Auth;

using Database.Interfaces.Repositories.Supabase;
using Database.Repositories.Supabase;
using DesktopApplication.Interfaces.Services.Auth;
using DesktopApplication.Interfaces.Services.User;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public partial class ServicesAuth : INotifyPropertyChanged, InterfacesServicesAuth
{
    private bool _isLoggedIn;
    bool InterfacesServicesAuth.IsLoggedIn { get => _isLoggedIn; set => IsLoggedIn = value; }
    public bool IsLoggedIn { get => _isLoggedIn; private set { if (_isLoggedIn != value) { _isLoggedIn = value; OnPropertyChanged("IsLoggedIn"); } } }

    private readonly InterfacesRepositoriesSupabase _repositorySupabase;
    private readonly InterfacesServicesUser _servicesUser;

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public ServicesAuth(RepositoriesSupabase RepositorySupabase, ServicesUser servicesUser)
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
}