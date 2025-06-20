namespace DesktopApplication.Services.Strategies.TeacherAccess;

using DesktopApplication.Interfaces.AccessStrategy;
using Models.Tables.Users;

public class ServicesStrategiesTeacherAccess : InterfacesAccessStrategy
{
    public ModelsUser ModelUser { get; set; }

    public ServicesStrategiesTeacherAccess(ModelsUser ModelUser) { this.ModelUser = ModelUser; }

    public bool CanGrade() => true;
    public bool CanViewGrades() => true;
    public bool CanManageUsers() => false;
}