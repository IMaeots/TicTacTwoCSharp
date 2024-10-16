namespace GameBrain;

public record GameConfiguration(
    string Name,
    int WinCondition = 3,
    int BoardSize = 5,
    int GridSize = 3,
    int MoveGridAfterNMoves = 2
)
{
    public override string ToString() =>
        $"Board {BoardSize}x{BoardSize}, " +
        $"Grid {GridSize}x{GridSize}, " +
        $"to win: {WinCondition}, " +
        $"can move the grid after {MoveGridAfterNMoves} moves.";
}