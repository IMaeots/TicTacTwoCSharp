namespace Common;

public static class Constants
{
    public const int MaxPlayers = 2;
    public const char FirstPlayerSymbol = 'X';
    public const char SecondPlayerSymbol = 'O';
        
    public const string GameOverMessage = "Game Over!";
    public const string InvalidMoveMessage = "Invalid Move! Please try again.";
        
    public const string ConfigFileExtension = ".config.json";
    public const string GameDataFileExtension = ".games.json";
    // TODO: Verify BasePath.
    public static string BasePath = Environment
                                        .GetFolderPath(System.Environment.SpecialFolder.UserProfile)
                                    + Path.DirectorySeparatorChar + "icd0008-24f" + Path.DirectorySeparatorChar;   
}
