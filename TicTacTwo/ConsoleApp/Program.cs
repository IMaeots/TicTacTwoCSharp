using Common;
using ConsoleApp.MenuSystem;
using Data.Context;
using Data.Repositories;
using Data.Repositories.Config;
using Data.Repositories.Game;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

var serviceCollection = new ServiceCollection();
var useDatabase = bool.Parse(configuration["UseDatabase"] ?? "true");
if (useDatabase)
{
    Directory.CreateDirectory(Constants.DatabaseDirectory);
    
    var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                           throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    connectionString = connectionString.Replace("<%location%>", Constants.DatabaseDirectory);

    serviceCollection.AddDbContext<GameDbContext>(options => options.UseSqlite(connectionString));
    
    serviceCollection.AddSingleton<IGameRepository, GameRepositoryDb>();
    serviceCollection.AddSingleton<IConfigRepository, ConfigRepositoryDb>();
}
else
{
    Directory.CreateDirectory(Constants.GamesPath);
    Directory.CreateDirectory(Constants.ConfigurationsPath);
    
    serviceCollection.AddSingleton<IGameRepository, GameRepositoryJson>();
    serviceCollection.AddSingleton<IConfigRepository, ConfigRepositoryJson>();
}

serviceCollection.AddSingleton<ConsoleMenuSystem>();
var serviceProvider = serviceCollection.BuildServiceProvider();

if (useDatabase)
{
    try
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GameDbContext>();
        dbContext.Database.Migrate();
        Console.WriteLine("Database migration completed successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during database migration: {ex.Message}");
    }
}

var menuSystem = serviceProvider.GetRequiredService<ConsoleMenuSystem>();
menuSystem.Run();
