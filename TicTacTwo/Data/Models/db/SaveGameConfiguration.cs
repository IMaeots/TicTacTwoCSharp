using System.ComponentModel.DataAnnotations;

namespace Data.Models.db;

public class SaveGameConfiguration
{
    public int Id { get; set; }

    [MaxLength(128)]
    public required string Name { get; set; }

    [MaxLength(10000)]
    public required string JsonConfiguration { get; set; } = default!;

    // ReSharper disable once CollectionNeverUpdated.Global
    public ICollection<SaveGame>? SavedGames { get; set; }
}
