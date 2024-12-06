using static Common.Constants;

namespace GameLogic;

public static partial class GameConfigurationValidator
{
    public static string? ValidateName(string name) =>
        IsAlphanumericRegex().IsMatch(name)
            ? null
            : "Configuration name must contain only normal characters or numbers.";
    
    public static string? ValidateMode(string input) =>
        IsCorrectModeRegex().IsMatch(input)
            ? null
            : "Mode must be a single character representing wished mode from the options.";

    public static string? ValidateWinCondition(int winCondition, int gridHeight, int gridWidth) =>
        winCondition <= 0 ? "Win condition must be a positive integer."
        : winCondition > gridHeight && winCondition > gridWidth ? "Win condition cannot be greater than the grid's dimensions." 
        : null;
    
    public static string? ValidateBoardWidth(int width) => 
        ValidateRangeAccordingToRules(width);

    public static string? ValidateBoardHeight(int height) =>
        ValidateRangeAccordingToRules(height);

    public static string? ValidateGridWidth(int gridWidth, int boardWidth) => 
        gridWidth > boardWidth ? "Grid width cannot be greater than the board width." : ValidateRangeAccordingToRules(gridWidth);

    public static string? ValidateGridHeight(int gridHeight, int boardHeight) => 
        gridHeight > boardHeight ? "Grid height cannot be greater than the board height." : ValidateRangeAccordingToRules(gridHeight);

    public static string? ValidateMoveGridAfterNMoves(int moves) => 
        moves <= 1 ? "Moves required to move grid must be greater than 1." : null;

    public static string? ValidateMarkers(int markers, int winCondition) => 
        markers <= 0 ? "Number of markers must be a positive integer." :
            markers < winCondition ? $"Number of markers must be greater or equal to winCondition ({winCondition})." : null;

    public static string? ValidateStartingGridXPosition(int position, int boardWidth, int gridWidth) => 
        ValidateStartingPosition(position, boardWidth, gridWidth, "X");

    public static string? ValidateStartingGridYPosition(int position, int boardHeight, int gridHeight) => 
        ValidateStartingPosition(position, boardHeight, gridHeight, "Y");

    public static string? ValidateStartingPlayer(int player) => 
        player is < 1 or > 2 ? "Starting player must be either 1 or 2." : null;
    
    // Helpers
    [System.Text.RegularExpressions.GeneratedRegex(@"^[a-zA-Z0-9]+$")]
    public static partial System.Text.RegularExpressions.Regex IsAlphanumericRegex();
    
    [System.Text.RegularExpressions.GeneratedRegex(@"^[SsLlOoBb]$")]
    private static partial System.Text.RegularExpressions.Regex IsCorrectModeRegex();
    
    private static string? ValidateRangeAccordingToRules(int value)
    {
        return value is < MinimumGameDimension or > MaximumGameDimension ?
            $"Value must be between {MinimumGameDimension}-{MaximumGameDimension}." : null;
    }
    
    private static string? ValidateStartingPosition(int position, int boardDimension, int gridDimension, string axis)
    {
        var validLastPosition = boardDimension - gridDimension;
        return position < 0 || position > validLastPosition ? $"{axis} position must be between 0 and {validLastPosition}." : null;
    }
}
