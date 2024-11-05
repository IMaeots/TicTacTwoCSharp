using Microsoft.EntityFrameworkCore;

namespace Data.Models;
    
public class GameDbContext(DbContextOptions<GameDbContext> options) : DbContext(options)
{
    public DbSet<SaveGame> SavedGames { get; set; } = null!;
    public DbSet<SaveGameConfiguration> Configurations { get; set; } = null!;
}
