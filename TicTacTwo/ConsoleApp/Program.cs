using ConsoleApp;
using Data.Repositories;

var menuSystem = new ConsoleMenuSystem(new ConfigRepositoryJson());

menuSystem.Run();
