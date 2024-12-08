using System.Text.Json;
using Common;
using Common.Entities;
using GameLogic;

namespace Data.Repositories.Config;

public class ConfigRepositoryJson : IConfigRepository
{
    public List<string> GetConfigurationNames()
    {
        CheckAndCreateInitialConfig();

        return Directory
            .GetFiles(Constants.ConfigurationsPath, "*" + Constants.JsonFileExtension)
            .Select(Path.GetFileNameWithoutExtension)
            .Where(name => name != null)
            .Cast<string>()
            .ToList();
    }

    public GameConfiguration? GetConfigurationByName(string name)
    {
        var configJsonStr = File.ReadAllText(Constants.ConfigurationsPath + name + Constants.JsonFileExtension);
        var config = JsonSerializer.Deserialize<GameConfiguration>(configJsonStr);
        return config;
    }

    public void SaveConfig(GameConfiguration newConfig) =>
        File.WriteAllText(
            path: Path.Combine(Constants.ConfigurationsPath, newConfig.Name + Constants.JsonFileExtension),
            contents: JsonSerializer.Serialize(newConfig)
        );

    private void CheckAndCreateInitialConfig()
    {
        if (!Directory.Exists(Constants.ConfigurationsPath))
            Directory.CreateDirectory(Constants.ConfigurationsPath);

        var data = Directory.GetFiles(Constants.ConfigurationsPath, "*" + Constants.JsonFileExtension).ToList();
        if (data.Count != 0) return;

        var defaultGameConfigurations = new List<GameConfiguration>
        {
            new(Name: "Classical", Mode: EGameMode.LocalTwoPlayer, StartingPlayer: EGamePiece.Player1, WinCondition: 3,
                BoardWidth: 5, BoardHeight: 5, GridWidth: 3, GridHeight: 3, UnlockSpecialMovesAfterNMoves: 2,
                NumberOfMarkers: 4, StartingGridXPosition: 1, StartingGridYPosition: 1),
            new(Name: "Big Board", Mode: EGameMode.LocalTwoPlayer, StartingPlayer: EGamePiece.Player2, WinCondition: 4,
                BoardWidth: 10, BoardHeight: 10, GridWidth: 4, GridHeight: 4, UnlockSpecialMovesAfterNMoves: 4,
                NumberOfMarkers: 6, StartingGridXPosition: 3, StartingGridYPosition: 3)
        };

        foreach (var config in defaultGameConfigurations)
        {
            SaveConfig(config);
        }
    }
}
