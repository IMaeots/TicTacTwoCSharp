using GameBrain;

namespace Data.Repositories;

public class ConfigRepositoryHardcoded : IConfigRepository
{
    private readonly List<GameConfiguration> _gameConfigurations =
    [
        new GameConfiguration(Name: "Classical"),
        new GameConfiguration(Name: "Big Board", WinCondition: 4, BoardSize: 10, GridSize: 4, MoveGridAfterNMoves: 4)
    ];

    public List<string> GetConfigurationNames()
    {
        return _gameConfigurations
            .OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase)
            .Select(config => config.Name)
            .ToList();
    }


    public GameConfiguration GetConfigurationByName(string name)
    {
        var config = _gameConfigurations.SingleOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (config == null)
        {
            throw new ArgumentException($"Configuration with name '{{name}}' not found.");
            
        } 
        return config;
    }
}
