namespace Models.Tables.Grades;

using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

[Table("Grades")]
public class ModelsGrades : BaseModel
{
    [PrimaryKey("Id")]
    public int Id { get; set; }

    [Column("Grade")]
    public int Grade { get; set; }

    [Column("Description")]
    public string? Description { get; set; } = null;

    [Column("Date")]
    public DateTime Date { get; set; }

    [Column("UserId")]
    public int UserId { get; set; }

    [Column("SubjectId")]
    public int SubjectId { get; set; }
}