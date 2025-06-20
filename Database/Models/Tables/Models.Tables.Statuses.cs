namespace Database.Models.Tables.Statuses;

using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

[Table("Statuses")]
public class ModelsStatuses : BaseModel
{
    [PrimaryKey("Id")]
    public int Id { get; set; }

    [Column("Status")]
    public string Status { get; set; }
}