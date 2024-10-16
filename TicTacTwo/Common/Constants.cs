namespace Common;

public static class Constants
{
    public const int MaxPlayers = 2;
    public const char FirstPlayerSymbol = 'X';
    public const char SecondPlayerSymbol = 'O';
        
    public const string ConfigFileExtension = ".config.json";
    public const string GameDataFileExtension = ".games.json";
    // TODO: Verify BasePath.
    public static string BasePath = Environment
                                        .GetFolderPath(Environment.SpecialFolder.UserProfile)
                                    + Path.DirectorySeparatorChar + "icd0008-24f" + Path.DirectorySeparatorChar; 
    
    public const string GameOverMessage = "Game Over!";
    public const string InvalidMoveMessage = "Invalid Move! Please try again.";

    public const string EmptyInputMessage =
        "It would be nice, if you actually choose something!!! Try again... Maybe...";
    public const string MenuDescriptionDivider = "---------------";
    public const string MenuDivider = "===============";
    public const string MenuInputBoxHint = ">";
    
    public const string ExitTitle = "Exit";
    public const string ExitShortcut = "E";
    public const string ReturnTitle = "Return";
    public const string ReturnShortcut = "R";
    public const string ReturnToMainTitle = "Return to Main menu";
    public const string ReturnToMainShortcut = "M";
}
