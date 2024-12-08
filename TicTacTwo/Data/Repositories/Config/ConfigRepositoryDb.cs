using System.Text.Json;
using Common.Entities;
using Data.Context;
using Data.Models.db;
using GameLogic;

namespace Data.Repositories.Config;

public class ConfigRepositoryDb(GameDbContext dbContext) : IConfigRepository
{
    public List<string> GetConfigurationNames()
    {
        CheckAndCreateInitialConfig();

        return dbContext.SavedGameConfigurations.Select(config => config.Name.ToString()).ToList();
    }

    public GameConfiguration? GetConfigurationByName(string name)
    {
        var configuration = dbContext.SavedGameConfigurations.FirstOrDefault(config => config.Name.ToString() == name);
        return configuration != null
            ? JsonSerializer.Deserialize<GameConfiguration>(configuration.JsonConfiguration)
            : null;
    }

    public void SaveConfig(GameConfiguration newConfig)
    {
        var jsonData = JsonSerializer.Serialize(newConfig);
        var configuration = new SaveGameConfiguration
        {
            Name = newConfig.Name,
            JsonConfiguration = jsonData
        };

        dbContext.SavedGameConfigurations.Add(configuration);
        dbContext.SaveChanges();
    }

    private void CheckAndCreateInitialConfig()
    {
        if (dbContext.SavedGameConfigurations.ToList().Count != 0) return;

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
