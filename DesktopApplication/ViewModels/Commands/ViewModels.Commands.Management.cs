using CommunityToolkit.Mvvm.Input;
using Models.Tables.Subjects;
using System.Windows;
using System.Windows.Input;

namespace DesktopApplication.ViewModels.Management;

public partial class ViewModelsManagement
{
    public ICommand CommandReset { get; private set; }

    public void CommandInitialize()
    {
        CommandReset = new RelayCommand(OnReset);
    }

    private async void OnReset() { await LoadData(); HardReset(); }
}