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
                if (ModelSubject.Id == -2)
                {
                    var attendances = await _serviceGrades.GetAttendanceByStudent(_serviceGrades.CurrentUserId);
                    AvailableGrades = attendances.Select(attendancesProvider => new ModelsGradesExtended(attendancesProvider, _serviceGrades.AvailableSubjects.FirstOrDefault(subjectsProvider => subjectsProvider.Id == attendancesProvider.SubjectId)?.Name ?? "Unknown")).ToList();
                }
                else if (ModelSubject.Id == -1)
                {
                    var grades = await _serviceGrades.GetAllGradesByStudentId(_serviceGrades.CurrentUserId);
                    AvailableGrades = grades.Select(gradesProvider => new ModelsGradesExtended(gradesProvider, _serviceGrades.AvailableSubjects.FirstOrDefault(subjectsProvider => subjectsProvider.Id == gradesProvider.SubjectId)?.Name ?? "Unknown")).ToList();
                }
                else
                {
                    var grades = await _serviceGrades.GetAllGradesBySubjectAndStudent(ModelSubject.Id, _serviceGrades.CurrentUserId);
                    AvailableGrades = grades.Select(gradesProvider => new ModelsGradesExtended(gradesProvider, ModelSubject.Name)).ToList();
                }
                return;
            }

            if (ChosenStudent == null) { AvailableGrades = new List<ModelsGradesExtended>(); return; }

            if (ModelSubject.Id == -2)
            {
                var attendances = await _serviceGrades.GetAttendanceByStudent(ChosenStudent.Id);
                AvailableGrades = attendances.Select(attendancesProvider => new ModelsGradesExtended(attendancesProvider, _serviceGrades.AvailableSubjects.FirstOrDefault(subjectsProvider => subjectsProvider.Id == attendancesProvider.SubjectId)?.Name ?? "Unknown")).ToList();
            }
            else if (ModelSubject.Id == -1)
            {
                var grades = await _serviceGrades.GetAllGradesByStudentId(ChosenStudent.Id);
                AvailableGrades = grades.Select(gradesProvider => new ModelsGradesExtended(gradesProvider, _serviceGrades.AvailableSubjects.FirstOrDefault(subjectsProvider => subjectsProvider.Id == gradesProvider.SubjectId)?.Name ?? "Unknown")).ToList();
            }
            else
            {
                var grades = await _serviceGrades.GetAllGradesBySubjectAndStudent(ModelSubject.Id, ChosenStudent.Id);
                AvailableGrades = grades.Select(gradesProvider => new ModelsGradesExtended(gradesProvider, ModelSubject.Name)).ToList();
            }
        }
        catch (Exception E) { MessageBox.Show(E.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
}