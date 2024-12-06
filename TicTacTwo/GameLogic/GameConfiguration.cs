using Common.Entities;

namespace GameLogic;

public record GameConfiguration(
    string Name,
    EGameMode Mode,
    EGamePiece StartingPlayer,
    int WinCondition,
    int BoardWidth,
    int BoardHeight,
    int GridWidth,
    int GridHeight,
    int UnlockSpecialMovesAfterNMoves,
    int NumberOfMarkers,
    int StartingGridXPosition,
    int StartingGridYPosition
)
{
    public override string ToString() =>
        $"Game Name: {Name}\n" +
        $"Game Mode: {Mode}\n" +
        $"Board Size: {BoardWidth}x{BoardHeight}\n" +
        $"Grid Size: {GridWidth}x{GridHeight}\n" +
        $"Win Condition: {WinCondition} in a row\n" +
        $"Markers Per Player: {NumberOfMarkers}\n" +
        $"Special moves (moving grid or re-placing a marker) are enabled after {UnlockSpecialMovesAfterNMoves} moves made by both players\n" +
        $"Starting Grid Position: ({StartingGridXPosition}, {StartingGridYPosition})\n" +
        $"Starting Player: {StartingPlayer.ToString()}";
}
