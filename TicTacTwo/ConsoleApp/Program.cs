using ConsoleApp;
using Data.Context;
using Data.Repositories;
using Data.Repositories.Config;
using Data.Repositories.Game;
using Microsoft.EntityFrameworkCore;

IGameRepository gameRepository;
IConfigRepository configRepository;

const bool useDatabase = true;
if (useDatabase)
{
    var dbContext = new GameDbContextFactory().CreateDbContext([]);
    dbContext.Database.Migrate();
    
    configRepository = new ConfigRepositoryDb(dbContext);
    gameRepository = new GameRepositoryDb(dbContext);
}
else
{
    gameRepository = new GameRepositoryJson();
    configRepository = new ConfigRepositoryJson();
}

var menuSystem = new ConsoleMenuSystem(configRepository, gameRepository);

menuSystem.Run();
