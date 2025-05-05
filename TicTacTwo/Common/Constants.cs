namespace Common;

public static class Constants
{
    public const string GameName = "TIC-TAC-TWO";
    public const string FirstPlayerSymbol = "X";
    public const string SecondPlayerSymbol = "O";
    public const int MinimumGameDimension = 2;
    public const int MaximumGameDimension = 100;
    public const string JsonFileExtension = ".json";

    private static readonly string BasePath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "TicTacTwo");
    private static readonly string JsonDirectory = Path.Combine(BasePath, "JsonData");
    public static readonly string DatabaseDirectory = Path.Combine(BasePath, "Database");
    public static readonly string GamesPath = Path.Combine(JsonDirectory, "Games");
    public static readonly string ConfigurationsPath = Path.Combine(JsonDirectory, "Configurations");
    
    public const string ExitShortcut = "E";
    public const string ReturnShortcut = "R";
    public const string ReturnToMainShortcut = "M";
    public const string LeaveGameShortcut = "L";
    public const string ManualExitShortcut = "EXIT";
    public const string ConfirmSymbol = "Y";
    
    public const string MenuRulesAndInfoDescription =
        "How to Play: \n" +
        "TicTacTwo is an enhanced version of Tic-Tac-Toe.\n" +
        "Players take turns placing their markers on the game board that also includes the winning grid.\n" +
        "The goal is to align a specified number of your markers (the 'win condition') either horizontally, vertically, or diagonally inside the grid to win.\n" +
        "After completing some number of moves ('UnlockSpecialMovesAfterNMoves') you can also move the grid around or move your already placed marker.\n" +
        "'NumberOfMarkers' illustrates how many markers you can place down on the board.\n" +
        "\n" +
        "To be Noted: \n" +
        "Game is automatically saved after every move. Under Saved Games you can rejoin.\n" +
        "There are 4 Game Modes: Single Player (play against AI), Local Two Player (play with a friend on one screen), Online Two Player (play with a friend on two screens), Bots (AI vs AI)\n" +
        "Depending on game mode there may be passwords for games to distinguish players!\n" +
        "You can also make your own configurations for the game to suit your preference!\n" +
        "\n" +
        "Configuration Rules: \n" +
        "- Game Mode: Single Player (play against AI), Local Two Player (play with a friend on one screen), Online Two Player (play with a friend on two screens), Bots (AI vs AI). \n" +
        "- Board Width and Height: Must be between 2 and 100. \n" +
        "- Grid Width and Height: Cannot exceed board dimensions and must be between 2 and 100. \n" +
        "- Win Condition: Must be a positive integer and cannot exceed the grid's dimensions. \n" +
        "- Markers: Number of markers must be a positive integer that is greater or equal to win condition. \n" +
        "- Moves to Enable Special Moves (move grid or already placed marker): Must be greater than 1. \n" +
        "- Starting Player: Must be either player 1 [1] or player 2 [2]. \n" +
        "- Starting Grid Position (X and Y): Is the grid's top left corner location. Must account for the grid's size to fit inside the board!\n" +
        "\n" +
        "Create your own configuration to enjoy a fresh & interesting game!";
}
