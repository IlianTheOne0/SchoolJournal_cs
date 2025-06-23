namespace DesktopApplication.ViewModels.Profile;

using CommunityToolkit.Mvvm.Input;
using DesktopApplication.Services;
using Models.Tables.Users;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Threading.Tasks;

public class ViewModelsProfile : INotifyPropertyChanged
{
    private readonly ServicesUser _userService;
    private ModelsUser? _originalModelUser;
    private ModelsUser? _modelUser;
    private bool _isEditing;

    public event PropertyChangedEventHandler? PropertyChanged;

    public ModelsUser? ModelUser
    {
        get => _modelUser;
        private set { if (_modelUser != value) { _modelUser = value; OnPropertyChanged(); OnPropertyChanged(nameof(AvatarUrl)); } }
    }

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

    public bool IsReadOnly => !IsEditing;
    public bool ShowEditButton => !IsEditing;
    public bool ShowSaveButton => IsEditing;
    public string? AvatarUrl => ModelUser?.AvatarUrl;

    public IAsyncRelayCommand CommandSaveButton { get; }
    public IRelayCommand CommandEditButton { get; }
    public IRelayCommand CommandResetButton { get; }

    public ViewModelsProfile(ServicesUser ServiceUser)
    {
        _userService = ServiceUser;

        CommandEditButton = new RelayCommand(OnEditButton);
        CommandSaveButton = new AsyncRelayCommand(OnSaveButtonAsync);
        CommandResetButton = new RelayCommand(OnResetButton);
    }

    public async Task LoadDataAsync()
    {
        try
        {
            if (_userService.AccessStrategy?.ModelUser == null) { return; }

            var currentUser = await _userService.GetUserById(_userService.AccessStrategy.ModelUser.Id);

            if (currentUser == null) { MessageBox.Show("Failed to load user data", "Error", MessageBoxButton.OK, MessageBoxImage.Error); return; }

            _originalModelUser = currentUser;
            ModelUser = CloneUser(currentUser);
        }
        catch (Exception e) { MessageBox.Show($"Load data failed: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void OnEditButton() => IsEditing = true;

    private async Task OnSaveButtonAsync()
    {
        if (ModelUser == null) { return; }

        try
        {
            await _userService.UpdateUser(ModelUser);
            _originalModelUser = CloneUser(ModelUser);
            IsEditing = false;
        }
        catch (Exception e) { MessageBox.Show($"Save failed: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void OnResetButton()
    {
        if (_originalModelUser != null) { ModelUser = CloneUser(_originalModelUser); }
        IsEditing = false;
    }

    public void ResetEditingState() => OnResetButton();

    private static ModelsUser CloneUser(ModelsUser user) => new()
    {
        Id = user.Id,
        Username = user.Username,
        FullName = user.FullName,
        Email = user.Email,
        PhoneNumber = user.PhoneNumber,
        Sex = user.Sex,
        DateOfBirth = user.DateOfBirth,
        CreatedAt = user.CreatedAt,
        DateOfTheLastUpdate = user.DateOfTheLastUpdate,
        DateOfTheLastVisitToTheJorunal = user.DateOfTheLastVisitToTheJorunal,
        AvatarUrl = user.AvatarUrl,
        StatusId = user.StatusId,
        EducationalInstitutionId = user.EducationalInstitutionId,
        ProfileId = user.ProfileId,
        StatusName = user.StatusName,
        EducationalInstitutionName = user.EducationalInstitutionName
    };

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
}