namespace Models.Tables.Attending;

using Models.Supports.SupabaseCommands;

using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

[Table("Attending")]
public class ModelsAttending : BaseModel, InterfacesModelsWithId
{
    [PrimaryKey("Id")]
    public int Id { get; set; }
    
    [Column("Sickness")]
    public bool Sickness { get; set; }

    [Column("Date")]
    public DateTime Date { get; set; }

    [Column("UserId")]
    public int UserId { get; set; }

    [Column("SubjectId")]
    public int SubjectId { get; set; }
}