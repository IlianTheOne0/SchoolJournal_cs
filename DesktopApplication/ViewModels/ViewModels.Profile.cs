namespace DesktopApplication.ViewModels.Profile;

using CommunityToolkit.Mvvm.Input;
using DesktopApplication.Services;
using Models.Tables.Users;
using Models.Tables.Users.Extended;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

public class ViewModelsProfile : INotifyPropertyChanged
{
    private readonly ServicesUser _serviceUser;
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
    public ICommand CommandSaveButton { get; }
    public ICommand CommandEditButton { get; }
    public ICommand CommandResetButton { get; }
    public ICommand CommandUploadButton { get; }

    public ViewModelsProfile(ServicesUser ServiceUser)
    {
        _serviceUser = ServiceUser;

        CommandEditButton = new RelayCommand(OnEditButton);
        CommandSaveButton = new AsyncRelayCommand(OnSaveButtonAsync);
        CommandResetButton = new RelayCommand(OnResetButton);
        CommandUploadButton = new RelayCommand(OnUploadButton);
    }

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

    private void OnEditButton() => IsEditing = true;

    private async Task OnSaveButtonAsync()
    {
        if (ModelUser == null) { MessageBox.Show("No user data to save", "Error", MessageBoxButton.OK, MessageBoxImage.Error); return; }
        try
        {
            if (!string.IsNullOrEmpty(NewAvatarPath))
            {
                await _serviceUser.UpdateAvatar(ModelUser.Id, NewAvatarPath);
                NewAvatarPath = null;
            }

            var userToUpdate = new ModelsUser
            {
                Id = ModelUser.Id,
                Username = ModelUser.Username?.Trim()!,
                FullName = ModelUser.FullName?.Trim()!,
                Email = ModelUser.Email?.Trim()!,
                PhoneNumber = ModelUser.PhoneNumber?.Trim()!,
                Sex = ModelUser.Sex,
                DateOfBirth = ModelUser.DateOfBirth,
                CreatedAt = ModelUser.CreatedAt,
                DateOfTheLastVisitToTheJorunal = ModelUser.DateOfTheLastVisitToTheJorunal,
                AvatarUrl = ModelUser.AvatarUrl,
                StatusId = ModelUser.StatusId,
                EducationalInstitutionId = ModelUser.EducationalInstitutionId,
                ProfileId = ModelUser.ProfileId,
                DateOfTheLastUpdate = DateTime.Now
            };

            if (string.IsNullOrWhiteSpace(userToUpdate.Username)) { MessageBox.Show("Username is required", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            if (string.IsNullOrWhiteSpace(userToUpdate.Email) || !Database.Utilities.EmailValidation.UtilitiesEmailValidation.Execute(userToUpdate.Email)) { MessageBox.Show("Please enter a valid email address", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

            await _serviceUser.UpdateUser(userToUpdate);
            _originalModelUser = CloneUser(ModelUser);
            await LoadDataAsync();
            IsEditing = false;

            MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex) { MessageBox.Show($"Failed to save changes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void OnResetButton()
    {
        if (_originalModelUser != null) { ModelUser = CloneUser(_originalModelUser); NewAvatarPath = null; }
        IsEditing = false;
    }

    private void OnUploadButton()
    {
        if (!IsEditing) { return; }

        var openFileDialog = new Microsoft.Win32.OpenFileDialog
        {
            Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png",
            Title = "Select an avatar image"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            NewAvatarPath = openFileDialog.FileName;
            ModelUser!.AvatarUrl = NewAvatarPath;
            OnPropertyChanged(nameof(AvatarUrl));
        }
    }

    public void ResetEditingState() => OnResetButton();

    private static ModelsUserExtended CloneUser(ModelsUserExtended user) => new()
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