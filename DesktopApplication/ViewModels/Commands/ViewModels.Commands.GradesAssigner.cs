using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace DesktopApplication.ViewModels.GradesAssigner;

public partial class ViewModelsGradesAssigner
{
    public ICommand GradeButtonCommand { get; private set; }
    public ICommand SaveGradeCommand { get; private set; }
    public ICommand CancelGradeCommand { get; private set; }
    public ICommand ShowGradeEntryCommand { get; private set; }

    private void InitializeComamnds()
    {
        GradeButtonCommand = new RelayCommand(OnGradeButton);
        SaveGradeCommand = new RelayCommand(OnSaveGrade, CanOnSaveGrade);
        CancelGradeCommand = new RelayCommand(OnCancelGrade);
        ShowGradeEntryCommand = new RelayCommand(OnShowGradeEntry);
    }
}