namespace DesktopApplication.ViewModels.GradesAssigner;

using DesktopApplication.Interfaces.Services.Grades;
using Models.Tables.Classes;
using Models.Tables.Grades;
using Models.Tables.Subjects;
using Models.Tables.Users;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Models.Support.GradesAssigner;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

public class ViewModelsGradesAssigner
{
    private readonly InterfacesServicesGrades _serviceGrades;

    private List<ModelsClasses> _availableClasses; public List<ModelsClasses> AvailableClasses { get => _availableClasses; private set { _availableClasses = value; OnPropertyChanged(); } }
    private List<ModelsUserExtended> _availableStudents; public List<ModelsUserExtended> AvailableStudents { get => _availableStudents; private set { _availableStudents = value; OnPropertyChanged(); } }
    private List<ModelsSubjects> _availableSubjects; public List<ModelsSubjects> AvailableSubjects { get => _availableSubjects; private set { _availableSubjects = value; OnPropertyChanged(); } }
    private List<StudentGradeRow> _availableGrades; public List<StudentGradeRow> AvailableGrades { get => _availableGrades; private set { _availableGrades = value; OnPropertyChanged(); } }

    public List<int> AvailableMonths { get; } = Enumerable.Range(1, 12).ToList();
    public List<int> AvailableYears { get; } = Enumerable.Range(DateTime.Now.Year - 5, 10).ToList();
    private bool _isChosenClass = false; public bool IsChosenClass { get => _isChosenClass; set { _isChosenClass = value; OnPropertyChanged(); } }

    private ModelsClasses _chosenClass; public ModelsClasses ChosenClass { get => _chosenClass; set { _chosenClass = value; OnPropertyChanged(); OnClassChoise(value); } }
    private ModelsSubjects _chosenSubject; public ModelsSubjects ChosenSubject { get => _chosenSubject; set { _chosenSubject = value; OnPropertyChanged(); _ = OnSubjectChoise(value); } }

    private int? _chosenMonth = DateTime.Now.Month; public int? ChosenMonth { get => _chosenMonth; set { _chosenMonth = value; OnPropertyChanged(); } }
    private int? _chosenYear = DateTime.Now.Year; public int? ChosenYear { get => _chosenYear; set { _chosenYear = value; OnPropertyChanged(); } }

    public PropertyChangedEventHandler? PropertyChanged;

    public ViewModelsGradesAssigner(InterfacesServicesGrades ServiceGrades)
    {
        _serviceGrades = ServiceGrades;
        _ = Initialize();
    }

    private void ApplyChanges()
    {
        try
        {
            AvailableClasses = _serviceGrades.AvailableClasses;
            AvailableSubjects = _serviceGrades.AvailableSubjects.ToList();
        }
        catch (Exception E) { throw new Exception($"Applying changes failed: {E.Message}", E); }
    }

    public async Task Initialize()
    {
        try
        {
            await _serviceGrades.Initialize();
            ApplyChanges();

            if (ChosenClass != null) { await LoadStudentsForClass(ChosenClass.Id); }
        }
        catch (Exception E) { MessageBox.Show($"Initializing of grades assigner's view model failed: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    public async Task Refresh()
    {
        try
        {
            await _serviceGrades.Refresh();
            ApplyChanges();

            if (ChosenClass != null) { await LoadStudentsForClass(ChosenClass.Id); await LoadGradesForMonth(); }
        }
        catch (Exception E) { MessageBox.Show($"Refreshing of the table failed: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    public async void OnClassChoise(ModelsClasses ModelClass)
    {
        try
        {
            if (ModelClass != null) { await LoadStudentsForClass(ModelClass.Id); }
            else
            {
                AvailableStudents = new List<ModelsUserExtended>();
                UpdateStudentsGrades(AvailableStudents);
            }
        }
        catch (Exception E) { MessageBox.Show($"Command execution failed: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    public async Task OnSubjectChoise(ModelsSubjects ModelSubject)
    {
        try
        {
            if (ModelSubject != null)
            {
                if (AvailableStudents?.Any() == true && AvailableGrades?.Any() == true) { await LoadGradesForMonth(); }
                return;
            }
        }
        catch (Exception E) { MessageBox.Show($"Command execution failed: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private async Task LoadStudentsForClass(int classId)
    {
        try
        {
            AvailableStudents = await _serviceGrades.GetAllStudentsByClassId(classId);
            UpdateStudentsGrades(AvailableStudents);
        }
        catch (Exception E) { MessageBox.Show($"Loading students for class failed: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    public async Task LoadGradesForMonth()
    {
        try
        {
            foreach (var studentRow in AvailableGrades)
            {
                var grades = await _serviceGrades.GetAllGradesBySubjectAndStudent(ChosenSubject.Id, studentRow.UserId);
                UpdateStudentGradesForMonth(studentRow, grades);
            }
        }
        catch (Exception E) { MessageBox.Show($"Loading grades for month failed: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void UpdateStudentsGrades(List<ModelsUserExtended> Students)
    {
        if (ChosenYear == null || ChosenMonth == null) { AvailableGrades = new List<StudentGradeRow>(); return; }

        var daysInMonth = DateTime.DaysInMonth(ChosenYear.Value, ChosenMonth.Value);
        var newStudentsGrades = new List<StudentGradeRow>();

        foreach (var student in Students)
        {
            var studentRow = new StudentGradeRow
            {
                UserId = student.Id,
                StudentName = student.FullName,
                DayGrades = new List<DayGrade>()
            };

            for (int day = 1; day <= daysInMonth; day++)
            {
                studentRow.DayGrades.Add(new DayGrade
                {
                    Date = new DateTime(ChosenYear.Value, ChosenMonth.Value, day)
                });
            }

            newStudentsGrades.Add(studentRow);
        }

        AvailableGrades = newStudentsGrades;
    }

    private void UpdateStudentGradesForMonth(StudentGradeRow StudentRow, List<ModelsGradesExtended> Grades)
    {
        var monthGrades = Grades.Where(
            gradesProvider => gradesProvider.Date.Year == ChosenYear.Value && gradesProvider.Date.Month == ChosenMonth.Value
        ).ToList();

        foreach (var grade in monthGrades)
        {
            int dayIndex = grade.Date.Day - 1;
            if (dayIndex >= 0 && dayIndex < StudentRow.DayGrades.Count)
            {
                StudentRow.DayGrades[dayIndex].Grade = grade.Grade.ToString();
                StudentRow.DayGrades[dayIndex].Comment = grade.Description;
            }
        }
    }

    public async Task LoadStudentsByClass(int ClassId)
    {
        try { await _serviceGrades.GetAllStudentsByClassId(ClassId); }
        catch (Exception E) { MessageBox.Show($"Loading students by class failed: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    public async Task AddGrade(ModelsGrades Grade)
    {
        try { await _serviceGrades.AddGrade(Grade); }
        catch (Exception E) { MessageBox.Show($"Adding the grade failed: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void OnPropertyChanged([CallerMemberName] string? PropertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
}