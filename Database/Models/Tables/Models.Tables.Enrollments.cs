namespace Database.Models.Tables.Enrollments;

using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

[Table("Enrollments")]
public class ModelsEnrollments : BaseModel
{
    [PrimaryKey("Id")]
    public int Id { get; set; }

    [Column("UserId")]
    public int UserId { get; set; }

    [Column("ClassId")]
    public int ClassId { get; set; }
}