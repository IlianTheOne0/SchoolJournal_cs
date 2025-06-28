namespace Models.Tables.Classes;

using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

[Table("Classes")]
public class ModelsClasses : BaseModel
{
    [PrimaryKey("Id")]
    public int Id { get; set; }

    [Column("Name")]
    public string Name { get; set; }

    [Column("Year")]
    public int Year { get; set; }
    
    [Column("EducationalInstitutionId")]
    public int EducationalInstitutionId { get; set; }
}