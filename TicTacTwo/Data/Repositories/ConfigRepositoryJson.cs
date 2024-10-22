using System.Text.Json;
using Common;
using Common.Entities;
using GameBrain;

namespace Data.Repositories;

public class ConfigRepositoryJson : IConfigRepository
{
    public List<string> GetConfigurationNames()
    {
        CheckAndCreateInitialConfig();

        return Directory
            .GetFiles(Constants.BasePath, "*" + Constants.ConfigFileExtension)
            .Select(fullFileName =>
                Path.GetFileNameWithoutExtension(
                    Path.GetFileNameWithoutExtension(fullFileName)
                )
            )
            .ToList();
    }

    public GameConfiguration? GetConfigurationByName(string name)
    {
        var configJsonStr = File.ReadAllText(Constants.BasePath + name + Constants.ConfigFileExtension);
        var config = JsonSerializer.Deserialize<GameConfiguration>(configJsonStr);
        return config;
    }

    public void SaveConfig(GameConfiguration newConfig)
    {
        var configJsonStr = JsonSerializer.Serialize(newConfig);
        File.WriteAllText(Path.Combine(Constants.BasePath, newConfig.Name + Constants.ConfigFileExtension), configJsonStr);
    }

    private void CheckAndCreateInitialConfig()
    {
        if (!Directory.Exists(Constants.BasePath))
        {
            Directory.CreateDirectory(Constants.BasePath);
        }

        var data = Directory.GetFiles(Constants.BasePath, "*" + Constants.ConfigFileExtension).ToList();
        if (data.Count == 0)
        {
            var defaultGameConfigurations = new List<GameConfiguration>
            {
                new (Name: "Classical"),
                new (Name: "Big Board", WinCondition: 4, BoardWidth: 10, BoardHeight: 10, GridWidth: 4, GridHeight: 4,
                    MoveGridAfterNMoves: 4, StartingPlayer: EGamePiece.Player2)
            };
            
            foreach (var config in defaultGameConfigurations)
            {
                SaveConfig(config);
            }
        }
    }
}
