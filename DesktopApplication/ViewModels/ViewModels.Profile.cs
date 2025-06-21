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

    private readonly ServicesAuth _serviceAuth;

    public event PropertyChangedEventHandler? PropertyChanged;
    private ModelsUser? _modelUser = null; public ModelsUser ModelUser { get => _modelUser!; set { _modelUser = value; OnPropertyChanged(); OnPropertyChanged("AvatarUrl"); } }
    private bool isEditing = false;
    public bool IsReadOnly => !isEditing;
    public bool ShowEditButton => !isEditing;
    public bool ShowSaveButton => isEditing;

    public ViewModelsProfile(ServicesAuth ServiceAuth)
    {
        _serviceAuth = ServiceAuth;

        CommandEditButton = new RelayCommand(OnEditButton);
        CommandSaveButton = new RelayCommand(OnSaveButton);
    }

    public void LoadData()
    {
        try { ModelUser = _serviceAuth.AccessStrategy!.ModelUser; OnPropertyChanged(nameof(ModelUser)); }
        catch (Exception e) { MessageBox.Show($"Load data failed: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    public void OnEditButton()
    {
        isEditing = true;

        OnPropertyChanged(nameof(IsReadOnly)); OnPropertyChanged(nameof(ShowEditButton)); OnPropertyChanged(nameof(ShowSaveButton));
    }

    public void OnSaveButton() => throw new NotImplementedException();

    public void ResetEditingState()
    {
        isEditing = false;
        LoadData();
        OnPropertyChanged(nameof(IsReadOnly)); OnPropertyChanged(nameof(ShowEditButton)); OnPropertyChanged(nameof(ShowSaveButton));
    }

    protected void OnPropertyChanged([CallerMemberName] string? PropertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
}