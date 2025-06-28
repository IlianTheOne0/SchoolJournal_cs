namespace DesktopApplication.Services.Grades;

using DesktopApplication.Interfaces.Services.Grades;
using DesktopApplication.Interfaces.Services.Supabase;
using DesktopApplication.Interfaces.Services.User;
using DesktopApplication.Services.Strategies.TeacherAccess;

using Database.Interfaces.Repositories.Grades;
using Database.Repositories.Grades;

using Models.Tables.Attending;
using Models.Tables.Classes;
using Models.Tables.Grades;
using Models.Tables.Subjects;
using Models.Tables.Users;
using Models.Supports.GradesAssigner;
using System.Security.Cryptography;

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

    public ServicesGrades(InterfacesServicesSupabase ServiceSupabase, InterfacesServicesUser ServiceUser)
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
        catch (Exception E) { throw new Exception($"Grades initialize failed: {E.Message}", E); }
    }
    public async Task Refresh()
    {
        try { await Initialize(); }
        catch (Exception E) { throw new Exception($"Grades refreshment failed: {E.Message}", E); }
    }

    public async Task<List<ModelsUserExtended>> GetAllStudentsByClassId(int ClassId)
    {
        try { AvailableStudents = await _repositoryGrades.GetAllStudentsByClassId(ClassId); return AvailableStudents; }
        catch (Exception E) { throw new Exception($"Getting students by class failed: {E.Message}", E); }
    }
    public async Task<List<ModelsGradesExtended>> GetAllGradesByStudentId(int ClassId)
    {
        try { AvailableGrades = await _repositoryGrades.GetAllGradesByStudentId(ClassId); return AvailableGrades; }
        catch (Exception E) { throw new Exception($"Grades refreshment failed: {E.Message}", E); }
    }
    public async Task<List<ModelsGradesExtended>> GetAllGradesBySubjectAndStudent(int SubjectId, int StudentId)
    {
        try { AvailableGrades = await _repositoryGrades.GetAllGradesBySubjectAndStudent(SubjectId, StudentId); return AvailableGrades; }
        catch (Exception E) { throw new Exception($"Grades refreshment failed: {E.Message}", E); }
    }

    public async Task<List<ModelsClasses>> GetAvailableClasses()
    {
        try { return await _repositoryGrades.GetAllClassesByEducationalInstitution(_serviceUser.AccessStrategy!.ModelUser.EducationalInstitutionId); }
        catch (Exception E) { throw new Exception($"Getting of available classes failed: {E.Message}", E); }
    }
    public async Task<List<ModelsSubjects>> GetSubjectsByClass(int ClassId)
    {
        try { return await _repositoryGrades.GetAllSubjectsByClass(ClassId); }
        catch (Exception E) { throw new Exception($"Getting of available classes failed: {E.Message}", E); }
    }
    public async Task<List<StudentGradeAssignment>> GetStudentAssignments(int ClassId, int SubjectId, int Month, int Year)
    {
        try
        {
            var students = await _repositoryGrades.GetAllStudentsByClassId(ClassId);

            var startDate = new DateTime(Year, Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var attendanceRecords = await _repositoryGrades.GetAttendanceByClass(ClassId, SubjectId, Month, Year);

            var assignments = new List<StudentGradeAssignment>();

            foreach (var student in students)
            {
                var grades = await _repositoryGrades.GetAllGradesBySubjectAndStudent(SubjectId, student.Id);

                var studentAssignment = new StudentGradeAssignment
                {
                    StudentId = student.Id,
                    StudentName = student.FullName,
                    Grades = new Dictionary<DateTime, GradeAssignment>(),
                    Attendances = new Dictionary<DateTime, ModelsAttending>()
                };

                var studentAttendances = attendanceRecords
                    .Where(attendanceProvider => attendanceProvider.UserId == student.Id)
                    .GroupBy(attendanceProvider => attendanceProvider.Date.Date)
                    .ToDictionary(gradeProvider => gradeProvider.Key, gradeProvider => gradeProvider.First());

                for (var date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    var grade = grades.FirstOrDefault(gradeProvider => gradeProvider.Date.Date == date.Date);
                    studentAssignment.Grades[date] = new GradeAssignment
                    {
                        Grade = grade?.Grade,
                        Description = grade?.Description,
                        Date = date
                    };

                    if (studentAttendances.TryGetValue(date.Date, out var attendance)) { studentAssignment.Attendances[date.Date] = attendance; }
                }

                assignments.Add(studentAssignment);
            }

            return assignments;
        }
        catch (Exception E) { throw new Exception($"Getting of available classes failed: {E.Message}", E); }
    }

    public async Task UpdateGrade(int StudentId, int SubjectId, DateTime Date, int GradeValue, string Description)
    {
        try
        {
            var existingGrades = await _repositoryGrades.GetExistingGrades(StudentId);

            var existingGrade = existingGrades.FirstOrDefault(
                gradesProvider => gradesProvider.SubjectId == SubjectId && gradesProvider.Date.Date == Date.Date
            );

            var newGrade = new ModelsGrades
            {
                Id = RandomNumberGenerator.GetInt32(1234567890),
                UserId = StudentId,
                SubjectId = SubjectId,
                Date = Date,
                Grade = GradeValue,
                Description = Description
            };
            await _repositoryGrades.Update(newGrade);
        }
        catch (Exception E) { throw new Exception($"Updating grade failed: {E.Message}", E); }
    }

    public async Task DeleteGrade(int StudentId, int SubjectId, DateTime Date)
    {
        try
        {
            await _repositoryGrades.DeleteGrade(StudentId, SubjectId, Date);
            await Refresh();
        }
        catch (Exception E) { throw new Exception($"Deleting grade failed: {E.Message}", E); }
    }

    public async Task DeleteAttendance(int StudentId, int SubjectId, DateTime Date)
    {
        try{ await _repositoryGrades.DeleteAttendance(StudentId, SubjectId, Date); }
        catch (Exception E) { throw new Exception($"Deleting attendance failed: {E.Message}", E); }
    }
    public async Task InsertAttendance(ModelsAttending Attendance)
    {
        try { Attendance.Id = RandomNumberGenerator.GetInt32(1234567890); await _repositoryGrades.InsertAttendance(Attendance); }
        catch (Exception E) { throw new Exception($"Inserting attendance failed: {E.Message}", E); }
    }

    public async Task<List<ModelsAttending>> GetAttendanceByClass(int ClassId, int SubjectId, int Month, int Year)
    {
        try { return await _repositoryGrades.GetAttendanceByClass(ClassId, SubjectId, Month, Year); }
        catch (Exception E) { throw new Exception($"Failed to get attendance by class: {E.Message}", E); }
    }

    public async Task<List<ModelsAttending>> GetAttendanceByStudent(int StudentId)
    {
        try { return await _repositoryGrades.GetAttendanceByStudent(StudentId); }
        catch (Exception E) { throw new Exception($"Failed to get attendance by student: {E.Message}", E); }
    }
}