using ConsoleApp;
using Data.Repositories;

var menuSystem = new ConsoleMenuSystem(new ConfigRepositoryHardcoded());

menuSystem.Run();
