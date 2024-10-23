using System.Text.Json;
using Common;
using Common.Entities;
using GameBrain;

namespace Data.Repositories;

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
        
        if (savedGameState != null)
        {
            savedGameState.GameBoard = ConvertToMultidimensionalArray(savedGameState.SerializedGameBoard);
        }
        
        return savedGameState;
    }

    public void SaveGame(GameState gameState, string savedGameName)
    {
        gameState.SerializedGameBoard = ConvertToListOfLists(gameState.GameBoard); // JsonSerializer can not serialize/deserialize multidimensional arrays...

        var filePath = Path.Combine(Constants.BasePath, savedGameName + Constants.GameDataFileExtension);
        File.WriteAllText(filePath, JsonSerializer.Serialize(gameState));
    }
    
    private EGamePiece[,] ConvertToMultidimensionalArray(List<List<EGamePiece>> list)
    {
        var board = new EGamePiece[list.Count, list[0].Count];

        for (var i = 0; i < list.Count; i++)
        {
            for (var j = 0; j < list[i].Count; j++)
            {
                board[i, j] = list[i][j];
            }
        }
        return board;
    }

    private List<List<EGamePiece>> ConvertToListOfLists(EGamePiece[,] board)
    {
        var list = new List<List<EGamePiece>>();
        
        for (var i = 0; i < board.GetLength(0); i++)
        {
            var innerList = new List<EGamePiece>();
            for (var j = 0; j < board.GetLength(1); j++)
            {
                innerList.Add(board[i, j]);
            }
            list.Add(innerList);
        }
        return list;
    }
}
