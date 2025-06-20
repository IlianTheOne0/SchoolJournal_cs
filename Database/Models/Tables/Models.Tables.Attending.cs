namespace Database.Models.Tables.Attending;

using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

[Table("Attending")]
public class ModelsAttending : BaseModel
{
    [PrimaryKey("Id")]
    public int Id { get; set; }

    [Column("Date")]
    public DateTime Date { get; set; }

    [Column("Present")]
    public bool Present { get; set; }

    [Column("UserId")]
    public int UserId { get; set; }

    [Column("SubjectId")]
    public int SubjectId { get; set; }
}