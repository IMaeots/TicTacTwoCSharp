using System.Text.Json;
using Common;
using Data.Models.file;
using GameLogic;

namespace Data.Repositories.Game;

public class GameRepositoryJson : IGameRepository
{
    public GameRepositoryJson()
    {
        EnsureDirectoriesExist();
    }

    public async Task<List<string>> GetSavedGamesNamesAsync()
    {
        EnsureDirectoriesExist();
        
        var files = await Task.Run(() => Directory.GetFiles(Constants.GamesPath, "*" + Constants.JsonFileExtension));
        return files
            .Select(Path.GetFileNameWithoutExtension)
            .Where(name => name != null)
            .Cast<string>()
            .ToList();
    }

    public async Task<GameLogic.Game> GetSavedGameByNameAsync(string gameName)
    {
        EnsureDirectoriesExist();
        
        var filePath = GetGameFilePath(gameName);
        if (!File.Exists(filePath)) throw new FileNotFoundException($"Game {gameName} not found.");

        try
        {
            var savedGameJsonStr = await File.ReadAllTextAsync(filePath);
            var savedGame = JsonSerializer.Deserialize<JsonGameModel>(savedGameJsonStr);

            if (savedGame == null) throw new FileNotFoundException($"Game {gameName} was not found.");

            var configuration = GetGameConfiguration(savedGame.JsonConfiguration);
            var state = GetGameState(savedGame.JsonGameStates.Last());

            return new GameLogic.Game(
                savedGame.Name,
                configuration,
                state,
                savedGame.PasswordP1,
                savedGame.PasswordP2
            );
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Error parsing game data for {gameName}: {ex.Message}", ex);
        }
        catch (Exception ex) when (ex is not FileNotFoundException and not InvalidOperationException)
        {
            throw new InvalidOperationException($"Unexpected error loading game {gameName}: {ex.Message}", ex);
        }
    }

    public async Task SaveNewGameAsync(GameLogic.Game game)
    {
        EnsureDirectoriesExist();
        
        var configFilePath = GetConfigurationFilePath(game.Configuration.Name);
        if (!File.Exists(configFilePath))
            throw new InvalidOperationException($"Configuration '{game.Configuration.Name}' does not exist.");

        var gameFilePath = GetGameFilePath(game.Name);
        if (File.Exists(gameFilePath)) throw new InvalidOperationException($"Game '{game.Name}' already exists.");

        try
        {
            var jsonGameState = JsonSerializer.Serialize(game.State);
            var jsonGameModel = new JsonGameModel
            {
                Name = game.Name,
                JsonConfiguration = await File.ReadAllTextAsync(configFilePath),
                JsonGameStates = [jsonGameState],
                PasswordP1 = game.PasswordP1,
                PasswordP2 = game.PasswordP2
            };

            await File.WriteAllTextAsync(gameFilePath, JsonSerializer.Serialize(jsonGameModel));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to save game '{game.Name}': {ex.Message}", ex);
        }
    }

    public async Task SaveGameStateAsync(GameLogic.Game game)
    {
        EnsureDirectoriesExist();
        
        var filePath = GetGameFilePath(game.Name);
        if (!File.Exists(filePath))
            throw new FileNotFoundException("The game file does not exist and cannot be updated.", filePath);

        try
        {
            var savedGameJsonStr = await File.ReadAllTextAsync(filePath);
            var savedGame = JsonSerializer.Deserialize<JsonGameModel>(savedGameJsonStr);
            if (savedGame == null)
                throw new InvalidOperationException($"Game '{game.Name}' data could not be deserialized.");

            var updatedJsonGameState = JsonSerializer.Serialize(game.State);
            savedGame.JsonGameStates.Add(updatedJsonGameState);

            await File.WriteAllTextAsync(filePath, JsonSerializer.Serialize(savedGame));
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Error parsing game data for {game.Name}: {ex.Message}", ex);
        }
        catch (Exception ex) when (ex is not FileNotFoundException and not InvalidOperationException)
        {
            throw new InvalidOperationException($"Unexpected error saving game state for {game.Name}: {ex.Message}", ex);
        }
    }

    public async Task DeleteGameAsync(GameLogic.Game game)
    {
        EnsureDirectoriesExist();
        
        var filePath = GetGameFilePath(game.Name);
        if (!File.Exists(filePath)) throw new FileNotFoundException($"Game '{game.Name}' does not exist.", filePath);

        try
        {
            await Task.Run(() => File.Delete(filePath));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to delete game '{game.Name}': {ex.Message}", ex);
        }
    }

    public async Task EditGameNameAsync(GameLogic.Game game, string newName)
    {
        EnsureDirectoriesExist();
        
        var oldFilePath = GetGameFilePath(game.Name);
        if (!File.Exists(oldFilePath))
            throw new FileNotFoundException($"Game '{game.Name}' does not exist.", oldFilePath);

        var newFilePath = GetGameFilePath(newName);
        if (File.Exists(newFilePath))
            throw new InvalidOperationException($"Game with name '{newName}' already exists.");

        try
        {
            await Task.Run(() => File.Move(oldFilePath, newFilePath));

            var savedGameJsonStr = await File.ReadAllTextAsync(newFilePath);
            var savedGame = JsonSerializer.Deserialize<JsonGameModel>(savedGameJsonStr);
            if (savedGame == null)
                throw new InvalidOperationException($"Game '{newName}' data could not be deserialized.");

            savedGame.Name = newName;
            await File.WriteAllTextAsync(newFilePath, JsonSerializer.Serialize(savedGame));
        }
        catch (Exception ex) when (ex is not FileNotFoundException and not InvalidOperationException)
        {
            throw new InvalidOperationException($"Failed to rename game from '{game.Name}' to '{newName}': {ex.Message}", ex);
        }
    }

    private static string GetGameFilePath(string gameName) =>
        Path.Combine(Constants.GamesPath, gameName + Constants.JsonFileExtension);

    private static string GetConfigurationFilePath(string configName) =>
        Path.Combine(Constants.ConfigurationsPath, configName + Constants.JsonFileExtension);

    private static GameConfiguration GetGameConfiguration(string jsonConfiguration)
    {
        try
        {
            return JsonSerializer.Deserialize<GameConfiguration>(jsonConfiguration) ??
                   throw new InvalidOperationException("Failed to deserialize game configuration.");
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Failed to deserialize game configuration: " + ex.Message, ex);
        }
    }

    private static GameState GetGameState(string jsonGameState)
    {
        try
        {
            return JsonSerializer.Deserialize<GameState>(jsonGameState) ??
                   throw new InvalidOperationException("Failed to deserialize game state.");
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Failed to deserialize game state: " + ex.Message, ex);
        }
    }
    
    private static void EnsureDirectoriesExist()
    {
        if (!Directory.Exists(Constants.GamesPath)) Directory.CreateDirectory(Constants.GamesPath);
        if (!Directory.Exists(Constants.ConfigurationsPath)) Directory.CreateDirectory(Constants.ConfigurationsPath);
    }
}
