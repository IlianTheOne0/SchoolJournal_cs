namespace DesktopApplication.ViewModels.GradesAssigner;

using DesktopApplication.Interfaces.Services.Grades;
using Models.Supports.GradesAssigner;
using Models.Tables.Classes;
using Models.Tables.Subjects;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class ViewModelsGradesAssigner : INotifyPropertyChanged
{
    private readonly InterfacesServicesGrades _serviceGrades;

    private List<ModelsClasses> _availableClasses; public List<ModelsClasses> AvailableClasses { get => _availableClasses; private set { _availableClasses = value; OnPropertyChanged(); } }
    private List<ModelsSubjects> _availableSubjects; public List<ModelsSubjects> AvailableSubjects { get => _availableSubjects; private set { _availableSubjects = value; OnPropertyChanged(); } }
    private List<StudentGradeAssignment> _availableGrades; public List<StudentGradeAssignment> AvailableGrades { get => _availableGrades; private set { _availableGrades = value; OnPropertyChanged(); } }

    private List<string> _availableMonths = new List<string> { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
    public List<string> AvailableMonths { get => _availableMonths; private set { _availableMonths = value; OnPropertyChanged(); } }
    private List<int> _availableYears = Enumerable.Range(DateTime.Now.Year - 5, 10).ToList(); public List<int> AvailableYears { get => _availableYears; private set { _availableYears = value; OnPropertyChanged(); } }

    private ModelsClasses _chosenClass; public ModelsClasses ChosenClass { get => _chosenClass; set { _chosenClass = value; OnPropertyChanged(); LoadSubjects(); } }
    private ModelsSubjects _chosenSubject; public ModelsSubjects ChosenSubject { get => _chosenSubject; set { _chosenSubject = value; OnPropertyChanged(); LoadGrades(); } }
    private string _chosenMonth; public string ChosenMonth { get => _chosenMonth; set { _chosenMonth = value; OnPropertyChanged(); LoadGrades(); } }
    private int _chosenYear; public int ChosenYear { get => _chosenYear; set { _chosenYear = value; OnPropertyChanged(); LoadGrades(); } }

    public event PropertyChangedEventHandler PropertyChanged;

    public ViewModelsGradesAssigner(InterfacesServicesGrades ServiceGrades) { _serviceGrades = ServiceGrades; Initialize(); }

    private async void Initialize()
    {
        try
        {
            AvailableClasses = await _serviceGrades.GetAvailableClasses();
            if (AvailableClasses.Any()) { ChosenClass = AvailableClasses.First(); ChosenMonth = DateTime.Now.ToString("MMMM"); ChosenYear = DateTime.Now.Year; }
        }
        catch (Exception E) { MessageBox.Show($"Initialization failed: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private async void LoadSubjects()
    {
        if (ChosenClass == null) { return; }

        try
        {
            AvailableSubjects = await _serviceGrades.GetSubjectsByClass(ChosenClass.Id);
            if (AvailableSubjects.Any()) { ChosenSubject = AvailableSubjects.First(); }
        }
        catch (Exception E) { MessageBox.Show($"Loading subjects failed: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private async void LoadGrades()
    {
        if (ChosenClass == null || ChosenSubject == null || string.IsNullOrEmpty(ChosenMonth) || ChosenYear == 0) { return; }

        try
        {
            int monthNumber = DateTime.ParseExact(ChosenMonth, "MMMM", null).Month;
            AvailableGrades = await _serviceGrades.GetStudentAssignments(ChosenClass.Id, ChosenSubject.Id, monthNumber, ChosenYear );
        }
        catch (Exception E) { MessageBox.Show($"Loading grades failed: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
    
    public async Task UpdateGrade(int StudentId, DateTime Date, int Grade, string Description)
    {
        try
        {
            int monthNumber = DateTime.ParseExact(ChosenMonth, "MMMM", null).Month;
            var dateWithMonth = new DateTime(ChosenYear, monthNumber, Date.Day);

            await _serviceGrades.UpdateGrade(StudentId, ChosenSubject.Id, dateWithMonth, Grade, Description);
            LoadGrades();
        }
        catch (Exception E) { MessageBox.Show($"Updating grades failed: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void OnPropertyChanged([CallerMemberName] string? PropertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
}