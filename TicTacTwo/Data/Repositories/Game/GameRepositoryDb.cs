using System.Globalization;
using System.Text.Json;
using Data.Context;
using Data.Models;
using GameBrain;

namespace Data.Repositories.Game;

public class GameRepositoryDb(GameDbContext dbContext, IConfigRepository configRepository) : IGameRepository
{
    public List<string> GetSavedGamesNames()
    {
        return dbContext.SavedGames.Select(game => game.Name.ToString()).ToList();
    }

    public GameState? GetGameStateByName(string savedGameName)
    {
        var game = dbContext.SavedGames.FirstOrDefault(game => game.Name == savedGameName);
        return game != null 
            ? JsonSerializer.Deserialize<GameState>(game.JsonState) 
            : null;
    }

    public void SaveGame(GameState gameState, string savedGameName)
    {
        var configName = gameState.GameConfiguration.Name;
        var configuration = dbContext.Configurations
            .FirstOrDefault(config => config.Name == configName);
        
        if (configuration == null)
        {
            throw new InvalidOperationException($"Configuration '{configName}' does not exist.");
        }
        
        var jsonData = JsonSerializer.Serialize(gameState);
        var game = new SaveGame
        {
            Name = savedGameName,
            CreatedAtDateTime  = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture),
            JsonState = jsonData,
            ConfigurationId = configuration.Id
        };

        dbContext.SavedGames.Add(game);
        dbContext.SaveChanges();
    }
}
