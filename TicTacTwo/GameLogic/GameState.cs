using System.Text.Json.Serialization;
using Common.Entities;

namespace GameLogic;

public class GameState
{
    public EGamePiece[][] GameBoard { get; set; }
    public EGamePiece NextMoveBy { get; set; }
    public EGameOutcome GameOutcome { get; set; }
    public int Player1MarkersPlaced { get; set; }
    public int Player2MarkersPlaced { get; set; }
    public int GridX { get; set; }
    public int GridY { get; set; }
    public int MoveCount { get; set; }
    
    [JsonConstructor]
    public GameState(EGamePiece[][] gameBoard, EGamePiece nextMoveBy, EGameOutcome gameOutcome,
        int player1MarkersPlaced, int player2MarkersPlaced, int gridX, int gridY, int moveCount)
    {
        GameBoard = gameBoard;
        NextMoveBy = nextMoveBy;
        GameOutcome = gameOutcome;
        Player1MarkersPlaced = player1MarkersPlaced;
        Player2MarkersPlaced = player2MarkersPlaced;
        GridX = gridX;
        GridY = gridY;
        MoveCount = moveCount;
    }

    public GameState(GameConfiguration gameConfiguration)
    {
        var boardWidth = gameConfiguration.BoardWidth;
        var boardHeight = gameConfiguration.BoardHeight;
        GameBoard = new EGamePiece[boardWidth][];
        for (var x = 0; x < GameBoard.Length; x++)
        {
            GameBoard[x] = new EGamePiece[boardHeight];
        }
        GameBoard = Enumerable
            .Range(0, boardWidth)
            .Select(_ => new EGamePiece[boardHeight])
            .ToArray();
        
        GridX = gameConfiguration.StartingGridXPosition;
        GridY = gameConfiguration.StartingGridYPosition;
        NextMoveBy = gameConfiguration.StartingPlayer == EGamePiece.Player2 ? EGamePiece.Player2 : EGamePiece.Player1;
        Player1MarkersPlaced = 0;
        Player2MarkersPlaced = 0;
        MoveCount = 0;
    }

    public override string ToString()
    {
        return System.Text.Json.JsonSerializer.Serialize(this);
    }
}
