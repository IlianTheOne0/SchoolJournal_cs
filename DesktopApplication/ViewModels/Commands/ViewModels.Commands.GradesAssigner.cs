namespace DesktopApplication.ViewModels.GradesAssigner;

using Models.Supports.GradesAssigner;

using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Input;

public partial class ViewModelsGradesAssigner
{
    public ICommand CommandGradeButton { get; private set; }
    public ICommand CommandSaveGrade { get; private set; }
    public ICommand CommandCancelGrade { get; private set; }
    public ICommand CommandShowGradeEntry { get; private set; }

    private void InitializeComamnds()
    {
        CommandGradeButton = new RelayCommand<object>(OnGradeButton);
        CommandSaveGrade = new RelayCommand<object>(OnSaveGradeButton, CanOnSaveGrade);
        CommandCancelGrade = new RelayCommand<object>(OnCancelGradeButton);
        CommandShowGradeEntry = new RelayCommand<object>(OnShowGradeEntryButton);
    }

    private async void OnGradeButton(object Parameter)
    {
        if (Parameter is int grade && CurrentStudent != null) { await SaveGrade(grade); }
    }

    private async void OnSaveGradeButton(object Parameter)
    {
        if (int.TryParse(CustomGradeText, out int customGrade)) { await SaveGrade(customGrade); }
        else { MessageBox.Show("Please enter a valid grade", "Error", MessageBoxButton.OK, MessageBoxImage.Warning); }
    }

    private bool CanOnSaveGrade(object Parameter) => CurrentStudent != null && (int.TryParse(CustomGradeText, out _));

    private void OnCancelGradeButton(object Parameter)
    {
        IsGradeEntryVisible = false;
        CurrentStudent = null;
        CustomGradeText = "";
        DescriptionText = string.Empty;
        IsCustomGradeEnabled = false;
    }

    private void OnShowGradeEntryButton(object Parameter)
    {
        if (Parameter is Tuple<StudentGradeAssignment, DateTime> data) { ShowGradeEntry(data.Item1, data.Item2); }
    }

    private async Task SaveGrade(int Grade)
    {
        if (CurrentStudent == null) { return; }

        await UpdateGrade(CurrentStudent.StudentId, CurrentDate, Grade, DescriptionText);
        IsGradeEntryVisible = false;
    }

    public void ShowGradeEntry(StudentGradeAssignment Student, DateTime Date)
    {
        CurrentStudent = Student;
        CurrentDate = Date;

        if (CurrentStudent.Grades.TryGetValue(CurrentDate, out var existingGrade))
        {
            CustomGradeText = existingGrade.Grade?.ToString() ?? "";
            DescriptionText = existingGrade.Description ?? "";
        }
        else { CustomGradeText = ""; DescriptionText = ""; }

        IsGradeEntryVisible = true;
    }
}