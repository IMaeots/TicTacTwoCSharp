using Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Data.Context;

public class GameDbContextFactory : IDesignTimeDbContextFactory<GameDbContext>
{
    public GameDbContext CreateDbContext(string[] args)
    {
        if (!Directory.Exists(Constants.DatabaseDirectory)) Directory.CreateDirectory(Constants.DatabaseDirectory);

        var connectionString = $@"Data Source={Path.Combine(Constants.DatabaseDirectory, "TicTacTwo.db")}";

        var contextOptions = new DbContextOptionsBuilder<GameDbContext>()
            .UseSqlite(connectionString)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging()
            .Options;

        return new GameDbContext(contextOptions);
    }
}
