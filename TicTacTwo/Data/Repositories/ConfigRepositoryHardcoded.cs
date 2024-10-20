using Common.Entities;
using GameBrain;

namespace Data.Repositories;

public class ConfigRepositoryHardcoded : IConfigRepository
{
    private readonly List<GameConfiguration> _gameConfigurations =
    [
        new (Name: "Classical"),
        new (Name: "Big Board", WinCondition: 4, BoardWidth: 10, BoardHeight: 10, GridWidth: 4, GridHeight: 4, MoveGridAfterNMoves: 4, StartingPlayer: EGamePiece.Player2),
    ];

    public List<string> GetConfigurationNames()
    {
        return _gameConfigurations
            .OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase)
            .Select(config => config.Name)
            .ToList();
    }


    public GameConfiguration? GetConfigurationByName(string name)
    {
        var config = _gameConfigurations.SingleOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        return config;
    }

    public void SaveConfig(GameConfiguration? newConfig)
    {
        if (newConfig != null) _gameConfigurations.Add(newConfig);
    }
}
