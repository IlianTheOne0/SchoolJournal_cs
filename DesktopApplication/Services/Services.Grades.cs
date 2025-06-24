namespace DesktopApplication.Services.Grades;

using Infrastructure.Models.Tables.Grades;
using Infrastructure.Models.Tables.Classes;
using Infrastructure.Models.Tables.Users;
using Infrastructure.Models.Tables.Subjects;
using Infrastructure.Interfaces.Mediators.Grades;

using DesktopApplication.Interfaces.Services.Grades;
using DesktopApplication.Interfaces.Services.Strategies.AccessStrategy;
using DesktopApplication.Services.Strategies.TeacherAccess;

public class ServicesGrade : InterfacesServicesGrades
{
    private readonly InterfacesMediatorsGrades _mediatorGrades;
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

    public ServicesGrade(InterfacesMediatorsGrades MediatorGrade, InterfacesAccessStrategy AccessStrategy) { _mediatorGrades = MediatorGrade; _accessStrategy = AccessStrategy; }

    public async Task Initialize()
    {
        IsTeacherMode = _accessStrategy is ServicesStrategiesTeacherAccess;

        if (IsTeacherMode) { await LoadClasses(); await LoadSubjects(); }
        else { await LoadStudentGrades(_accessStrategy.ModelUser.Id); }
    }

    private async Task LoadClasses() { _availableClasses = await _mediatorGrades.GetAllClassesAsync(); InvokeChangedProperty(); }
    private async Task LoadSubjects() {  _availableSubjects = await _mediatorGrades.GetAllSubjectsAsync(); InvokeChangedProperty(); }
    
    private async Task LoadStudentInClass(int ClassId)
    {
        var enrollments = await _mediatorGrades.GetEnrollmentsByClassAsync(ClassId);
        var studentIds = enrollments.Select(enrollmentProvider => enrollmentProvider.UserId).ToList();

        _studentsInClass = studentIds.Any() ? await _mediatorGrades.GetStudentsByIdsAsync(studentIds) : new List<ModelsUser>();

        InvokeChangedProperty();
    }

    private async Task LoadGradesForStudent(int StudentId) { _grades = await _mediatorGrades.GetGradesByStudentAsync(StudentId); UpdatedFilteredGrades(); }

    private async Task LoadStudentGrades(int StudentId)
    {
        _grades = await _mediatorGrades.GetGradesByStudentAsync(StudentId);
        FilteredGrades = new List<ModelsGrades>(_grades);

        var subjectsIds = _grades.Select(gradeProvider => gradeProvider.SubjectId).Distinct().ToList();
        _availableSubjects = subjectsIds.Any() ? await _mediatorGrades.GetSubjectsByIdsAsync(subjectsIds) : new List<ModelsSubjects>();

        InvokeChangedProperty();
    }

    private void UpdatedFilteredGrades()
    {
        FilteredGrades = SelectedSubject == null ? new List<ModelsGrades>(_grades) : _grades.Where(gradeProvider => gradeProvider.SubjectId == SelectedSubject.Id).ToList();
        InvokeChangedProperty();
    }

    private void InvokeChangedProperty() => PropertyChanged?.Invoke();
}