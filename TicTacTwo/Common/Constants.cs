namespace Common;

public static class Constants
{
    public const string GameName = "TIC-TAC-TWO";
    public const string FirstPlayerSymbol = "X";
    public const string SecondPlayerSymbol = "O";
        
    public const string ConfigFileExtension = ".config.json";
    public const string GameDataFileExtension = ".games.json";
    public static readonly string BasePath = Environment
                                                 .GetFolderPath(Environment.SpecialFolder.UserProfile)
                                             + Path.DirectorySeparatorChar + "icd0008-24f" + Path.DirectorySeparatorChar; 
    
    public const string EmptyInputMessage = "It would be nice, if you actually choose something!!! Try again... Maybe...";
    public const string InvalidChoiceMessage = "Invalid choice. Available options: ";
    public const string MenuDescriptionDivider = "---------------";
    public const string MenuDivider = "===============";
    public const string MenuInputBoxHint = ">";
    public const string ExitTitle = "Exit";
    public const string ExitShortcut = "E";
    public const string ConfirmExitText = "Are you sure you want to exit? (Y/N)";
    public const string ReturnTitle = "Return";
    public const string ReturnShortcut = "R";
    public const string ReturnToMainTitle = "Return to Main menu";
    public const string ReturnToMainShortcut = "M";

    public const string ChooseConfigMenuTitle = GameName + " Choose Game Gonfiguration";
    public const string OptionsMenuTitle = GameName + " Options";
    public const string SavedGamesMenuTitle = GameName + " Saved Games";
    public const string InfoMenuTitle = GameName + " Information";
    public const string InfoMenuDescription = "How to Play: WIP  \nConfiguration Rules: WIP";
}
