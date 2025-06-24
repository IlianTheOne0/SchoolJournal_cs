namespace DesktopApplication.Services.Grades;

using Database.Interfaces.Repositories.Grade;
using DesktopApplication.Interfaces.Services.Grades;
using DesktopApplication.Interfaces.Services.Strategies.AccessStrategy;
using Models.Tables.Grades;
using Models.Tables.Classes;
using Models.Tables.Users;
using Models.Tables.Subjects;
using Database.Repositories.Grades;
using DesktopApplication.Services.Strategies.TeacherAccess;

public class ServicesGrade : InterfacesServicesGrades
{
    private readonly InterfacesRepositoriesGrades _repositoryGrades;
    private readonly InterfacesAccessStrategy _accessStrategy;

    private List<ModelsClasses> _availableClasses = new(); public List<ModelsClasses> AvailableClasses => _availableClasses;
    private List<ModelsUser> _studentsInClass = new(); public List<ModelsUser> StudentsInClass => _studentsInClass;
    private List<ModelsGrades> _grades = new(); public List<ModelsGrades> FilteredGrades { get; private set; } = new();
    private List<ModelsSubjects> _availableSubjects = new(); public List<ModelsSubjects> AvailableSubjects => _availableSubjects;

    public event Action PropertyChanged;

    public ModelsClasses SelectedClass { get; set; }
    public ModelsUser SelectedStudent { get; set; }
    public ModelsSubjects SelectedSubject { get; set; }

    public bool IsTeacherMode { get; private set; }
    public bool HasClassSelected => SelectedClass != null;

    public ServicesGrade(RepositoriesGrades RepositoryGrade, InterfacesAccessStrategy AccessStrategy) { _repositoryGrades = RepositoryGrade; _accessStrategy = AccessStrategy; }

    public async Task Initialize()
    {
        IsTeacherMode = _accessStrategy is ServicesStrategiesTeacherAccess;

        if (IsTeacherMode) { await LoadClasses(); await LoadSubjects(); }
        else { await LoadStudentGrades(_accessStrategy.ModelUser.Id); }
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
        FilteredGrades = new List<ModelsGrades>(_grades);

        var subjectsIds = _grades.Select(gradeProvider => gradeProvider.SubjectId).Distinct().ToList();
        _availableSubjects = subjectsIds.Any() ? await _repositoryGrades.GetSubjectsByIdsAsync(subjectsIds) : new List<ModelsSubjects>();

        InvokeChangedProperty();
    }

    private void UpdatedFilteredGrades()
    {
        FilteredGrades = SelectedSubject == null ? new List<ModelsGrades>(_grades) : _grades.Where(gradeProvider => gradeProvider.SubjectId == SelectedSubject.Id).ToList();
        InvokeChangedProperty();
    }

    private void InvokeChangedProperty() => PropertyChanged?.Invoke();
}