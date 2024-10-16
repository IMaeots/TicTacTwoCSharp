using System.Text.Json;
using Common.Entities;

namespace GameBrain;

public class GameState(
    EGamePiece[][] gameBoard,
    GameConfiguration gameConfiguration
)
{
    public GameConfiguration GameConfiguration { get; } = gameConfiguration;
    public EGamePiece[][] GameBoard { get; set; } = gameBoard;
    public EGamePiece NextMoveBy { get; set; } = EGamePiece.Player1;
    public int Player1MarkersPlaced { get; set; } = 0;
    public int Player2MarkersPlaced { get; set; } = 0;

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
