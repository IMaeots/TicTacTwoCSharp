using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class SaveGameConfiguration
{
    public int Id { get; set; }
    
    [MaxLength(128)]
    public required string Name { get; set; }
    
    public required string JsonConfiguration { get; set; } = default!;
    
    public ICollection<SaveGame>? SavedGames { get; set; }
}