namespace Contracts.Models.Tables.Users;

using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

[Table("Users")]
public class ModelsUser : BaseModel
{
    [PrimaryKey("Id")]
    public int Id { get; set; }

    [Column("Username")]
    public virtual string Username { get; set; }

    [Column("FullName")]
    public virtual string FullName { get; set; }

    [Column("Email")]
    public virtual string Email { get; set; }

    [Column("PhoneNumber")]
    public virtual string PhoneNumber { get; set; }

    [Column("Sex")]
    public virtual bool Sex { get; set; }

    [Column("DateOfBirth")]
    public virtual DateTime DateOfBirth { get; set; }

    [Column("CreatedAt")]
    public DateTime CreatedAt { get; set; }

    [Column("DateOfTheLastUpdate")]
    public DateTime DateOfTheLastUpdate { get; set; }

    [Column("DateOfTheLastVisitToTheJournal")]
    public DateTime DateOfTheLastVisitToTheJournal { get; set; }

    [Column("AvatarUrl")]
    public string? AvatarUrl { get; set; } = null;

    [Column("StatusId")]
    public int StatusId { get; set; }

    [Column("EducationalInstitutionId")]
    public int EducationalInstitutionId { get; set; }

    [Column("ProfileId")]
    public string ProfileId { get; set; }
}