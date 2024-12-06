namespace Data.Repositories;

public interface IGameRepository
{
    List<string> GetSavedGamesNames();
    GameLogic.Game GetSavedGameByName(string gameName);
    void SaveNewGame(GameLogic.Game game);
    void SaveGameState(GameLogic.Game game);
    void DeleteGame(GameLogic.Game game);
}
