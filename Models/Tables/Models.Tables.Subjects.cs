namespace Models.Tables.Subjects;

using Models.Supports.SupabaseCommands;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

[Table("Subjects")]
public class ModelsSubjects : BaseModel, InterfacesModelsWithId
{
    [PrimaryKey("Id")]
    public int Id { get; set; }

    [Column("Name")]
    public virtual string Name { get; set; }

    [Column("TeacherId")]
    public int TeacherId { get; set; }

    [Column("ClassId")]
    public int ClassId { get; set; }
}