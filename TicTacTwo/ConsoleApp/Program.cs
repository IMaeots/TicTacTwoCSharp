using ConsoleApp;
using Data.Repositories;

var menuSystem = new ConsoleMenuSystem(
    new ConfigRepositoryJson(),
    new GameRepositoryJson()
);

menuSystem.Run();
