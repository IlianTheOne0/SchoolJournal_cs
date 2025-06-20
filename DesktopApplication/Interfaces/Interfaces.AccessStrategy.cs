namespace DesktopApplication.Interfaces.AccessStrategy;

using Models.Tables.Users;

public interface InterfacesAccessStrategy
{
    public ModelsUser ModelUser { get; set; }

    bool CanGrade();
    bool CanViewGrades();
    bool CanManageUsers();
}