using System.Text.Json;
using Common;
using Data.Models.file;
using GameLogic;

namespace Data.Repositories.Game;

public class GameRepositoryJson : IGameRepository
{
    public GameRepositoryJson()
    {
        if (!Directory.Exists(Constants.GamesPath)) Directory.CreateDirectory(Constants.GamesPath);
        if (!Directory.Exists(Constants.ConfigurationsPath)) Directory.CreateDirectory(Constants.ConfigurationsPath);
    }

    public List<string> GetSavedGamesNames() => Directory
        .GetFiles(Constants.GamesPath, "*" + Constants.JsonFileExtension)
        .Select(Path.GetFileNameWithoutExtension)
        .Where(name => name != null)
        .Cast<string>()
        .ToList();

    public GameLogic.Game GetSavedGameByName(string gameName)
    {
        var filePath = GetGameFilePath(gameName);
        if (!File.Exists(filePath)) throw new FileNotFoundException($"Game {gameName} not found.");

        var savedGameJsonStr = File.ReadAllText(filePath);
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

    public void SaveNewGame(GameLogic.Game game)
    {
        var configFilePath = GetConfigurationFilePath(game.Configuration.Name);
        if (!File.Exists(configFilePath))
            throw new InvalidOperationException($"Configuration '{game.Configuration.Name}' does not exist.");

        var gameFilePath = GetGameFilePath(game.Name);
        if (File.Exists(gameFilePath)) throw new InvalidOperationException($"Game '{game.Name}' already exists.");

        var jsonGameState = JsonSerializer.Serialize(game.State);
        var jsonGameModel = new JsonGameModel
        {
            Name = game.Name,
            JsonConfiguration = File.ReadAllText(configFilePath),
            JsonGameStates = [jsonGameState],
            PasswordP1 = game.PasswordP1,
            PasswordP2 = game.PasswordP2
        };

        File.WriteAllText(gameFilePath, JsonSerializer.Serialize(jsonGameModel));
    }

    public void SaveGameState(GameLogic.Game game)
    {
        var filePath = GetGameFilePath(game.Name);
        if (!File.Exists(filePath))
            throw new FileNotFoundException("The game file does not exist and cannot be updated.", filePath);

        var savedGameJsonStr = File.ReadAllText(filePath);
        var savedGame = JsonSerializer.Deserialize<JsonGameModel>(savedGameJsonStr);
        if (savedGame == null)
            throw new InvalidOperationException($"Game '{game.Name}' data could not be deserialized.");

        var updatedJsonGameState = JsonSerializer.Serialize(game.State);
        savedGame.JsonGameStates.Add(updatedJsonGameState);

        File.WriteAllText(filePath, JsonSerializer.Serialize(savedGame));
    }

    public void DeleteGame(GameLogic.Game game)
    {
        var filePath = GetGameFilePath(game.Name);
        if (!File.Exists(filePath)) throw new FileNotFoundException($"Game '{game.Name}' does not exist.", filePath);

        File.Delete(filePath);
    }

    private string GetGameFilePath(string gameName) =>
        Path.Combine(Constants.GamesPath, gameName + Constants.JsonFileExtension);

    private string GetConfigurationFilePath(string configName) =>
        Path.Combine(Constants.ConfigurationsPath, configName + Constants.JsonFileExtension);

    private GameConfiguration GetGameConfiguration(string jsonConfiguration) =>
        JsonSerializer.Deserialize<GameConfiguration>(jsonConfiguration) ??
        throw new InvalidOperationException("Failed to deserialize game configuration.");

    private GameState GetGameState(string jsonGameState) =>
        JsonSerializer.Deserialize<GameState>(jsonGameState) ??
        throw new InvalidOperationException("Failed to deserialize game state.");
}
