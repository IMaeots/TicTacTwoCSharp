namespace Common;

public static class Constants
{
    public const string GameName = "TIC-TAC-TWO";
    public const string FirstPlayerSymbol = "X";
    public const string SecondPlayerSymbol = "O";
    public const int MinimumGameDimension = 2;
    public const int MaximumGameDimension = 100;
    public const string JsonFileExtension = ".json";
    public static readonly string BasePath = Environment
                                                 .GetFolderPath(Environment.SpecialFolder.UserProfile)
                                             + Path.DirectorySeparatorChar + "icd0008-24f" + Path.DirectorySeparatorChar;
    public static readonly string GamesPath =Path.Combine(BasePath, "Games" + Path.DirectorySeparatorChar);
    public static readonly string ConfigurationsPath = Path.Combine(BasePath, "Configurations" + Path.DirectorySeparatorChar);
    
    public const string EmptyInputMessage = "It would be nice, if you actually choose something!!! Try again... Maybe...";
    public const string InvalidChoiceMessage = "Invalid choice. Available options: ";
    public const string ExitTitle = "Exit";
    public const string ExitShortcut = "E";
    public const string ConfirmExitText = "Are you sure you want to close the game? (Y/N)";
    public const string ConfirmSymbol = "Y";
    public const string LeaveGameShortcut = "L";
    public const string ConfirmLeaveGameText = "Are you sure you want to leave the game? (Y/N)";
    public const string ReturnTitle = "Return";
    public const string ReturnShortcut = "R";
    public const string ReturnToMainTitle = "Return to Main menu";
    public const string ReturnToMainShortcut = "M";
    
    public const string MenuDescriptionDivider = "---------------";
    public const string MenuDivider = "===============";
    public const string MenuInputBoxHint = ">";
    public const string MenuNewGameTitle = "New Game";
    public const string MenuNewGameShortcut = "N";
    public const string MenuSavedGamesTitle = "Saved Games";
    public const string MenuSavedGamesShortcut = "S";
    public const string MenuRulesAndInfoTitle = "Rules & Info";
    public const string MenuRulesAndInfoShortcut = "R";
    public const string MenuConfigCreationTitle = "Create New Config";
    public const string MenuConfigCreationShortcut = "C";
    public const string MenuChooseConfigHeading = GameName + " Choose Game Gonfiguration";
    public const string MenuSavedGamesHeading = GameName + " Saved Games";
    public const string MenuRulesAndInfoHeading = GameName + " Information";
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
        "Create your own configuration to enjoy a fresh & interesting game!";
}
