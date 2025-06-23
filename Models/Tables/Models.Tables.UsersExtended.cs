namespace Models.Tables.Users.Extended;

using Models.Tables.Users;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class ModelsUserExtended : ModelsUser, INotifyPropertyChanged
{
    private string _username; public override string Username { get => _username; set { _username = value; OnPropertyChanged(); } }
    private string _fullName; public override string FullName { get => _fullName; set { _fullName = value; OnPropertyChanged(); }  }
    private string _phoneNumber; public override string PhoneNumber { get => _phoneNumber; set { _phoneNumber = value; OnPropertyChanged(); }  }
    private bool _sex; public override bool Sex { get => _sex; set { _sex = value; OnPropertyChanged(); } }
    private DateTime _dateOfBirth; public override DateTime DateOfBirth { get => _dateOfBirth; set { _dateOfBirth = value; OnPropertyChanged(); } }

    public string? StatusName { get; set; }
    public string? EducationalInstitutionName { get; set; }

    public ModelsUserExtended() { }

    public ModelsUserExtended(ModelsUser user, string statusName, string educationalInstitutionName)
    {
        Id = user.Id;
        Username = user.Username;
        FullName = user.FullName;
        Email = user.Email;
        PhoneNumber = user.PhoneNumber;
        Sex = user.Sex;
        DateOfBirth = user.DateOfBirth;
        CreatedAt = user.CreatedAt;
        DateOfTheLastUpdate = user.DateOfTheLastUpdate;
        DateOfTheLastVisitToTheJorunal = user.DateOfTheLastVisitToTheJorunal;
        AvatarUrl = user.AvatarUrl;
        StatusId = user.StatusId;
        EducationalInstitutionId = user.EducationalInstitutionId;
        ProfileId = user.ProfileId;

        StatusName = statusName;
        EducationalInstitutionName = educationalInstitutionName;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
}