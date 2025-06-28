namespace DesktopApplication.ViewModels.GradesViewer;

using DesktopApplication.Interfaces.Services.Grades;
using Models.Tables.Classes;
using Models.Tables.Grades;
using Models.Tables.Subjects;
using Models.Tables.Users;

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

public partial class ViewModelsGradesViewer : INotifyPropertyChanged
{
    private readonly InterfacesServicesGrades _serviceGrades = null!;

    private List<ModelsClasses> _availableClasses; public List<ModelsClasses> AvailableClasses { get => _availableClasses; private set { _availableClasses = value; OnPropertyChanged(); } }
    private List<ModelsUserExtended> _availableStudents; public List<ModelsUserExtended> AvailableStudents {get => _availableStudents; private set { _availableStudents = value; OnPropertyChanged(); } }
    private List<ModelsSubjects> _availableSubjects; public List<ModelsSubjects> AvailableSubjects {get => _availableSubjects; private set { _availableSubjects = value; OnPropertyChanged(); } }
    private List<ModelsGradesExtended> _availableGrades; public List<ModelsGradesExtended> AvailableGrades { get => _availableGrades; private set { _availableGrades = value; OnPropertyChanged(); } }

    private readonly ModelsSubjects _absenceOption = new ModelsSubjects
    {
        Id = -2,
        Name = "Absence",
        TeacherId = -1,
        ClassId = -1
    };

    private readonly ModelsSubjects _allSubjectsOption = new ModelsSubjects
    {
        Id = -1,
        Name = "All",
        TeacherId = -1,
        ClassId = -1
    };
    private bool _isTeacherMode = false; public bool IsTeacherMode { get => _isTeacherMode; set { _isTeacherMode = value; OnPropertyChanged(); } }
    private bool _isChosenClass = false; public bool IsChosenClass { get => _isChosenClass; set { _isChosenClass= value; OnPropertyChanged(); } }

    private ModelsClasses _chosenClass; public ModelsClasses ChosenClass { get => _chosenClass; set { _chosenClass = value; OnPropertyChanged(); OnClassChoise(value); } }
    private ModelsUserExtended _chosenStudent; public ModelsUserExtended ChosenStudent { get => _chosenStudent; set { _chosenStudent = value; OnPropertyChanged(); OnStudentChoise(value); } }
    private ModelsSubjects _chosenSubject; public ModelsSubjects ChosenSubject { get => _chosenSubject; set { _chosenSubject = value; OnPropertyChanged(); OnSubjectChoise(value); } }

    public event PropertyChangedEventHandler? PropertyChanged;
    
    public ViewModelsGradesViewer(InterfacesServicesGrades ServiceGrades)
    {
        _serviceGrades = ServiceGrades;

        InitializeComamnds();
        Initialize();
    }

    private void ApplyChanges()
    {
        try
        {
            AvailableClasses = _serviceGrades.AvailableClasses;
            AvailableStudents = _serviceGrades.AvailableStudents;

            var currentSelectedSubject = ChosenSubject;
            var subjects = _serviceGrades.AvailableSubjects.ToList();
            subjects.Insert(0, _allSubjectsOption); subjects.Insert(1, _absenceOption);
            AvailableSubjects = subjects;

            if (currentSelectedSubject != null)
            {
                var matchingSubject = AvailableSubjects.FirstOrDefault(subjectsProvider => subjectsProvider.Id == currentSelectedSubject.Id);
                if (matchingSubject != null) { ChosenSubject = matchingSubject; }
                else { ChosenSubject = _allSubjectsOption; }
            }

            AvailableGrades = _serviceGrades.AvailableGrades;
        
            IsTeacherMode = _serviceGrades.GetIsTeacherMode();
        }
        catch (Exception E) { throw new Exception($"Applying changes failed: {E.Message}", E); }
    }

    public async void Initialize()
    {
        try
        {
            await _serviceGrades.Initialize();

            var subjects = _serviceGrades.AvailableSubjects;
            subjects.Insert(0, _allSubjectsOption);
            AvailableSubjects = subjects;

            ChosenSubject = _allSubjectsOption;

            ApplyChanges();
        }
        catch (Exception E) { MessageBox.Show(E.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    public void Reset() => OnReset();

    private void OnPropertyChanged([CallerMemberName] string? PropertyName = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName)); }
}