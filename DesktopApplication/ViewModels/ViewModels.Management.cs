namespace DesktopApplication.ViewModels.Management;

using DesktopApplication.Interfaces.Services.Management;
using Models.Supports.Management;
using Models.Supports.SupabaseCommands;
using Models.Tables.Classes;
using Models.Tables.EducationalInstitutions;
using Models.Tables.Statuses;
using Models.Tables.Subjects;
using Models.Tables.Users;
using Supabase.Postgrest.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

public partial class ViewModelsManagement : INotifyPropertyChanged
{
    private readonly InterfacesServicesManagement _servicesManagement = null!;

    private readonly ModelsEducationalInstitutions _nothingEdu = new ModelsEducationalInstitutions { Id = -1, Name = "None" };
    private readonly ModelsEducationalInstitutions _addNewEdu = new ModelsEducationalInstitutions { Id = -2, Name = "Add New" };

    private readonly ModelsClasses _nothingClass = new ModelsClasses { Id = -1, Name = "None" };
    private readonly ModelsClasses _addNewClass = new ModelsClasses { Id = -2, Name = "Add New" };

    private readonly ModelsUserExtended _nothingUser = new ModelsUserExtended { Id = -1, FullName = "None" };
    private readonly ModelsUserExtended _addNewUser = new ModelsUserExtended { Id = -2, FullName = "Add New" };

    private List<ModelsEducationalInstitutions> _availableEdu = new(); public List<ModelsEducationalInstitutions> AvailableEdu { get => _availableEdu; private set { _availableEdu = value; OnPropertyChanged(); } }
    private List<ModelsClasses> _availableClasses = new(); public List<ModelsClasses> AvailableClasses { get => _availableClasses; private set { _availableClasses = value; OnPropertyChanged(); } }
    private List<ModelsUserExtended> _availableUsers = new(); public List<ModelsUserExtended> AvailableUsers { get => _availableUsers; private set { _availableUsers = value; OnPropertyChanged(); } }
    private List<ModelsUserExtended> _availableUsersWithNone = new(); public List<ModelsUserExtended> AvailableUsersWithNone { get => _availableUsersWithNone; private set { _availableUsersWithNone = value; OnPropertyChanged(); } }
    private List<ModelsUserExtended> _availableTeachers = new(); public List<ModelsUserExtended> AvailableTeachers { get => _availableTeachers; set { _availableTeachers = value; OnPropertyChanged(); } }
    private List<ModelsSubjectsExtended> _availableSubjects = new(); public List<ModelsSubjectsExtended> AvailableSubjects { get => _availableSubjects; set { _availableSubjects = value; OnPropertyChanged(); } }
    private List<ModelsStatuses> _availableStatuses = new(); public List<ModelsStatuses> AvailableStatuses { get => _availableStatuses; set { _availableStatuses = value; OnPropertyChanged(); } }

    private bool _isChosenEdu; public bool IsChosenEdu { get => _isChosenEdu; set { _isChosenEdu = value; OnPropertyChanged(); } }

    private ModelsEducationalInstitutions _chosenEdu; public ModelsEducationalInstitutions ChosenEdu { get => _chosenEdu; set { _chosenEdu = value; OnPropertyChanged(); OnEduChoise(value); UpdateState(); ChosenClass = _nothingClass; ChosenUser = _nothingUser; } }
    private ModelsClasses _chosenClass; public ModelsClasses ChosenClass { get => _chosenClass; set { _chosenClass = value; OnPropertyChanged(); OnClassChoise(value); UpdateState(); ChosenUser = _nothingUser; } }
    private ModelsUserExtended _chosenUser; public ModelsUserExtended ChosenUser { get => _chosenUser; set { _chosenUser = value; OnPropertyChanged(); UpdateState(); } }
    private ModelsSubjectsExtended _chosenSubject; public ModelsSubjectsExtended ChosenSubject { get => _chosenSubject; set { _chosenSubject = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsTeacherComboEnabled)); } }

    public event PropertyChangedEventHandler? PropertyChanged;

    private bool _isEditing0 = false; public bool IsEditing0 { get => _isEditing0; set { _isEditing0 = value; OnPropertyChanged(); } }
    private bool _isEditing1 = false; public bool IsEditing1 { get => _isEditing1; set { _isEditing1 = value; OnPropertyChanged(); } }
    private ManagementState _currentState; public ManagementState CurrentState { get => _currentState; set { _currentState = value; OnPropertyChanged(); } }
    private readonly List<ManagementRule> _rules = new()
    {
        new((edu, cls, usr) => edu == -1 && cls == -1 && usr == -1,     ManagementState.NoneSelected),
        new((edu, cls, usr) => edu == -2 && cls == -1 && usr == -1,     ManagementState.AddNewEdu),
        new((edu, cls, usr) => edu > 0   && cls == -1 && usr == -1,     ManagementState.ExistingEdu),
        new((edu, cls, usr) => edu > 0   && cls == -2 && usr == -1,     ManagementState.AddNewClass),
        new((edu, cls, usr) => edu > 0   && cls > 0   && usr == -1,     ManagementState.ExistingClass),
        new((edu, cls, usr) => edu > 0   && cls > 0   && usr == -2,     ManagementState.AddNewUser),
        new((edu, cls, usr) => edu > 0   && cls == -1 && usr > 0  ,     ManagementState.ExistingUser),
        new((edu, cls, usr) => edu > 0   && cls > 0   && usr > 0  ,     ManagementState.ExistingUser)
    };
    public bool IsTeacherComboEnabled => ChosenSubject != null && IsEditing1;

    public ViewModelsManagement(InterfacesServicesManagement ServiceGrades) { _servicesManagement = ServiceGrades; HardReset(); CommandInitialize(); }

    public async Task LoadData()
    {
        try
        {
            await _servicesManagement.Load();
            IsEditing0 = false; IsEditing1 = false;

            InsertIntoEdu(); InsertIntoClasses(); InsertIntoUsers();
            await OnStatuses();
        }
        catch (Exception e) { MessageBox.Show($"Load data failed: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    public void HardReset()
    {
        _isChosenEdu = false;
        ChosenEdu = _nothingEdu; ChosenClass = _nothingClass; ChosenUser = _nothingUser;
    }

    private async Task LoadTeachers()
    {
        await _servicesManagement.GetAllTeachersByEduId(ChosenEdu.Id);
        AvailableTeachers = _servicesManagement.AvailableTeachers
            .OrderBy(teachersProvider => teachersProvider.FullName)
            .ToList();
    }

    public void InsertIntoEdu()
    {
        var edu = _servicesManagement.AvailableEdu;
        edu.Insert(0, _addNewEdu);
        edu.Insert(0, _nothingEdu);
        AvailableEdu = edu;
    }

    public void InsertIntoClasses()
    {
        var classes = _servicesManagement.AvailableClasses;
        classes.Insert(0, _addNewClass);
        classes.Insert(0, _nothingClass);
        AvailableClasses = classes;
    }

    public void InsertIntoUsers()
    {
        var users = _servicesManagement.AvailableUsers;
        users.Insert(0, _addNewUser);
        users.Insert(0, _nothingUser);
        AvailableUsers = users;
    }

    private void UpdateState()
    {
        var eduId = ChosenEdu?.Id ?? -1; var classId = ChosenClass?.Id ?? -1; var userId = ChosenUser?.Id ?? -1;
        CurrentState = _rules.FirstOrDefault(rule => rule.Condition(eduId, classId, userId))?.State ?? ManagementState.NoneSelected;
    }

    private async void OnEduChoise(ModelsEducationalInstitutions ModelEdu)
    {
        try
        {
            IsChosenEdu = false;
            if (ModelEdu == null || ModelEdu.Id <= 0) { return; }
            IsChosenEdu = true;

            await _servicesManagement.GetAllClassesByEduId(ModelEdu.Id); InsertIntoClasses();
            await LoadUsersByEduId();
        }
        catch (Exception E) { MessageBox.Show($"OnEduChoise failed: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private async void OnClassChoise(ModelsClasses ModelClass)
    {
        try
        {
            if (ModelClass == null || ModelClass.Id <= 0) { return; }

            await LoadTeachers();
            await OnSubjects(ModelClass.Id);
            await _servicesManagement.GetAllUsersByClassId(ModelClass.Id);
            var users = _servicesManagement.AvailableUsers;
            users.Insert(0, _addNewUser);
            users.Insert(0, _nothingUser);
            AvailableUsersWithNone = users;
        }
        catch (Exception E) { MessageBox.Show($"OnClassChoise failed: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    public async Task OnSubjects(int ClassId)
    {
        ModelsEducationalInstitutions edu = ChosenEdu; ModelsClasses cl = ChosenClass;

        await _servicesManagement.GetAllSubjectsByClassId(ClassId);
        AvailableSubjects = _servicesManagement.AvailableSubjects
            .Select(subjectsProvider => new ModelsSubjectsExtended(
                subjectsProvider, AvailableTeachers.FirstOrDefault(teachersProvider => teachersProvider.Id == subjectsProvider.TeacherId)?.FullName ?? "Unknown")
            )
            .OrderBy(subjectsProvider => subjectsProvider.Name)
            .ToList();

        ChosenSubject = null;
        if (cl != _nothingClass && cl != _addNewClass && ChosenClass?.Id != cl.Id) { ChosenClass = cl; }
    }

    public async Task OnStatuses()
    {
        await _servicesManagement.GetAllStatuses();
        AvailableStatuses = _servicesManagement.AvailableStatuses;
    }

    private async Task LoadUsersByEduId()
    {
        try
        {
            if (ChosenEdu == null || ChosenEdu.Id <= 0) { AvailableUsers = new(); AvailableUsersWithNone = new() { _nothingUser }; return; }

            await _servicesManagement.GetAllUsersByEduId(ChosenEdu.Id);

            var users = _servicesManagement.AvailableUsers
                .Where(usersProvider => usersProvider.StatusId == 1 || usersProvider.StatusId == 2)
                .OrderBy(usersProvider => usersProvider.FullName)
                .ToList();

            AvailableUsers = users;

            var usersWithNone = new List<ModelsUserExtended> { _nothingUser };
            usersWithNone.AddRange(users);
            AvailableUsersWithNone = usersWithNone;
        }
        catch (Exception E) { MessageBox.Show($"Failed to load users by Edu ID: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }


    public async Task Add<TModel>(TModel Model)
        where TModel : BaseModel, InterfacesModelsWithId, new()
    {
        try { await _servicesManagement.Add(Model); }
        catch (Exception E) { MessageBox.Show($"Adding failed: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    public async Task AddSubject(ModelsSubjects Model)
    {
        try { await _servicesManagement.Add(Model); }
        catch (Exception E) { MessageBox.Show($"Adding failed: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    public async Task AddUser(ModelsUser User, string Password)
    {
        try { await _servicesManagement.AddUser(User, Password, ChosenClass.Id); }
        catch (Exception E) { MessageBox.Show($"Adding failed: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    public async Task Edit<TModel>(TModel Model, string[] ConflictColumns)
        where TModel : BaseModel, InterfacesModelsWithId, new()
    {
        try { await _servicesManagement.Edit(Model, ConflictColumns); }
        catch (Exception E) { MessageBox.Show($"Edititng failed: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    public async Task Delete<TModel>(TModel Model)
        where TModel : BaseModel, InterfacesModelsWithId, new()
    {
        try { await _servicesManagement.Delete(Model); HardReset(); }
        catch (Exception E) { MessageBox.Show($"Deleting failed: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    public async Task OperationsWithEdu(string? EduName = null)
    { 
        await LoadData();
        
        if (EduName != null) { ChosenEdu = AvailableEdu.FirstOrDefault(eduProvider => eduProvider.Name == EduName); }
    }
    public async Task OperationsWithClasses(string? EduName = null, string? ClassName = null)
    {
        await OperationsWithEdu(EduName);
        await Task.Delay(100);
        if (ClassName != null) { ChosenClass = AvailableClasses.FirstOrDefault(classesProvider => classesProvider.Name == ClassName); }
    }

    public async Task OperationsWithUsers(string? EduName = null, string? ClassName = null, string? UserName = null)
    {
        await OperationsWithClasses(EduName, ClassName);
        await Task.Delay(700);
        if (UserName != null) { ChosenUser = AvailableUsers.FirstOrDefault(usersProvider => usersProvider.FullName == UserName); }
    }

    private void OnPropertyChanged([CallerMemberName] string? PropertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
}