namespace DesktopApplication.ViewModels.GradeViewer;

using DesktopApplication.Interfaces.Services.Grades;
using DesktopApplication.Services.Grades;
using Models.Tables.Classes;
using Models.Tables.Grades;
using Models.Tables.Subjects;
using Models.Tables.Users;
using System.Windows;

public class ViewModelsGradeViewer
{
    private readonly InterfacesServicesGrades _serviceGrades;

    public ViewModelsGradeViewer(ServicesGrade ServiceGrade)
    {
        _serviceGrades = ServiceGrade;

        _serviceGrades.PropertyChanged += OnPropertyChanged;
        Initialize();
    }

    public List<ModelsClasses> AvailableClasses => _serviceGrades.AvailableClasses; public ModelsClasses SelectedClass { get => _serviceGrades.SelectedClass; set => _serviceGrades.SelectedClass = value; }
    public List<ModelsUser> StudentsInClass => _serviceGrades.StudentsInClass; public ModelsUser SelectedStudent { get => _serviceGrades.SelectedStudent; set => _serviceGrades.SelectedStudent = value; }
    public List<ModelsSubjects> AvailableSubjects => _serviceGrades.AvailableSubjects; public ModelsSubjects SelectedSubject { get => _serviceGrades.SelectedSubject; set => _serviceGrades.SelectedSubject = value; }
    public List<ModelsGradesExtended> FilteredGrades => _serviceGrades.FilteredGrades;

    public event Action PropertyChanged;

    public bool IsTeacherMode => _serviceGrades.IsTeacherMode;
    public bool HasClassSelected => _serviceGrades.HasClassSelected;

    public async void RefreshData()
    {
        try
        {
            await _serviceGrades.Initialize();
            PropertyChanged?.Invoke();
        }
        catch (Exception e) { MessageBox.Show($"Failed to refresh data: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private async void Initialize() => await _serviceGrades.Initialize();
    private void OnPropertyChanged() => PropertyChanged?.Invoke();
}