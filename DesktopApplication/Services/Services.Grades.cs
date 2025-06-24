namespace DesktopApplication.Services.Grades;

using Database.Interfaces.Repositories.Grade;
using Database.Repositories.Grades;
using DesktopApplication.Interfaces.Services.Grades;
using DesktopApplication.Interfaces.Services.Strategies.AccessStrategy;
using DesktopApplication.Interfaces.Services.User;
using DesktopApplication.Services.Strategies.TeacherAccess;
using DesktopApplication.Services.Supabase;
using Models.Tables.Classes;
using Models.Tables.Grades;
using Models.Tables.Subjects;
using Models.Tables.Users;

public class ServicesGrade : InterfacesServicesGrades
{
    private readonly InterfacesRepositoriesGrades _repositoryGrades;
    private readonly InterfacesServicesUser _serviceUser;

    private List<ModelsClasses> _availableClasses = new(); public List<ModelsClasses> AvailableClasses => _availableClasses;
    private List<ModelsUser> _studentsInClass = new(); public List<ModelsUser> StudentsInClass => _studentsInClass;
    private List<ModelsGradesExtended> _grades = new(); public List<ModelsGradesExtended> FilteredGrades { get; private set; } = new();
    private List<ModelsSubjects> _availableSubjects = new(); public List<ModelsSubjects> AvailableSubjects => _availableSubjects;

    public event Action PropertyChanged;

    public ModelsClasses SelectedClass { get; set; }
    public ModelsUser SelectedStudent { get; set; }
    public ModelsSubjects SelectedSubject { get; set; }

    public bool IsTeacherMode { get; private set; }
    public bool HasClassSelected => SelectedClass != null;

    public ServicesGrade(ServicesSupabase ServiceSupabase, ServicesUser ServiceUser)
    {
        _repositoryGrades = new RepositoriesGrades(ServiceSupabase.RepositorySupabase);
        _serviceUser = ServiceUser;
    }

    public async Task Initialize()
    {
        if (_serviceUser?.AccessStrategy == null) { throw new InvalidOperationException("User access strategy is not initialized"); }

        InterfacesAccessStrategy accessStrategy = _serviceUser.AccessStrategy;
        IsTeacherMode = accessStrategy is ServicesStrategiesTeacherAccess;

        if (IsTeacherMode) { await LoadClasses(); await LoadSubjects(); }
        else
        {
            if (accessStrategy.ModelUser == null) { throw new InvalidOperationException("User model is not initialized"); }
            await LoadStudentGrades(accessStrategy.ModelUser.Id);
        }
    }

    private async Task LoadClasses() { _availableClasses = await _repositoryGrades.GetAllClassesAsync(); InvokeChangedProperty(); }
    private async Task LoadSubjects() {  _availableSubjects = await _repositoryGrades.GetAllSubjectsAsync(); InvokeChangedProperty(); }
    
    private async Task LoadStudentInClass(int ClassId)
    {
        var enrollments = await _repositoryGrades.GetEnrollmentsByClassAsync(ClassId);
        var studentIds = enrollments.Select(enrollmentProvider => enrollmentProvider.UserId).ToList();

        _studentsInClass = studentIds.Any() ? await _repositoryGrades.GetStudentsByIdsAsync(studentIds) : new List<ModelsUser>();

        InvokeChangedProperty();
    }

    private async Task LoadGradesForStudent(int StudentId) { _grades = await _repositoryGrades.GetGradesByStudentAsync(StudentId); UpdatedFilteredGrades(); }

    private async Task LoadStudentGrades(int StudentId)
    {
        _grades = await _repositoryGrades.GetGradesByStudentAsync(StudentId);
        FilteredGrades = new List<ModelsGradesExtended>(_grades);

        var subjectsIds = _grades.Select(gradeProvider => gradeProvider.SubjectId).Distinct().ToList();
        _availableSubjects = subjectsIds.Any() ? await _repositoryGrades.GetSubjectsByIdsAsync(subjectsIds) : new List<ModelsSubjects>();

        InvokeChangedProperty();
    }

    public async Task RefreshData()
    {
        if (IsTeacherMode)
        {
            await LoadClasses(); await LoadSubjects();

            if (SelectedClass != null) { await LoadStudentInClass(SelectedClass.Id); }
            if (SelectedStudent != null) { await LoadGradesForStudent(SelectedStudent.Id); }
        }
        else { await LoadStudentGrades(_serviceUser.AccessStrategy?.ModelUser.Id ?? 0); }

        InvokeChangedProperty();
    }

    private void UpdatedFilteredGrades()
    {
        FilteredGrades = SelectedSubject == null ? new List<ModelsGradesExtended>(_grades) : _grades.Where(gradeProvider => gradeProvider.SubjectId == SelectedSubject.Id).ToList();
        InvokeChangedProperty();
    }

    private void InvokeChangedProperty() => PropertyChanged?.Invoke();
}