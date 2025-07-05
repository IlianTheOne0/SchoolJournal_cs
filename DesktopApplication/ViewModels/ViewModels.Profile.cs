namespace DesktopApplication.ViewModels.Profile;

using DesktopApplication.Interfaces.Services.User;

using Models.Tables.Users;

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

public partial class ViewModelsProfile : INotifyPropertyChanged
{
    private readonly InterfacesServicesUser _serviceUser;
    private ModelsUserExtended? _originalModelUser;
    private ModelsUserExtended? _modelUser;
    public ModelsUserExtended? ModelUser
    {
        get => _modelUser;
        private set { if (_modelUser != value) { _modelUser = value; OnPropertyChanged(); OnPropertyChanged(nameof(AvatarUrl)); } }
    }

    private bool _isEditing;
    public bool IsEditing
    {
        get => _isEditing;
        private set
        {
            if (_isEditing != value)
            {
                _isEditing = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(IsReadOnly));
                OnPropertyChanged(nameof(ShowEditButton));
                OnPropertyChanged(nameof(ShowSaveButton));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public bool IsReadOnly => !IsEditing;
    public bool ShowEditButton => !IsEditing;
    public bool ShowSaveButton => IsEditing;
    public string? AvatarUrl => ModelUser?.AvatarUrl;
    private string? _newAvatarPath; public string? NewAvatarPath { get => _newAvatarPath; set { _newAvatarPath = value; OnPropertyChanged(); } }

    public ViewModelsProfile(InterfacesServicesUser ServiceUser) { _serviceUser = ServiceUser; InitializeComamnds(); }

    public async Task LoadDataAsync()
    {
        try
        {
            if (_serviceUser.AccessStrategy?.ModelUser == null) { return; }

            var currentUser = await _serviceUser.GetUserById(_serviceUser.AccessStrategy.ModelUser.Id);

            if (currentUser == null) { MessageBox.Show("Failed to load user data", "Error", MessageBoxButton.OK, MessageBoxImage.Error); return; }

            _originalModelUser = currentUser;
            ModelUser = CloneUser(currentUser);
        }
        catch (Exception e) { MessageBox.Show($"Load data failed: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    public void ResetEditingState() => OnResetButton();

    private static ModelsUserExtended CloneUser(ModelsUserExtended User) => new()
    {
        Id = User.Id,
        Username = User.Username,
        FullName = User.FullName,
        Email = User.Email,
        PhoneNumber = User.PhoneNumber,
        Sex = User.Sex,
        DateOfBirth = User.DateOfBirth,
        CreatedAt = User.CreatedAt,
        DateOfTheLastUpdate = User.DateOfTheLastUpdate,
        DateOfTheLastVisitToTheJournal = User.DateOfTheLastVisitToTheJournal,
        AvatarUrl = User.AvatarUrl,
        StatusId = User.StatusId,
        EducationalInstitutionId = User.EducationalInstitutionId,
        ProfileId = User.ProfileId,
        StatusName = User.StatusName,
        EducationalInstitutionName = User.EducationalInstitutionName
    };

    protected virtual void OnPropertyChanged([CallerMemberName] string? PropertyName = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName)); }
}