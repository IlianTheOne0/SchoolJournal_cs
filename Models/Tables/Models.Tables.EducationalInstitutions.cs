namespace Models.Tables.EducationalInstitutions;

using Models.Supports.SupabaseCommands;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

[Table("EducationalInstitutions")]
public class ModelsEducationalInstitutions : BaseModel, InterfacesModelsWithId
{
    [PrimaryKey("Id")]
    public int Id { get; set; }

    [Column("Name")]
    public string Name { get; set; }
}