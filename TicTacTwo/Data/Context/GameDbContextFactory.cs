using Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Data.Context;

public class GameDbContextFactory : IDesignTimeDbContextFactory<GameDbContext>
{
    public GameDbContext CreateDbContext(string[] args)
    {
        if (!Directory.Exists(Constants.DatabaseDirectory)) Directory.CreateDirectory(Constants.DatabaseDirectory);

        var optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();
        
        var connectionString = "DataSource=<%location%>app.db;Cache=Shared";
        connectionString = connectionString.Replace("<%location%>", Constants.DatabaseDirectory);
        optionsBuilder.UseSqlite(connectionString);

        return new GameDbContext(optionsBuilder.Options);
    }
}
