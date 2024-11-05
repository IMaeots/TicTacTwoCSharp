using ConsoleApp;
using Data;
using Data.Models;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

IGameRepository gameRepository;
IConfigRepository configRepository;

const bool useDatabase = true;
if (useDatabase)
{
    var dbContext = new GameDbContextFactory().CreateDbContext([]);
    dbContext.Database.Migrate();
    
    configRepository = new ConfigRepositoryDb(dbContext);
    gameRepository = new GameRepositoryDb(dbContext, configRepository);
}
else
{
    gameRepository = new GameRepositoryJson();
    configRepository = new ConfigRepositoryJson();
}

var menuSystem = new ConsoleMenuSystem(configRepository, gameRepository);

menuSystem.Run();
