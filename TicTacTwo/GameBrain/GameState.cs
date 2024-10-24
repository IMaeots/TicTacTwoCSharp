using Common.Entities;

namespace GameBrain;

public class GameState
{
    public EGamePiece[][] GameBoard { get; set; }
    public GameConfiguration GameConfiguration { get; }
    public EGamePiece NextMoveBy { get; set; }
    public int Player1MarkersPlaced { get; set; }
    public int Player2MarkersPlaced { get; set; }
    public int GridX { get; set; }
    public int GridY { get; set; }
    public int MoveCount { get; set; }

    public GameState(GameConfiguration gameConfiguration)
    {
        GameConfiguration = gameConfiguration;
        
        var startingPlayer = GameConfiguration.FinalStartingPlayer;
        var boardWidth = GameConfiguration.BoardWidth;
        var boardHeight = GameConfiguration.BoardHeight;
        GameBoard = new EGamePiece[boardWidth][];
        for (var x = 0; x < GameBoard.Length; x++)
        {
            GameBoard[x] = new EGamePiece[boardHeight];
        }
        GameBoard = Enumerable
            .Range(0, boardWidth)
            .Select(_ => new EGamePiece[boardHeight])
            .ToArray();
        
        GridX = GameConfiguration.FinalStartingGridXPosition;
        GridY = GameConfiguration.FinalStartingGridYPosition;
        NextMoveBy = startingPlayer == EGamePiece.Player2 ? EGamePiece.Player2 : EGamePiece.Player1;
        Player1MarkersPlaced = 0;
        Player2MarkersPlaced = 0;
        MoveCount = 0;
    }
    
    public bool CanMoveGrid()
    {
        return MoveCount / 2 >= GameConfiguration.MoveGridAfterNMoves;
    }

    public override string ToString()
    {
        return System.Text.Json.JsonSerializer.Serialize(this);
    }
}
