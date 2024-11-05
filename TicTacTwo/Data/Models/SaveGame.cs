using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class SaveGame
{
    public int Id { get; set; }
    
    [MaxLength(128)]
    public required string Name { get; set; }

    public required string CreatedAtDateTime { get; set; } = default!;
    
    public required string JsonState { get; set; } = default!;
    
    public int ConfigurationId { get; set; }
    public SaveGameConfiguration Configuration { get; set; } = default!;
}