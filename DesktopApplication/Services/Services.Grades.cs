namespace DesktopApplication.Services.Grades;

using Database.Repositories.Grades;
using Database.Interfaces.Repositories.Grades;
using DesktopApplication.Interfaces.Services.Grades;
using DesktopApplication.Interfaces.Services.User;
using DesktopApplication.Services.Supabase;
using Models.Tables.Classes;
using Models.Tables.Grades;
using Models.Tables.Subjects;
using Models.Tables.Users;
using DesktopApplication.Services.Strategies.TeacherAccess;

public class ServicesGrades : InterfacesServicesGrades
{
    private readonly InterfacesRepositoriesGrades _repositoryGrades;
    private readonly InterfacesServicesUser _serviceUser;

    public int CurrentUserId { get; private set; }

    public List<ModelsClasses> AvailableClasses { get; private set; } = new();
    public List<ModelsUserExtended> AvailableStudents { get; private set; } = new();
    public List<ModelsSubjects> AvailableSubjects { get; private set; } = new();
    public List<ModelsGradesExtended> AvailableGrades { get; private set; } = new();

    private bool _isTeacherMode;

    public ServicesGrades(ServicesSupabase ServiceSupabase, ServicesUser ServiceUser)
    {
        _repositoryGrades = new RepositoriesGrades(ServiceSupabase.RepositorySupabase);
        _serviceUser = ServiceUser;

        CurrentUserId = _serviceUser.AccessStrategy?.ModelUser.Id ?? -1;
    }

    public bool GetIsTeacherMode() => _isTeacherMode;

    public async Task Initialize()
    {
        try
        {
            CurrentUserId = _serviceUser.AccessStrategy?.ModelUser.Id ?? -1;

            _isTeacherMode = false;

            if (_serviceUser.AccessStrategy is ServicesStrategiesTeacherAccess)
            {
                _isTeacherMode = true;

                AvailableClasses = await _repositoryGrades.GetAllClassesByEducationalInstitution(_serviceUser.AccessStrategy!.ModelUser.EducationalInstitutionId);
            }
            AvailableSubjects = await _repositoryGrades.GetAllSubjectsByEducationalInstitution(_isTeacherMode, _serviceUser.AccessStrategy!.ModelUser.EducationalInstitutionId);
        }
        catch (Exception e) { throw new Exception($"Grades initialize failed: {e.Message}", e); }
    }
    public async Task Refresh()
    {
        try { await Initialize(); }
        catch (Exception e) { throw new Exception($"Grades refreshment failed: {e.Message}", e); }
    }

    public async Task<List<ModelsUserExtended>> GetAllStudentsByClassId(int ClassId)
    {
        try { AvailableStudents = await _repositoryGrades.GetAllStudentsByClassId(ClassId); return AvailableStudents; }
        catch (Exception e) { throw new Exception($"Getting students by class failed: {e.Message}", e); }
    }
    public async Task<List<ModelsGradesExtended>> GetAllGradesByStudentId(int ClassId)
    {
        try { AvailableGrades = await _repositoryGrades.GetAllGradesByStudentId(ClassId); return AvailableGrades; }
        catch (Exception e) { throw new Exception($"Grades refreshment failed: {e.Message}", e); }
    }
    public async Task<List<ModelsGradesExtended>> GetAllGradesBySubjectAndStudent(int SubjectId, int StudentId)
    {
        try { AvailableGrades = await _repositoryGrades.GetAllGradesBySubjectAndStudent(SubjectId, StudentId); return AvailableGrades; }
        catch (Exception e) { throw new Exception($"Grades refreshment failed: {e.Message}", e); }
    }
}