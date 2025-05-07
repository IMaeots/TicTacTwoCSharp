using System.Text.Json;
using Data.Context;
using Data.Models.db;
using GameLogic;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Game;

public class GameRepositoryDb(GameDbContext dbContext) : IGameRepository
{
    public async Task<List<string>> GetSavedGamesNamesAsync()
    {
        try
        {
            return await dbContext.SavedGames
                .Select(game => game.Name)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to retrieve saved games", ex);
        }
    }

    public async Task<GameLogic.Game> GetSavedGameByNameAsync(string gameName)
    {
        try
        {
            var savedGame = await dbContext.SavedGames
                                .AsNoTracking()
                                .Include(saveGame => saveGame.Configuration)
                                .FirstOrDefaultAsync(game => game.Name == gameName)
                            ?? throw new KeyNotFoundException($"Game {gameName} not found.");

            var configuration = JsonSerializer.Deserialize<GameConfiguration>(savedGame.Configuration.JsonConfiguration);
            var state = JsonSerializer.Deserialize<GameState>(savedGame.JsonGameStates.Last());

            if (configuration == null || state == null)
                throw new ArgumentNullException($"Game configuration or state is missing or invalid.");

            return new GameLogic.Game(savedGame.Name, configuration, state, savedGame.PasswordP1, savedGame.PasswordP2);
        }
        catch (Exception ex) when (ex is not KeyNotFoundException and not ArgumentNullException)
        {
            throw new InvalidOperationException($"Failed to retrieve game {gameName}", ex);
        }
    }

    public async Task SaveNewGameAsync(GameLogic.Game game)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            var configName = game.Configuration.Name;
            var configuration = await dbContext.SavedGameConfigurations
                .FirstOrDefaultAsync(config => config.Name == configName);

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

            await dbContext.SavedGames.AddAsync(newSave);
            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new InvalidOperationException($"Failed to save new game {game.Name}", ex);
        }
    }

    public async Task SaveGameStateAsync(GameLogic.Game game)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            var dbGame = await dbContext.SavedGames
                             .FirstOrDefaultAsync(g => g.Name == game.Name)
                         ?? throw new InvalidOperationException($"Game '{game.Name}' does not exist.");

            var updatedJsonGameState = JsonSerializer.Serialize(game.State);
            dbGame.JsonGameStates.Add(updatedJsonGameState);

            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new InvalidOperationException($"Failed to update game state for {game.Name}", ex);
        }
    }

    public async Task DeleteGameAsync(GameLogic.Game game)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            var dbGame = await dbContext.SavedGames
                             .FirstOrDefaultAsync(g => g.Name == game.Name)
                         ?? throw new InvalidOperationException($"Game '{game.Name}' does not exist.");

            dbContext.SavedGames.Remove(dbGame);
            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new InvalidOperationException($"Failed to delete game {game.Name}", ex);
        }
    }

    public async Task EditGameNameAsync(GameLogic.Game game, string newName)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            var dbGame = await dbContext.SavedGames
                             .FirstOrDefaultAsync(g => g.Name == game.Name)
                         ?? throw new InvalidOperationException($"Game '{game.Name}' does not exist.");

            if (await dbContext.SavedGames.AnyAsync(g => g.Name == newName))
                throw new InvalidOperationException($"A game with name '{newName}' already exists.");

            dbGame.Name = newName;

            dbContext.SavedGames.Update(dbGame);
            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new InvalidOperationException($"Failed to rename game from {game.Name} to {newName}", ex);
        }
    }
}
