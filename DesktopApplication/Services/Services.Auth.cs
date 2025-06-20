namespace DesktopApplication.Services.Auth;

using Models.Repositories.Supabase;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public partial class ServicesAuth : INotifyPropertyChanged
{
    private bool _isLoggedIn; public bool IsLoggedIn { get => _isLoggedIn; private set { if (_isLoggedIn != value) { _isLoggedIn = value; OnPropertyChanged("IsLoggedIn"); } } }
    private RepositoriesSupabase _repositorySupabase;

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public ServicesAuth(RepositoriesSupabase RepositorySupabase)
    {
        _repositorySupabase = RepositorySupabase;
        _isLoggedIn = _repositorySupabase.IsLoggedIn;
    }

    public async Task Login(string Username, string Password)
    {
        await _repositorySupabase.Login(Username, Password);

        SetupAccessStrategy();
        IsLoggedIn = _repositorySupabase.IsLoggedIn;
    }

    public async Task Logout()
    {
        await _repositorySupabase.Logout();

        IsLoggedIn = _repositorySupabase.IsLoggedIn;

        _interfacesAccessStrategy = null;
    }
}