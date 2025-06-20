namespace DesktopApplication.Services.Strategies.StudentAccess;

using DesktopApplication.Interfaces.AccessStrategy;
using Models.Tables.Users;

public class ServicesStrategiesStudentAccess : InterfacesAccessStrategy
{
    public ModelsUser ModelUser { get; set; }

    public ServicesStrategiesStudentAccess(ModelsUser ModelUser) { this.ModelUser = ModelUser; }

    public bool CanGrade() => false;
    public bool CanViewGrades() => true;
    public bool CanManageUsers() => false;
}