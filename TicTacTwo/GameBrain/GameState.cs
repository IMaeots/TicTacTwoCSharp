using Common.Entities;

namespace GameBrain;

public class GameState(EGamePiece[][] gameBoard, GameConfiguration gameConfiguration)
{
    public EGamePiece[][] GameBoard { get; set; } = gameBoard;
    public EGamePiece NextMoveBy { get; set; } = EGamePiece.X;

    public GameConfiguration GameConfiguration { get; set; } = gameConfiguration;

    public override string ToString()
    {
        return System.Text.Json.JsonSerializer.Serialize(this);
    }
}
