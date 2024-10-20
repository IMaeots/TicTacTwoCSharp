using System.Text.Json;
using Common.Entities;

namespace GameBrain;

public class GameState
{
    public EGamePiece[][] GameBoard { get; private set; } = null!;
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


    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
    
    public bool CanMoveGrid()
    {
        return MoveCount / 2 >= GameConfiguration.MoveGridAfterNMoves;
    }
    
    public void ResetGameBoard()
    {
        var startingPlayer = GameConfiguration.StartingPlayer ?? EGamePiece.Player1;
        var boardWidth = GameConfiguration.BoardWidth;
        var boardHeight = GameConfiguration.BoardHeight;
        var gridWidth = GameConfiguration.GridWidth;
        var gridHeight = GameConfiguration.GridHeight;
        
        GameBoard = new EGamePiece[boardWidth][];
        for (var x = 0; x < GameBoard.Length; x++)
        {
            GameBoard[x] = new EGamePiece[boardHeight];
        }
        GameBoard = Enumerable
            .Range(0, boardWidth)
            .Select(_ => new EGamePiece[boardHeight])
            .ToArray();
        GridX = GameConfiguration.StartingGridXPosition ?? (boardWidth - gridWidth) / 2;
        GridY = GameConfiguration.StartingGridYPosition ?? (boardHeight - gridHeight) / 2;
        NextMoveBy = startingPlayer == EGamePiece.Player2 ? EGamePiece.Player2 : EGamePiece.Player1;
        Player1MarkersPlaced = 0;
        Player2MarkersPlaced = 0;
        MoveCount = 0;
    }
}
