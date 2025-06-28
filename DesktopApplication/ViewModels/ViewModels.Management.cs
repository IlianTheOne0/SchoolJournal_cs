namespace DesktopApplication.ViewModels.Management;

using DesktopApplication.Interfaces.Services.Management;
using Models.Tables.Classes;
using Models.Tables.Users;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Media3D;

public class ViewModelsManagement : INotifyPropertyChanged
{
    private readonly InterfacesServicesManagement _servicesManagement = null!;

    private readonly ModelsEducationalInstitutions _nothingEdu = new ModelsEducationalInstitutions { Id = -1, Name = "None" };
    private readonly ModelsEducationalInstitutions _addNewEdu = new ModelsEducationalInstitutions { Id = -2, Name = "Add New" };

    private readonly ModelsClasses _nothingClass = new ModelsClasses { Id = -1, Name = "None" };
    private readonly ModelsClasses _addNewClass = new ModelsClasses { Id = -2, Name = "Add New" };

    private readonly ModelsUserExtended _nothingUser = new ModelsUserExtended { Id = -1, FullName = "None" };
    private readonly ModelsUserExtended _addNewUser = new ModelsUserExtended { Id = -2, FullName = "Add New" };

    private List<ModelsEducationalInstitutions> _availableEdu; public List<ModelsEducationalInstitutions> AvailableEdu { get => _availableEdu; private set { _availableEdu = value; OnPropertyChanged(); } }
    private List<ModelsClasses> _availableClasses; public List<ModelsClasses> AvailableClasses { get => _availableClasses; private set { _availableClasses = value; OnPropertyChanged(); } }
    private List<ModelsUserExtended> _availableUsers; public List<ModelsUserExtended> AvailableUsers { get => _availableUsers; private set { _availableUsers = value; OnPropertyChanged(); } }

    private bool _isChosenEdu; public bool IsChosenEdu { get => _isChosenEdu; set { _isChosenEdu = value; OnPropertyChanged(); } }
    private bool _isChosenClass; public bool IsChosenClass { get => _isChosenClass; set { _isChosenClass = value; OnPropertyChanged(); } }

    private ModelsEducationalInstitutions _chosenEdu; public ModelsEducationalInstitutions ChosenEdu { get => _chosenEdu; set { _chosenEdu = value; OnPropertyChanged(); OnEduChoise(value); } }
    private ModelsClasses _chosenClass; public ModelsClasses ChosenClass { get => _chosenClass; set { _chosenClass = value; OnPropertyChanged(); OnClassChoise(value); } }
    private ModelsUserExtended _chosenUser; public ModelsUserExtended ChosenUser { get => _chosenUser; set { _chosenUser = value; OnPropertyChanged(); OnStudentChoise(value); } }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ViewModelsManagement(InterfacesServicesManagement ServiceGrades) { _servicesManagement = ServiceGrades; LoadData(); }

    public void LoadData()
    {
        try
        {
            _servicesManagement.Load();

            InsertIntoEdu(); InsertIntoClasses(); InsertIntoUsers();

            _isChosenEdu = false; _isChosenClass = false;
            ChosenEdu = _nothingEdu; ChosenClass = _nothingClass; ChosenUser = _nothingUser;
        }
        catch (Exception e) { MessageBox.Show($"Load data failed: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void InsertIntoEdu()
    {
        var edu = _servicesManagement.AvailableEdu.ToList();
        edu.Insert(0, _addNewEdu);
        edu.Insert(0, _nothingEdu);
        AvailableEdu = edu;
    }

    private void InsertIntoClasses()
    {
        var classes = _servicesManagement.AvailableClasses.ToList();
        classes.Insert(0, _addNewClass);
        classes.Insert(0, _nothingClass);
        AvailableClasses = classes;
    }

    private void InsertIntoUsers()
    {
        var users = _servicesManagement.AvailableUsers.ToList();
        users.Insert(0, _addNewUser);
        users.Insert(0, _nothingUser);
        AvailableUsers = users;
    }

    private async void OnEduChoise(ModelsEducationalInstitutions ModelEdu)
    {
        try
        {
            IsChosenEdu = false;
            if (ModelEdu == null || ModelEdu.Id <= 0) { return; }
            
            IsChosenEdu = true;
            await _servicesManagement.GetAllClassesByEduId(ModelEdu.Id); InsertIntoClasses();
        }
        catch (Exception e) { MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private async void OnClassChoise(ModelsClasses ModelClass)
    {
        try
        {
            IsChosenClass = false;
            if (ModelClass == null || ModelClass.Id <= 0) { return; }

            IsChosenClass = true;
            await _servicesManagement.GetAllUsersByClassId(ModelClass.Id); InsertIntoUsers();
        }
        catch (Exception e) { MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void OnStudentChoise(ModelsUserExtended Student)
    {

    }

    private void OnPropertyChanged([CallerMemberName] string? PropertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
}