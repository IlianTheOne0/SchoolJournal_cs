namespace DesktopApplication.ViewModels.Profile;

using CommunityToolkit.Mvvm.Input;
using DesktopApplication.Services.Auth;
using Models.Tables.Users;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

public class ViewModelsProfile : INotifyPropertyChanged
{
    public ICommand CommandEditButton { get; }
    public ICommand CommandSaveButton { get; }
    public ICommand CommandResetButton { get; }

    private readonly ServicesAuth _serviceAuth;

    public event PropertyChangedEventHandler? PropertyChanged;

    private ModelsUser? _originalModelUser = null;
    private ModelsUser? _modelUser = null; public ModelsUser ModelUser { get => _modelUser!; set { _modelUser = value; OnPropertyChanged(); OnPropertyChanged("AvatarUrl"); } }
    public bool IsEditing = false;
    public bool IsReadOnly => !IsEditing;
    public bool ShowEditButton => !IsEditing;
    public bool ShowSaveButton => IsEditing;

    public ViewModelsProfile(ServicesAuth ServiceAuth)
    {
        _serviceAuth = ServiceAuth;

        CommandEditButton = new RelayCommand(OnEditButton);
        CommandSaveButton = new RelayCommand(OnSaveButton);
        CommandResetButton = new RelayCommand(OnResetButton);
    }

    public void LoadData()
    {
        try
        {
            _originalModelUser = _serviceAuth.AccessStrategy!.ModelUser;
            ModelUser = new ModelsUser
            {
                Id = _originalModelUser.Id,
                Username = _originalModelUser.Username,
                FullName = _originalModelUser.FullName,
                Email = _originalModelUser.Email,
                PhoneNumber = _originalModelUser.PhoneNumber,
                Sex = _originalModelUser.Sex,
                DateOfBirth = _originalModelUser.DateOfBirth,
                CreatedAt = _originalModelUser.CreatedAt,
                DateOfTheLastUpdate = _originalModelUser.DateOfTheLastUpdate,
                DateOfTheLastVisitToTheJorunal = _originalModelUser.DateOfTheLastVisitToTheJorunal,
                AvatarUrl = _originalModelUser.AvatarUrl,
                StatusId = _originalModelUser.StatusId,
                EducationalInstitutionId = _originalModelUser.EducationalInstitutionId,
                ProfileId = _originalModelUser.ProfileId,
                StatusName = _originalModelUser.StatusName,
                EducationalInstitutionName = _originalModelUser.EducationalInstitutionName
            };
            OnPropertyChanged(nameof(ModelUser));
        }
        catch (Exception e) { MessageBox.Show($"Load data failed: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    public void OnEditButton()
    {
        IsEditing = true;

        OnPropertyChanged(nameof(IsReadOnly)); OnPropertyChanged(nameof(ShowEditButton)); OnPropertyChanged(nameof(ShowSaveButton));
    }

    public void OnSaveButton() => throw new NotImplementedException();
    public void OnResetButton() => ResetEditingState();

    public void ResetEditingState()
    {
        IsEditing = false;
        LoadData();

        OnPropertyChanged(nameof(IsReadOnly)); OnPropertyChanged(nameof(ShowEditButton)); OnPropertyChanged(nameof(ShowSaveButton));
    }

    protected void OnPropertyChanged([CallerMemberName] string? PropertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
}