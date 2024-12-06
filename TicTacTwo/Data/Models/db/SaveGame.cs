using System.ComponentModel.DataAnnotations;
using Common.Entities;

namespace Data.Models.db;

public class SaveGame
{
    public int Id { get; set; }
    
    [MaxLength(128)]
    public required string Name { get; set; }
    
    public required int ConfigurationId { get; set; }
    public SaveGameConfiguration Configuration { get; set; } = default!;
    
    public required List<string> JsonGameStates { get; set; } = default!;
    
    [MaxLength(128)]
    public string? PasswordP1 { get; set; }
    [MaxLength(128)]
    public string? PasswordP2 { get; set; }
    
    public DateTime CreatedAtDateTime { get; } = DateTime.UtcNow;
}
