using System.Text.Json;
using Common.Entities;

namespace GameBrain;

public class GameState(
    EGamePiece[][] gameBoard,
    GameConfiguration gameConfiguration
)
{
    public GameConfiguration GameConfiguration { get; } = gameConfiguration;
    public EGamePiece NextMoveBy { get; set; } = EGamePiece.Player1;
    public EGamePiece[][] GameBoard { get; set; } = gameBoard;
    public int GridX { get; set; } = (gameConfiguration.BoardSize - gameConfiguration.GridSize) / 2;
    public int GridY { get; set; } = (gameConfiguration.BoardSize - gameConfiguration.GridSize) / 2;
    public int Player1MarkersPlaced { get; set; } = 0;
    public int Player2MarkersPlaced { get; set; } = 0;
    public int MoveCount { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
    
    public bool CanMoveGrid()
    {
        return MoveCount / 2 >= GameConfiguration.MoveGridAfterNMoves;
    }
}
