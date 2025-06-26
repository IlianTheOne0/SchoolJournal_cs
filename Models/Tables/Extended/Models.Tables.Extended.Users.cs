namespace Models.Tables.Users;

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

    public ModelsUserExtended(ModelsUser User, string StatusName, string EducationalInstitutionName)
    {
        Id = User.Id;
        Username = User.Username;
        FullName = User.FullName;
        Email = User.Email;
        PhoneNumber = User.PhoneNumber;
        Sex = User.Sex;
        DateOfBirth = User.DateOfBirth;
        CreatedAt = User.CreatedAt;
        DateOfTheLastUpdate = User.DateOfTheLastUpdate;
        DateOfTheLastVisitToTheJournal = User.DateOfTheLastVisitToTheJournal;
        AvatarUrl = User.AvatarUrl;
        StatusId = User.StatusId;
        EducationalInstitutionId = User.EducationalInstitutionId;
        ProfileId = User.ProfileId;

        this.StatusName = StatusName;
        this.EducationalInstitutionName = EducationalInstitutionName;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName)); }
}