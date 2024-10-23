using System.Text.Json.Serialization;
using Common.Entities;

namespace GameBrain;

public class GameState
{
    [JsonIgnore]
    public EGamePiece[,] GameBoard { get; set; } = null!;
    public List<List<EGamePiece>> SerializedGameBoard { get; set; } = null!;
    public GameConfiguration GameConfiguration { get; }
    public EGamePiece NextMoveBy { get; set; } = EGamePiece.Player1;
    public int Player1MarkersPlaced { get; set; }
    public int Player2MarkersPlaced { get; set; }
    public int GridX { get; set; }
    public int GridY { get; set; }
    public int MoveCount { get; set; }

    public GameState(GameConfiguration gameConfiguration)
    {
        GameConfiguration = gameConfiguration;
        ResetGameBoard();
    }
    
    public bool CanMoveGrid()
    {
        return MoveCount / 2 >= GameConfiguration.MoveGridAfterNMoves;
    }
    
    public void ResetGameBoard()
    {
        var startingPlayer = GameConfiguration.FinalStartingPlayer;
        var boardWidth = GameConfiguration.BoardWidth;
        var boardHeight = GameConfiguration.BoardHeight;
        
        GameBoard = new EGamePiece[boardWidth, boardHeight];
        for (var x = 0; x < boardWidth; x++)
        {
            for (var y = 0; y < boardHeight; y++)
            {
                GameBoard[x, y] = EGamePiece.Empty;
            }
        }
        
        GridX = GameConfiguration.FinalStartingGridXPosition;
        GridY = GameConfiguration.FinalStartingGridYPosition;
        NextMoveBy = startingPlayer == EGamePiece.Player2 ? EGamePiece.Player2 : EGamePiece.Player1;
        Player1MarkersPlaced = 0;
        Player2MarkersPlaced = 0;
        MoveCount = 0;
    }
}
