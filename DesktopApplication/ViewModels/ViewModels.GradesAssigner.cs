namespace DesktopApplication.ViewModels.GradesAssigner;

using DesktopApplication.Interfaces.Services.Grades;
using Models.Supports.GradesAssigner;
using Models.Tables.Attending;
using Models.Tables.Classes;
using Models.Tables.Subjects;

using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;

public partial class ViewModelsGradesAssigner : INotifyPropertyChanged
{
    private readonly InterfacesServicesGrades _serviceGrades;

    private List<ModelsClasses> _availableClasses; public List<ModelsClasses> AvailableClasses { get => _availableClasses; private set { _availableClasses = value; OnPropertyChanged(); } }
    private List<ModelsSubjects> _availableSubjects; public List<ModelsSubjects> AvailableSubjects { get => _availableSubjects; private set { _availableSubjects = value; OnPropertyChanged(); } }
    private List<StudentGradeAssignment> _availableGrades; public List<StudentGradeAssignment> AvailableGrades { get => _availableGrades; private set { _availableGrades = value; OnPropertyChanged(); } }

    private List<string> _availableMonths = new List<string> { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
    public List<string> AvailableMonths { get => _availableMonths; private set { _availableMonths = value; OnPropertyChanged(); } }
    private List<int> _availableYears = Enumerable.Range(DateTime.Now.Year - 5, 10).ToList(); public List<int> AvailableYears { get => _availableYears; private set { _availableYears = value; OnPropertyChanged(); } }
    private List<DateColumn> _dateColumns = new(); public List<DateColumn> DateColumns { get => _dateColumns; private set { _dateColumns = value; OnPropertyChanged(); } }

    private ModelsClasses _chosenClass; public ModelsClasses ChosenClass { get => _chosenClass; set { _chosenClass = value; OnPropertyChanged(); LoadSubjects(); } }
    private ModelsSubjects _chosenSubject; public ModelsSubjects ChosenSubject { get => _chosenSubject; set { _chosenSubject = value; OnPropertyChanged(); LoadGrades(); } }
    private string _chosenMonth; public string ChosenMonth { get => _chosenMonth; set { _chosenMonth = value; OnPropertyChanged(); LoadGrades(); } }
    private int _chosenYear; public int ChosenYear { get => _chosenYear; set { _chosenYear = value; OnPropertyChanged(); LoadGrades(); } }

    private StudentGradeAssignment _currentStudent; public StudentGradeAssignment CurrentStudent { get => _currentStudent; set { _currentStudent = value; OnPropertyChanged(); } }

    private DateTime _currentDate; public DateTime CurrentDate { get => _currentDate; set { _currentDate = value; OnPropertyChanged(); } }
    private string _customGradeText = ""; public string CustomGradeText { get => _customGradeText; set { _customGradeText = value; OnPropertyChanged(); } }
    private string _descriptionText = ""; public string DescriptionText { get => _descriptionText; set { _descriptionText = value; OnPropertyChanged(); } }
    private bool _isCustomGradeEnabled = false; public bool IsCustomGradeEnabled { get => _isCustomGradeEnabled; set { _isCustomGradeEnabled = value; OnPropertyChanged(); } }
    private bool _isGradeEntryVisible = false; public bool IsGradeEntryVisible { get => _isGradeEntryVisible; set { _isGradeEntryVisible = value; OnPropertyChanged(); } }
    public string StudentDateInfo => CurrentStudent != null ? $"Student: {CurrentStudent.StudentName} | Date: {CurrentDate:dd.MM.yyyy}" : "";

    public event PropertyChangedEventHandler PropertyChanged;

    public ViewModelsGradesAssigner(InterfacesServicesGrades ServiceGrades) { _serviceGrades = ServiceGrades; Initialize(); }

    private async void Initialize()
    {
        try
        {
            AvailableClasses = await _serviceGrades.GetAvailableClasses();
            if (AvailableClasses.Any()) { ChosenClass = AvailableClasses.First(); ChosenMonth = DateTime.Now.ToString("MMMM", CultureInfo.GetCultureInfo("en-US")); ChosenYear = DateTime.Now.Year; }
        }
        catch (Exception E) { MessageBox.Show($"Initialization failed: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    public async void Load()
    {
        try
        {
            AvailableClasses = await _serviceGrades.GetAvailableClasses();

            if (AvailableClasses.Any())
            {
                ChosenClass = AvailableClasses.FirstOrDefault(c => c.Id == _chosenClass?.Id) ?? AvailableClasses.First();

                LoadSubjects();

                if (string.IsNullOrEmpty(ChosenMonth)) { ChosenMonth = DateTime.Now.ToString("MMMM", CultureInfo.GetCultureInfo("en-US")); }
                if (ChosenYear == 0) { ChosenYear = DateTime.Now.Year; }
            }
            else
            {
                ChosenClass = null;
                AvailableSubjects = new List<ModelsSubjects>();
                ChosenSubject = null;
            }
        }
        catch (Exception E) { MessageBox.Show($"Reloading data failed: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
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
            int monthNumber = DateTime.ParseExact(ChosenMonth, "MMMM", CultureInfo.GetCultureInfo("en-US")).Month;
            AvailableGrades = await _serviceGrades.GetStudentAssignments(ChosenClass.Id, ChosenSubject.Id, monthNumber, ChosenYear);
            GenerateDateColumns(monthNumber, ChosenYear);

            var attendanceRecords = await _serviceGrades.GetAttendanceByClass(
                ChosenClass.Id,
                ChosenSubject.Id,
                monthNumber,
                ChosenYear
            );

            foreach (var student in AvailableGrades)
            {
                student.Attendances = attendanceRecords
                    .Where(attendancesProvider => attendancesProvider.UserId == student.StudentId)
                    .ToDictionary(attendancesProvider => attendancesProvider.Date.Date, a => a);
            }
        }
        catch (Exception E) { MessageBox.Show($"Loading grades failed: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
    
    public async Task UpdateGrade(int StudentId, DateTime Date, int Grade, string Description)
    {
        try
        {
            int monthNumber = DateTime.ParseExact(ChosenMonth, "MMMM", CultureInfo.GetCultureInfo("en-US")).Month;
            var dateWithMonth = new DateTime(ChosenYear, monthNumber, Date.Day);

            await _serviceGrades.UpdateGrade(StudentId, ChosenSubject.Id, dateWithMonth, Grade, Description);
            LoadGrades();
        }
        catch (Exception E) { MessageBox.Show($"Updating grades failed: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    public async Task DeleteGrade(int StudentId, DateTime Date)
    {
        try
        {
            int monthNumber = DateTime.ParseExact(ChosenMonth, "MMMM", CultureInfo.GetCultureInfo("en-US")).Month;
            var dateWithMonth = new DateTime(ChosenYear, monthNumber, Date.Day);

            await _serviceGrades.DeleteGrade(StudentId, ChosenSubject.Id, dateWithMonth);
            LoadGrades();
        }
        catch (Exception E) { MessageBox.Show($"Deleting grade failed: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    public async Task MarkAttendance(int StudentId, DateTime Date, bool IsSick)
    {
        try
        {
            int monthNumber = DateTime.ParseExact(ChosenMonth, "MMMM", CultureInfo.GetCultureInfo("en-US")).Month;
            var dateWithMonth = new DateTime(ChosenYear, monthNumber, Date.Day);

            await _serviceGrades.DeleteAttendance(StudentId, ChosenSubject.Id, dateWithMonth);

            var attendance = new ModelsAttending
            {
                UserId = StudentId,
                SubjectId = ChosenSubject.Id,
                Date = dateWithMonth,
                Sickness = IsSick
            };
            await _serviceGrades.InsertAttendance(attendance);

            LoadGrades();
        }
        catch (Exception E) { MessageBox.Show($"Attendance update failed: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    public async Task DeleteAttendance(int StudentId, DateTime Date)
    {
        try
        {
            int monthNumber = DateTime.ParseExact(ChosenMonth, "MMMM", CultureInfo.GetCultureInfo("en-US")).Month;
            var dateWithMonth = new DateTime(ChosenYear, monthNumber, Date.Day);

            await _serviceGrades.DeleteAttendance(StudentId, ChosenSubject.Id, dateWithMonth);
            LoadGrades();
        }
        catch (Exception E) { MessageBox.Show($"Attendance deletion failed: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
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

    private void OnPropertyChanged([CallerMemberName] string? PropertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        if (PropertyName == nameof(CurrentStudent) || PropertyName == nameof(CurrentDate)) { OnPropertyChanged(nameof(StudentDateInfo)); }
    }
}