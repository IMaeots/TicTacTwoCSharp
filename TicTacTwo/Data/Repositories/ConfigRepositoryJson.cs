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
                new (Name: "Classical", WinCondition: 3, BoardWidth: 5, BoardHeight: 5, GridWidth: 3, GridHeight: 3, MoveGridAfterNMoves: 2, NumberOfMarkers: 4),
                new (Name: "Big Board", WinCondition: 4, BoardWidth: 10, BoardHeight: 10, GridWidth: 4, GridHeight: 4, MoveGridAfterNMoves: 4, NumberOfMarkers: 6, UserInputStartingPlayer: 2)
            };
            
            foreach (var config in defaultGameConfigurations)
            {
                SaveConfig(config);
            }
        }
    }
}
