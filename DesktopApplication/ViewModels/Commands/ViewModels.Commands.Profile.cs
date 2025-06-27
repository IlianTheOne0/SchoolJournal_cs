namespace DesktopApplication.ViewModels.Profile;

using CommunityToolkit.Mvvm.Input;
using Models.Tables.Users;
using System.Windows;
using System.Windows.Input;

public partial class ViewModelsProfile
{
    public ICommand CommandSaveButton { get; private set; }
    public ICommand CommandEditButton { get; private set; }
    public ICommand CommandResetButton { get; private set; }
    public ICommand CommandUploadButton { get; private set; }

    private void InitializeComamnds()
    {

        CommandEditButton = new RelayCommand(OnEditButton);
        CommandSaveButton = new AsyncRelayCommand(OnSaveButtonAsync);
        CommandResetButton = new RelayCommand(OnResetButton);
        CommandUploadButton = new RelayCommand(OnUploadButton);
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
                DateOfTheLastVisitToTheJournal = ModelUser.DateOfTheLastVisitToTheJournal,
                AvatarUrl = ModelUser.AvatarUrl,
                StatusId = ModelUser.StatusId,
                EducationalInstitutionId = ModelUser.EducationalInstitutionId,
                ProfileId = ModelUser.ProfileId,
                DateOfTheLastUpdate = DateTime.Now
            };

            if (string.IsNullOrWhiteSpace(userToUpdate.Username)) { MessageBox.Show("Username is required", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

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
}