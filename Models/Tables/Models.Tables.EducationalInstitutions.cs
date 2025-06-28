namespace Models.Tables.Classes;

using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

[Table("EducationalInstitutions")]
public class ModelsEducationalInstitutions : BaseModel
{
    [PrimaryKey("Id")]
    public int Id { get; set; }

    [Column("Name")]
    public string Name { get; set; }
}