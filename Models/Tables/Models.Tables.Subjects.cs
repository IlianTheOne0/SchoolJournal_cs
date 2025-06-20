namespace Models.Tables.Subjects;

using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

[Table("Subjects")]
public class ModelsSubjects : BaseModel
{
    [PrimaryKey("Id")]
    public int Id { get; set; }

    [Column("Name")]
    public string Name { get; set; }

    [Column("TeacherId")]
    public int TeacherId { get; set; }

    [Column("ClassId")]
    public int ClassId { get; set; }
}