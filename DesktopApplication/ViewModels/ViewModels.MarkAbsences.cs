namespace DesktopApplication.ViewModels.MarkAbsences;

using DesktopApplication.Interfaces.Services.Grades;
using Models.Supports.GradesAssigner;
using Models.Tables.Classes;
using Models.Tables.Subjects;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;

public class ViewModelsMarkAbsences : INotifyPropertyChanged
{
    private readonly InterfacesServicesGrades _serviceGrades;

    private List<ModelsClasses> _availableClasses; public List<ModelsClasses> AvailableClasses { get => _availableClasses; private set { _availableClasses = value; OnPropertyChanged(); } }
    private List<ModelsSubjects> _availableSubjects; public List<ModelsSubjects> AvailableSubjects { get => _availableSubjects; private set { _availableSubjects = value; OnPropertyChanged(); } }
    private List<string> _availableMonths = new List<string> { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
    public List<string> AvailableMonths { get => _availableMonths; private set { _availableMonths = value; OnPropertyChanged(); } }
    private List<int> _availableYears = Enumerable.Range(DateTime.Now.Year - 5, 10).ToList(); public List<int> AvailableYears { get => _availableYears; private set { _availableYears = value; OnPropertyChanged(); } }
    private List<DateColumn> _dateColumns = new();
    public List<DateColumn> DateColumns { get => _dateColumns; private set { _dateColumns = value; OnPropertyChanged(); } }

    private ModelsClasses _chosenClass; public ModelsClasses ChosenClass { get => _chosenClass; set { _chosenClass = value; OnPropertyChanged(); LoadSubjects(); } }
    private ModelsSubjects _chosenSubject; public ModelsSubjects ChosenSubject { get => _chosenSubject; set { _chosenSubject = value; OnPropertyChanged(); LoadAbsences(); } }
    private string _chosenMonth; public string ChosenMonth { get => _chosenMonth; set { _chosenMonth = value; OnPropertyChanged(); LoadAbsences(); } }
    private int _chosenYear; public int ChosenYear { get => _chosenYear; set { _chosenYear = value; OnPropertyChanged(); LoadAbsences(); } }

    private DateTime _currentDate; public DateTime CurrentDate { get => _currentDate; set { _currentDate = value; OnPropertyChanged(); } }

    public event PropertyChangedEventHandler PropertyChanged;

    public ViewModelsMarkAbsences(InterfacesServicesGrades ServiceGrades) { _serviceGrades = ServiceGrades; Initialize(); }

    private async void Initialize()
    {
        try
        {
            AvailableClasses = await _serviceGrades.GetAvailableClasses();
            if (AvailableClasses.Any()) { ChosenClass = AvailableClasses.First(); ChosenMonth = DateTime.Now.ToString("MMMM", CultureInfo.GetCultureInfo("en-US")); ChosenYear = DateTime.Now.Year; }
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

    private async void LoadAbsences()
    {
        if (ChosenClass == null || ChosenSubject == null || string.IsNullOrEmpty(ChosenMonth) || ChosenYear == 0) { return; }
        // TODO
    }

    public async Task ChangeValue(int StudentId, DateTime Date, int Grade, string Description)
    {
        try
        {
            int monthNumber = DateTime.ParseExact(ChosenMonth, "MMMM", CultureInfo.GetCultureInfo("en-US")).Month;
            var dateWithMonth = new DateTime(ChosenYear, monthNumber, Date.Day);

            //await _serviceGrades.UpdateGrade(StudentId, ChosenSubject.Id, dateWithMonth, Grade, Description);
            LoadAbsences();
        }
        catch (Exception E) { MessageBox.Show($"Updating grades failed: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void GenerateDateColumns(int Month, int Year)
    {
        var columns = new List<DateColumn>();
        var daysInMonth = DateTime.DaysInMonth(Year, Month);
        var culture = CultureInfo.GetCultureInfo("en-US");

        for (int day = 1; day <= daysInMonth; day++)
        {
            var date = new DateTime(Year, Month, day);
            columns.Add(new DateColumn
            {
                Date = date,
                Header = date.ToString("dd MMM", culture)
            });
        }

        DateColumns = columns;
    }

    private void OnPropertyChanged([CallerMemberName] string? PropertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
}