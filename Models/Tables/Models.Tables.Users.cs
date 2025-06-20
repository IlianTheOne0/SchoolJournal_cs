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

    [Column("DateOfBirth")]
    public DateTime DateOfBirth { get; set; }

    [Column("CreatedAt")]
    public DateTime CreatedAt { get; set; }

    [Column("StatusId")]
    public int StatusId { get; set; }

    [Column("AvatarUrl")]
    public string? AvatarUrl { get; set; } = null;

    public string? StatusName { get; set; } = null;
}