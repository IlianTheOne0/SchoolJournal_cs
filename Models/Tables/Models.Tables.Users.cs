namespace Models.Tables.Users;

using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

[Table("Users")]
public class ModelsUser : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Column("Username")]
    public string Username { get; set; }

    [Column("FullName")]
    public string FullName { get; set; }

    [Column("Email")]
    public string Email { get; set; }

    [Column("PhoneNumber")]
    public string PhoneNumber { get; set; }

    [Column("Sex")]
    public bool Sex { get; set; }

    [Column("DateOfBirth")]
    public DateTime DateOfBirth { get; set; }

    [Column("CreatedAt")]
    public DateTime CreatedAt { get; set; }

    [Column("DateOfTheLastUpdate")]
    public DateTime DateOfTheLastUpdate { get; set; }

    [Column("DateOfTheLastVisitToTheJorunal")]
    public DateTime DateOfTheLastVisitToTheJorunal { get; set; }

    [Column("AvatarUrl")]
    public string? AvatarUrl { get; set; } = null;

    [Column("StatusId")]
    public int StatusId { get; set; }

    [Column("EducationalInstitutionId")]
    public int EducationalInstitutionId { get; set; }

    [Column("ProfileId")]
    public string ProfileId { get; set; }

    public string? StatusName { get; set; } = null;
    public string? EducationalInstitutionName { get; set; } = null;
}