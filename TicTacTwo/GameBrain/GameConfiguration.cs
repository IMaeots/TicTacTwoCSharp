using Common.Entities;

namespace GameBrain;

public record GameConfiguration(
    string Name,
    int WinCondition,
    int BoardWidth,
    int BoardHeight,
    int GridWidth,
    int GridHeight,
    int MoveGridAfterNMoves,
    int NumberOfMarkers,
    int? UserInputStartingGridXPosition = null,
    int? UserInputStartingGridYPosition = null,
    int? UserInputStartingPlayer = null
)
{
    public int FinalStartingGridXPosition { get; init; } = 
        UserInputStartingGridXPosition ?? (BoardWidth - GridWidth) / 2;
    public int FinalStartingGridYPosition { get; init; } = 
        UserInputStartingGridYPosition ?? (BoardHeight - GridHeight) / 2;
    public EGamePiece FinalStartingPlayer { get; init; } =
        UserInputStartingPlayer == 2 ? EGamePiece.Player2 : EGamePiece.Player1;

    public override string ToString() =>
        $"Game Name: {Name}\n" +
        $"Board Size: {BoardWidth}x{BoardHeight}\n" +
        $"Grid Size: {GridWidth}x{GridHeight}\n" +
        $"Win Condition: {WinCondition} in a row\n" +
        $"Markers Per Player: {NumberOfMarkers}\n" +
        $"Grid Moves After: {MoveGridAfterNMoves} moves made by both players\n" +
        $"Starting Grid Position: ({FinalStartingGridXPosition}, {FinalStartingGridYPosition})\n" +
        $"Starting Player: {FinalStartingPlayer.ToString()}";
}
