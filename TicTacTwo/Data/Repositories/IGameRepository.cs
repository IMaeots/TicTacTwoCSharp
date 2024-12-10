namespace Data.Repositories;

public interface IGameRepository
{
    Task<List<string>> GetSavedGamesNamesAsync();
    Task<GameLogic.Game> GetSavedGameByNameAsync(string gameName);
    Task SaveNewGameAsync(GameLogic.Game game);
    Task SaveGameStateAsync(GameLogic.Game game);
    Task DeleteGameAsync(GameLogic.Game game);
    Task EditGameNameAsync(GameLogic.Game game, string newName);
}
