using Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Data.Context;

public class GameDbContextFactory : IDesignTimeDbContextFactory<GameDbContext>
{
    public GameDbContext CreateDbContext(string[] args)
    {
        var dbDirectory = Path.Combine(Constants.BasePath, "TicTacTwo_Data");
        if (!Directory.Exists(dbDirectory))
        {
            Directory.CreateDirectory(dbDirectory);
        }

        var connectionString = $@"Data Source={Path.Combine(dbDirectory, "TicTacTwo.db")}";
            
        var contextOptions = new DbContextOptionsBuilder<GameDbContext>()
            .UseSqlite(connectionString)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging()
            .Options;

        return new GameDbContext(contextOptions);
    }
}
