namespace DesktopApplication.ViewModels.GradesViewer;

using Models.Tables.Classes;
using Models.Tables.Grades;
using Models.Tables.Subjects;
using Models.Tables.Users;

using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Input;

public partial class ViewModelsGradesViewer
{
    public ICommand CommandChosenClass { get; private set; }
    public ICommand CommandChosenStudent { get; private set; }
    public ICommand CommandChosenSubject { get; private set; }
    public ICommand CommandResetButton { get; private set; }

    private void InitializeComamnds()
    {
        CommandResetButton = new RelayCommand(OnReset);
        CommandChosenClass = new RelayCommand<ModelsClasses>(OnClassChoise!);
        CommandChosenStudent = new RelayCommand<ModelsUserExtended>(OnStudentChoise!);
        CommandChosenSubject = new AsyncRelayCommand<ModelsSubjects>(OnSubjectChoise!);
    }

    public async void OnReset()
    {
        try
        {
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
}