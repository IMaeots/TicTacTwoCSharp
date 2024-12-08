using System.Text.Json;
using Data.Context;
using Data.Models.db;
using GameLogic;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Game;

public class GameRepositoryDb(GameDbContext dbContext) : IGameRepository
{
    public List<string> GetSavedGamesNames() =>
        dbContext.SavedGames.Select(game => game.Name.ToString()).ToList();

    public GameLogic.Game GetSavedGameByName(string gameName)
    {
        var savedGame = dbContext.SavedGames
                            .Include(saveGame => saveGame.Configuration)
                            .FirstOrDefault(game => game.Name == gameName)
                        ?? throw new KeyNotFoundException($"Game {gameName} not found");

        var saveName = savedGame.Name;
        var configuration = JsonSerializer.Deserialize<GameConfiguration>(savedGame.Configuration.JsonConfiguration);
        var state = JsonSerializer.Deserialize<GameState>(savedGame.JsonGameStates.Last());
        var passwordP1 = savedGame.PasswordP1;
        var passwordP2 = savedGame.PasswordP2;

        if (configuration is null || state is null)
            throw new ArgumentNullException($"Game configuration and/or state was null.");

        return new GameLogic.Game(saveName, configuration, state, passwordP1, passwordP2);
    }

    public void SaveNewGame(GameLogic.Game game)
    {
        var configName = game.Configuration.Name;
        var configuration = dbContext.SavedGameConfigurations
            .FirstOrDefault(config => config.Name == configName);

        if (configuration == null)
            throw new InvalidOperationException($"Configuration '{configName}' does not exist.");

        var jsonGameState = JsonSerializer.Serialize(game.State);
        var newSave = new SaveGame
        {
            Name = game.Name,
            JsonGameStates = [jsonGameState],
            ConfigurationId = configuration.Id,
            PasswordP1 = game.PasswordP1,
            PasswordP2 = game.PasswordP2
        };

        dbContext.SavedGames.Add(newSave);
        dbContext.SaveChanges();
    }

    public void SaveGameState(GameLogic.Game game)
    {
        var dbGame = dbContext.SavedGames.FirstOrDefault(g => g.Name == game.Name)
                     ?? throw new InvalidOperationException($"Game '{game.Name}' does not exist.");

        var updatedJsonGameState = JsonSerializer.Serialize(game.State);
        dbGame.JsonGameStates.Add(updatedJsonGameState);

        dbContext.SaveChanges();
    }

    public void DeleteGame(GameLogic.Game game)
    {
        var dbGame = dbContext.SavedGames.FirstOrDefault(g => g.Name == game.Name)
                     ?? throw new InvalidOperationException($"Game '{game.Name}' does not exist.");

        dbContext.SavedGames.Remove(dbGame);
        dbContext.SaveChanges();
    }
}
