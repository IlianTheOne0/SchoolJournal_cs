namespace Models.Tables.Classes;

using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

[Table("Classes")]
public class ModelsClasses : BaseModel
{
    [PrimaryKey("Id")]
    public int Id { get; set; }

    [Column("Name")]
    public string Name { get; set; }

    [Column("Year")]
    public int Year { get; set; }
}