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

var useDatabase = bool.Parse(configuration["UseDatabase"] ?? "true");

var serviceCollection = new ServiceCollection();

if (useDatabase)
{
    serviceCollection.AddDbContext<GameDbContext>(options => 
        options.UseSqlite($"Data Source={configuration["DatabasePath"] ?? "TicTacTwo.db"}"));
    
    serviceCollection.AddSingleton<IGameRepository, GameRepositoryDb>();
    serviceCollection.AddSingleton<IConfigRepository, ConfigRepositoryDb>();
}
else
{
    serviceCollection.AddSingleton<IGameRepository, GameRepositoryJson>();
    serviceCollection.AddSingleton<IConfigRepository, ConfigRepositoryJson>();
}

serviceCollection.AddSingleton<ConsoleMenuSystem>();
var serviceProvider = serviceCollection.BuildServiceProvider();

if (useDatabase)
{
    using var scope = serviceProvider.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<GameDbContext>();
    dbContext.Database.Migrate();
}

var menuSystem = serviceProvider.GetRequiredService<ConsoleMenuSystem>();
menuSystem.Run();
