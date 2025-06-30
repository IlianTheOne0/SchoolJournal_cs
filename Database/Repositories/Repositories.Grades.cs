namespace Database.Repositories.Grades;

using Database.Interfaces.Repositories.Grades;
using Database.Interfaces.Repositories.Supabase;
using Models.Tables.Attending;
using Models.Tables.Classes;
using Models.Tables.EducationalInstitutions;
using Models.Tables.Enrollments;
using Models.Tables.Grades;
using Models.Tables.Statuses;
using Models.Tables.Subjects;
using Models.Tables.Users;
using System.Threading.Tasks;
using static global::Supabase.Postgrest.Constants;

public class RepositoriesGrades : InterfacesRepositoriesGrades
{
    private readonly InterfacesRepositoriesSupabase _repositorySupabase;

    public RepositoriesGrades(InterfacesRepositoriesSupabase RepositorySupabase) => _repositorySupabase = RepositorySupabase;

    public async Task<List<ModelsClasses>> GetAllClassesByEducationalInstitution(int EducationalInstitutionId)
    {
        try { return await _repositorySupabase.FilterAsync<ModelsClasses>("EducationalInstitutionId", Operator.Equals, EducationalInstitutionId); }
        catch (Exception E) { throw new Exception($"Failed to get all classe by educational institution id: {E.Message}", E); }
    }

    public async Task<List<ModelsSubjects>> GetAllSubjectsByEducationalInstitution(bool IsTeacher, int EducationalInstitutionId)
    {
        try
        {
            if (IsTeacher)
            {
                var classes = await _repositorySupabase.FilterAsync<ModelsClasses>("EducationalInstitutionId", Operator.Equals, EducationalInstitutionId);

                var tasks = classes.Select(async item => await _repositorySupabase.FilterAsync<ModelsSubjects>("ClassId", Operator.Equals, item.Id));

                var results = await Task.WhenAll(tasks);
                var subjects = results.SelectMany(list => list).ToList();

                return subjects;
            }
            else
            {
                var enrollments = await _repositorySupabase.FilterAsync<ModelsEnrollments>("UserId", Operator.Equals, _repositorySupabase.ModelUser!.Id);
                if (enrollments == null || enrollments.Count == 0) { return new List<ModelsSubjects>(); }

                var classId = enrollments[0].ClassId;

                return await _repositorySupabase.FilterAsync<ModelsSubjects>("ClassId", Operator.Equals, classId);
            }
        }
        catch (Exception E) { throw new Exception($"Failed to get all subjects by educational institution id: {E.Message}", E); }
    }

    public async Task<List<ModelsUserExtended>> GetAllStudentsByClassId(int ClassId)
    {
        try
        {
            var enrollments = await _repositorySupabase.FilterAsync<ModelsEnrollments>("ClassId", Operator.Equals, ClassId);

            if (enrollments == null || enrollments.Count == 0) { return new List<ModelsUserExtended>(); }

            var studentIds = enrollments.Select(e => e.UserId).ToList();
            var students = new List<ModelsUserExtended>();

            foreach (var studentId in studentIds)
            {
                var userResult = await _repositorySupabase.FilterAsync<ModelsUser>("Id", Operator.Equals, studentId);

                var user = userResult?.FirstOrDefault();
                if (user == null) { continue; }

                var statusResult = await _repositorySupabase.FilterAsync<ModelsStatuses>("Id", Operator.Equals, user.StatusId);
                var status = statusResult?.FirstOrDefault();

                var institutionResult = await _repositorySupabase.FilterAsync<ModelsEducationalInstitutions>("Id", Operator.Equals, user.EducationalInstitutionId);
                var institution = institutionResult?.FirstOrDefault();

                students.Add(
                    new ModelsUserExtended(user, status?.Status ?? "Unknown", institution?.Name ?? "Unknown")
                );
            }

            return students;
        }
        catch (Exception E) { throw new Exception($"Failed to get students by class ID: {E.Message}", E); }
    }

    public async Task<List<ModelsGradesExtended>> GetAllGradesByStudentId(int StudentId)
    {
        try
        {
            var grades = await _repositorySupabase.FilterAsync<ModelsGrades>("UserId", Operator.Equals, StudentId);

            if (grades == null || grades.Count == 0) { return new List<ModelsGradesExtended>(); }

            var extendedGrades = new List<ModelsGradesExtended>();
            foreach (var grade in grades)
            {
                var subject = (await _repositorySupabase.FilterAsync<ModelsSubjects>("Id", Operator.Equals, grade.SubjectId))?.FirstOrDefault();

                if (subject != null) { extendedGrades.Add(new ModelsGradesExtended(grade, subject.Name)); }
            }

            return extendedGrades;
        }
        catch (Exception E) { throw new Exception($"Failed to get grades by student ID: {E.Message}", E); }
    }

    public async Task<List<ModelsGradesExtended>> GetAllGradesBySubjectAndStudent(int SubjectId, int StudentId)
    {
        try
        {
            var grades = await _repositorySupabase.FilterAsync<ModelsGrades>("UserId", Operator.Equals, StudentId);

            var subjectGrades = grades
                .Where(gradesProvider => gradesProvider.SubjectId == SubjectId)
                .ToList();

            var subject = (await _repositorySupabase.FilterAsync<ModelsSubjects>("Id", Operator.Equals, SubjectId))?.FirstOrDefault();

            return subjectGrades.Select(gradesProvider => new ModelsGradesExtended(gradesProvider, subject?.Name ?? "Unknown")).ToList();
        }
        catch (Exception E) { throw new Exception($"Failed to get grades by student and subject: {E.Message}", E); }
    }

    public async Task<List<ModelsSubjects>> GetAllSubjectsByClass(int ClassId)
    {
        try { return await _repositorySupabase.FilterAsync<ModelsSubjects>("ClassId", Operator.Equals, ClassId); }
        catch (Exception E) { throw new Exception($"Failed to get existing grades: {E.Message}", E); }
    }

    public async Task<List<ModelsGrades>> GetExistingGrades(int StudentId)
    {
        try { return await _repositorySupabase.FilterAsync<ModelsGrades>("UserId", Operator.Equals, StudentId); }
        catch (Exception E) { throw new Exception($"Failed to get existing grades: {E.Message}", E); }
    }

    public async Task Insert(ModelsGrades Item)
    {
        try { await _repositorySupabase.Insert(Item); }
        catch(Exception E) { throw new Exception($"Failed to insert grades: {E.Message}", E); }
    }
    public async Task Update(ModelsGrades Item)
    {
        try { await _repositorySupabase.Upsert(Item, new[] { "Date", "UserId", "SubjectId" }); }
        catch (Exception E) { throw new Exception($"Failed to update grades: {E.Message}", E); }
    }

    public async Task DeleteGrade(int StudentId, int SubjectId, DateTime Date)
    {
        try
        {
            var existingGrades = await _repositorySupabase.FilterAsync<ModelsGrades>("UserId", Operator.Equals, StudentId);

            var gradeToDelete = existingGrades.FirstOrDefault(
                gradesProvider => gradesProvider.UserId == StudentId && gradesProvider.Date.Date == Date.Date && gradesProvider.SubjectId == SubjectId
            );

            if (gradeToDelete != null) { await _repositorySupabase.Delete(gradeToDelete); }
        }
        catch (Exception E) { throw new Exception($"Failed to delete grade: {E.Message}", E); }
    }

    public async Task DeleteAttendance(int StudentId, int SubjectId, DateTime Date)
    {
        try
        {
            var existing = await _repositorySupabase.FilterAsync<ModelsAttending>("UserId", Operator.Equals, StudentId);

            var attendanceToDelete = existing.FirstOrDefault(
                attendaceProvider => attendaceProvider.UserId == StudentId && attendaceProvider.Date.Date == Date.Date && attendaceProvider.SubjectId == SubjectId
            );

            if (attendanceToDelete != null) { await _repositorySupabase.Delete(attendanceToDelete); }
        }
        catch (Exception E) { throw new Exception($"Failed to delete attendance: {E.Message}", E); }
    }

    public async Task InsertAttendance(ModelsAttending Attendance)
    {
        try { await _repositorySupabase.Upsert(Attendance, new[] { "Date", "UserId", "SubjectId" }); }
        catch (Exception E) { throw new Exception($"Failed to insert attendance: {E.Message}", E); }
    }

    public async Task<List<ModelsAttending>> GetAttendanceByClass(int ClassId, int SubjectId, int Month, int Year)
    {
        try
        {
            var startDate = new DateTime(Year, Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var conditions = new List<(string, Operator, object)>
            {
                ("SubjectId", Operator.Equals, SubjectId),
                ("Date", Operator.GreaterThanOrEqual, startDate.ToString("yyyy-MM-dd")),
                ("Date", Operator.LessThanOrEqual, endDate.ToString("yyyy-MM-dd"))
            };

            return await _repositorySupabase.FilterAsync<ModelsAttending>(conditions);
        }
        catch (Exception E) { throw new Exception($"Failed to get attendance: {E.Message}", E); }
    }

    public async Task<List<ModelsAttending>> GetAttendanceByStudent(int StudentId)
    {
        try { return await _repositorySupabase.FilterAsync<ModelsAttending>("UserId", Operator.Equals, StudentId); }
        catch (Exception E) { throw new Exception($"Failed to get attendance by student: {E.Message}", E); }
    }
}