using System.Text.Json;
using Common;
using GameBrain;

namespace Data.Repositories.Game;

public class GameRepositoryJson : IGameRepository
{
    public GameRepositoryJson()
    {
        Directory.CreateDirectory(Constants.BasePath);
    }

    public List<string> GetSavedGamesNames() => Directory
        .GetFiles(Constants.BasePath, "*" + Constants.GameDataFileExtension)
        .Select(fullFileName =>
            Path.GetFileNameWithoutExtension(
                Path.GetFileNameWithoutExtension(fullFileName)
            )
        ).ToList();

    public GameState? GetGameStateByName(string savedGameName)
    {
        var filePath = Path.Combine(Constants.BasePath, savedGameName + Constants.GameDataFileExtension);
        var savedGameJsonStr = File.ReadAllText(filePath);
        
        var savedGameState = JsonSerializer.Deserialize<GameState>(savedGameJsonStr);
        return savedGameState;
    }

    public void SaveGame(GameState gameState, string savedGameName)
    {
        var filePath = Path.Combine(Constants.BasePath, savedGameName + Constants.GameDataFileExtension);
        File.WriteAllText(filePath, JsonSerializer.Serialize(gameState));
    }
}
