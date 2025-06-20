namespace DesktopApplication.Services.Auth;

using Database.Repositories.Supabase;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public partial class ServicesAuth(RepositoriesSupabase RepositorySupabase) : INotifyPropertyChanged
{
    private bool _isLoggedIn = RepositorySupabase.IsLoggedIn; public bool IsLoggedIn { get => _isLoggedIn; private set { if (_isLoggedIn != value) { _isLoggedIn = value; OnPropertyChanged("IsLoggedIn"); } } }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public async Task Login(string Username, string Password)
    {
        await RepositorySupabase.Login(Username, Password);

        SetupAccessStrategy();
        IsLoggedIn = RepositorySupabase.IsLoggedIn;
    }

    public async Task Logout()
    {
        await RepositorySupabase.Logout();

        IsLoggedIn = RepositorySupabase.IsLoggedIn;

        _interfacesAccessStrategy = null;
    }
}