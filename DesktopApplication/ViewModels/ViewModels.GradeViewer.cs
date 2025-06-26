namespace DesktopApplication.ViewModels.GradeViewer;

using Interfaces.Services.Grades;

using Models.Tables.Classes;
using Models.Tables.Users;
using Models.Tables.Subjects;
using Models.Tables.Grades;

using System.Windows.Input;
using System.Windows;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Input;

public class ViewModelsGradeViewer : INotifyPropertyChanged
{
    private readonly InterfacesServicesGrades _serviceGrades = null!;

    private List<ModelsClasses> _availableClasses; public List<ModelsClasses> AvailableClasses { get => _availableClasses; private set { _availableClasses = value; OnPropertyChanged(); } }
    private List<ModelsUserExtended> _availableStudents; public List<ModelsUserExtended> AvailableStudents {get => _availableStudents; private set { _availableStudents = value; OnPropertyChanged(); } }
    private List<ModelsSubjects> _availableSubjects; public List<ModelsSubjects> AvailableSubjects {get => _availableSubjects; private set { _availableSubjects = value; OnPropertyChanged(); } }
    private List<ModelsGradesExtended> _availableGrades; public List<ModelsGradesExtended> AvailableGrades {get => _availableGrades; private set { _availableGrades = value; OnPropertyChanged(); } }

    
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

    public ICommand CommandChosenClass { get; }
    public ICommand CommandChosenStudent { get; }
    public ICommand CommandChosenSubject { get; }
    public ICommand CommandRefreshButton { get; }

    public event PropertyChangedEventHandler? PropertyChanged;
    
    public ViewModelsGradeViewer(InterfacesServicesGrades ServiceGrades)
    {
        _serviceGrades = ServiceGrades;

        CommandRefreshButton = new RelayCommand(Refresh);
        CommandChosenClass = new RelayCommand<ModelsClasses>(OnClassChoise!);
        CommandChosenStudent = new RelayCommand<ModelsUserExtended>(OnStudentChoise!);
        CommandChosenSubject = new AsyncRelayCommand<ModelsSubjects>(OnSubjectChoise!);

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
            subjects.Insert(0, _allSubjectsOption);
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
        catch (Exception e) { throw new Exception($"Applying changes failed: {e.Message}", e); }
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
        catch (Exception e) { MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
    public async void Refresh()
    {
        try {
            await _serviceGrades.Refresh(); ApplyChanges();

            ChosenSubject = _allSubjectsOption;
            await OnSubjectChoise(ChosenSubject);
        }
        catch (Exception e) { MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    public async void OnClassChoise(ModelsClasses ModelClass)
    {
        try
        {
            IsChosenClass = ModelClass != null && IsTeacherMode;

            if (IsChosenClass)
            {
                List<ModelsUserExtended> students = await _serviceGrades.GetAllStudentsByClassId(ModelClass!.Id);
                AvailableStudents = students;
            }
            else { AvailableStudents = new List<ModelsUserExtended>(); }
        }
        catch (Exception e) { MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
    public async void OnStudentChoise(ModelsUserExtended ModelUser)
    {
        try
        {
            if (ModelUser != null) { await _serviceGrades.GetAllGradesByStudentId(ModelUser.Id); AvailableGrades = await _serviceGrades.GetAllGradesByStudentId(ModelUser.Id); }
        }
        catch (Exception e) { MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
    public async Task OnSubjectChoise(ModelsSubjects ModelSubject)
    {
        try
        {
            if (ModelSubject == null) { AvailableGrades = new List<ModelsGradesExtended>(); return; }

            if (!IsTeacherMode)
            {
                List<ModelsGradesExtended> grades;
                if (ModelSubject.Id == -1) { grades = await _serviceGrades.GetAllGradesByStudentId(_serviceGrades.CurrentUserId); }
                else { grades = await _serviceGrades.GetAllGradesBySubjectAndStudent(ModelSubject.Id, _serviceGrades.CurrentUserId); }
                AvailableGrades = grades;
                return;
            }
            if (ChosenStudent == null) { AvailableGrades = new List<ModelsGradesExtended>(); return; }

            List<ModelsGradesExtended> studentGrades;
            if (ModelSubject.Id == -1) { studentGrades = await _serviceGrades.GetAllGradesByStudentId(ChosenStudent.Id); }
            else { studentGrades = await _serviceGrades.GetAllGradesBySubjectAndStudent(ModelSubject.Id, ChosenStudent.Id); }
            AvailableGrades = studentGrades;
        }
        catch (Exception e) { MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void OnPropertyChanged([CallerMemberName] string? PropertyName = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName)); }
}