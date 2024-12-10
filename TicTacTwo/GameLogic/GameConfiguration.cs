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

    public static List<GameConfiguration> GetDefaultGameConfigurations() =>
    [
        new (Name: "Classical", Mode: EGameMode.LocalTwoPlayer, StartingPlayer: EGamePiece.Player1,
            WinCondition: 3,
            BoardWidth: 5, BoardHeight: 5, GridWidth: 3, GridHeight: 3, UnlockSpecialMovesAfterNMoves: 2,
            NumberOfMarkers: 4, StartingGridXPosition: 1, StartingGridYPosition: 1),

        new (Name: "BigBoard", Mode: EGameMode.LocalTwoPlayer, StartingPlayer: EGamePiece.Player2,
            WinCondition: 4,
            BoardWidth: 10, BoardHeight: 10, GridWidth: 4, GridHeight: 4, UnlockSpecialMovesAfterNMoves: 4,
            NumberOfMarkers: 6, StartingGridXPosition: 3, StartingGridYPosition: 3)
    ];
}
