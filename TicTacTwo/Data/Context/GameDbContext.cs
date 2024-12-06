using Data.Models.db;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;
    
public class GameDbContext(DbContextOptions<GameDbContext> options) : DbContext(options)
{
    public DbSet<SaveGame> SavedGames { get; set; } = null!;
    public DbSet<SaveGameConfiguration> SavedGameConfigurations { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SaveGameConfiguration>()
            .HasIndex(config => config.Name)
            .IsUnique();
        
        modelBuilder.Entity<SaveGame>()
            .HasIndex(game => game.Name)
            .IsUnique();
        
        modelBuilder.Entity<SaveGame>()
            .HasOne(game => game.Configuration)
            .WithMany(config => config.SavedGames)
            .HasForeignKey(game => game.ConfigurationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SaveGame>()
            .HasOne(game => game.Configuration)
            .WithMany(config => config.SavedGames) 
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }
}
