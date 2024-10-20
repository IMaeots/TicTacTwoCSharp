using Common.Entities;

namespace GameBrain;

public record GameConfiguration(
    string Name,
    int WinCondition = 3,
    int BoardWidth = 5,
    int BoardHeight = 5,
    int GridWidth = 3,
    int GridHeight = 3,
    int? StartingGridXPosition = null,
    int? StartingGridYPosition = null,
    int MoveGridAfterNMoves = 2,
    int NumberOfMarkers = 4,
    EGamePiece? StartingPlayer = EGamePiece.Player1
)
{
    public override string ToString() =>
        $"Board {BoardWidth}x{BoardHeight}, " +
        $"Grid {GridWidth}x{GridHeight}, " +
        $"to win: {WinCondition}, " +
        $"number of Markers available per player: {NumberOfMarkers}, " +
        $"can move the grid after {MoveGridAfterNMoves} moves.";
}