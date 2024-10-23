using GameBrain;

namespace Data;

public interface IGameRepository
{
    List<string> GetSavedGamesNames();
    GameState? GetGameStateByName(string savedGameName);
    void SaveGame(GameState gameState, string savedGameName);
}
