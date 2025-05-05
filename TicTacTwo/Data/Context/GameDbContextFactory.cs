using Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Data.Context;

public class GameDbContextFactory : IDesignTimeDbContextFactory<GameDbContext>
{
    public GameDbContext CreateDbContext(string[] args)
    {
        try
        {
            if (!Directory.Exists(Constants.DatabaseDirectory)) 
                Directory.CreateDirectory(Constants.DatabaseDirectory);

            var optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();
            
            var connectionString = GetSqliteConnectionString();
            optionsBuilder.UseSqlite(connectionString);

            return new GameDbContext(optionsBuilder.Options);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error creating database context: {ex.Message}");
            throw;
        }
    }
    
    private static string GetSqliteConnectionString()
    {
        var baseConnectionString = "DataSource=<%location%>app.db;Cache=Shared";
        return baseConnectionString.Replace("<%location%>", Constants.DatabaseDirectory);
    }
}
