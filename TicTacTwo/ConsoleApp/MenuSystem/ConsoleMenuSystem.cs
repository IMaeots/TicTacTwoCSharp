using Common;
using Common.Entities;
using Data.Repositories;
using GameLogic;
using static Common.InputHelper;

namespace ConsoleApp.MenuSystem;

public class ConsoleMenuSystem(IConfigRepository configRepository, IGameRepository gameRepository)
{
    private const string MenuNewGameTitle = "New Game";
    private const string MenuNewGameShortcut = "N";
    private const string MenuSavedGamesTitle = "Saved Games";
    private const string MenuSavedGamesShortcut = "S";
    private const string MenuInfoTitle = "Info";
    private const string MenuInfoShortcut = "I";
    private const string MenuConfigCreationTitle = "Create New Config";
    private const string MenuConfigCreationShortcut = "C";
    private const string MenuChooseConfigHeading = Constants.GameName + " Choose Game Configuration";
    private const string MenuSavedGamesHeading = Constants.GameName + " Saved Games";
    private const string MenuRulesAndInfoHeading = Constants.GameName + " Information";
    private const string ConfirmExitText = "Are you sure you want to close the game? (Y/N)";
    private const string ConfirmLeaveGameText = "Are you sure you want to leave the game? (Y/N)";

    public void Run()
    {
        var result = CreateHomeMenu().Run();
        while (true)
        {
            switch (result)
            {
                case Constants.ReturnToMainShortcut:
                case Constants.LeaveGameShortcut:
                    result = CreateHomeMenu().Run();
                    break;
                case Constants.ExitShortcut:
                    if (ConfirmExit(ConfirmExitText))
                        return;
                    result = CreateHomeMenu().Run();
                    break;
                case Constants.ManualExitShortcut:
                case null:
                    return;
            }
        }
    }

    private ConsoleMenu CreateMenu(EMenuLevel menuLevel, string menuHeader, string? menuDescription,
        List<MenuItem> menuItems) =>
        new(menuLevel, menuHeader, menuItems, menuDescription);

    private ConsoleMenu CreateHomeMenu()
    {
        var homeMenuItems = new List<MenuItem>
        {
            new(MenuNewGameTitle, MenuNewGameShortcut, () => CreateNewGameMenu().Run()),
            new(MenuSavedGamesTitle, MenuSavedGamesShortcut, () => CreateSavedGamesMenu().Run()),
            new(MenuInfoTitle, MenuInfoShortcut, () => CreateRulesAndInfoMenu().Run())
        };

        return CreateMenu(EMenuLevel.Primary, Constants.GameName, null, homeMenuItems);
    }

    private ConsoleMenu CreateNewGameMenu()
    {
        var gameConfigurations = new List<MenuItem>
        {
            new(
                title: MenuConfigCreationTitle,
                shortcut: MenuConfigCreationShortcut,
                action: () => CreateNewConfigAndSaveIt().GetAwaiter().GetResult()
            )
        };

        var configNames = configRepository.GetConfigurationNamesAsync().Result;
        for (var i = 0; i < configNames.Count; i++)
        {
            var configIndex = i;
            gameConfigurations.Add(new MenuItem(
                title: configNames[i],
                shortcut: (i + 1).ToString(),
                action: () => StartNewGameWithConfig(configNames[configIndex])
            ));
        }

        return CreateMenu(EMenuLevel.Secondary, MenuChooseConfigHeading, null, gameConfigurations);
    }

    private async Task<string?> CreateNewConfigAndSaveIt()
    {
        var newConfig = CreateNewConfig();
        await configRepository.SaveConfigAsync(newConfig);
        return CreateNewGameMenu().Run();
    }

    private ConsoleMenu CreateSavedGamesMenu()
    {
        var savedGamesMenuItems = new List<MenuItem>();
        var savedGamesNames = gameRepository.GetSavedGamesNamesAsync().Result;
        for (var i = 0; i < savedGamesNames.Count; i++)
        {
            var saveIndex = i;
            savedGamesMenuItems.Add(new MenuItem(
                title: savedGamesNames[i],
                shortcut: (i + 1).ToString(),
                action: () => StartSavedGame(savedGamesNames[saveIndex])
            ));
        }

        return CreateMenu(EMenuLevel.Secondary, MenuSavedGamesHeading, null, savedGamesMenuItems);
    }

    private ConsoleMenu CreateRulesAndInfoMenu()
    {
        var rulesMenuItems = new List<MenuItem>();
        return CreateMenu(EMenuLevel.Secondary, MenuRulesAndInfoHeading,
            Constants.MenuRulesAndInfoDescription, rulesMenuItems);
    }

    private string StartNewGameWithConfig(string configName)
    {
        var gameConfiguration = configRepository.GetConfigurationByNameAsync(configName).Result;

        var newGame = CreateNewGame(gameConfiguration);
        gameRepository.SaveNewGameAsync(newGame);

        var playerType = GetPlayerType(newGame);
        if (playerType == null) return Constants.ReturnToMainShortcut;
        
        var gameController = new GameController(
            newGame,
            playerType.Value,
            () => gameRepository.GetSavedGameByNameAsync(newGame.Name).GetAwaiter().GetResult(),
            game => gameRepository.SaveGameStateAsync(game).Wait(),
            game => gameRepository.DeleteGameAsync(game).Wait()
        );
        
        return gameController.Play();
    }

    private string StartSavedGame(string savedGameName)
    {
        var savedGame = gameRepository.GetSavedGameByNameAsync(savedGameName).Result;

        var playerType = GetPlayerType(savedGame);
        if (playerType == null) return Constants.ReturnToMainShortcut;
        
        var gameController = new GameController(
            savedGame,
            playerType.Value,
            () => gameRepository.GetSavedGameByNameAsync(savedGame.Name).Result,
            game => gameRepository.SaveGameStateAsync(game).Wait(),
            game => gameRepository.DeleteGameAsync(game).Wait()
        );
        
        return gameController.Play();
    }

    private EGamePiece? GetPlayerType(Game game)
    {
        var isEscaping = false;
        EGamePiece? playerType = null;
        if (game.IsPasswordNeeded())
        {
            while (!isEscaping)
            {
                Console.WriteLine($"Please enter password for the game ({game.Name}): ");
                var password = Console.ReadLine();
                var type = game.GetPlayerTypeByPassword(password);

                if (type == null)
                {
                    Console.WriteLine("\nInvalid password.");
                    Console.WriteLine("Press [R] to return to the main menu. To try new password, enter any other key.");
                    if (Console.ReadKey().Key == ConsoleKey.R) { isEscaping = true; }
                }
                else
                {
                    playerType = type.Value;
                    break;
                }
            }
        }
        else
        {
            playerType = EGamePiece.Empty;
        }

        Console.Clear();
        return playerType;
    }

    private GameConfiguration CreateNewConfig()
    {
        Console.WriteLine("---------------------------");
        Console.WriteLine("Let's make a configuration!");
        Console.WriteLine("---------------------------\n");

        var name = GetValidatedName(
            prompt: "Enter a name for the configuration:",
            existingNames: configRepository.GetConfigurationNamesAsync().Result,
            validationRule: GameConfigurationValidator.ValidateInputAsAlphanumeric
        );

        var gameMode = GetValidatedString(
                prompt:
                "Enter game mode: [S] - Single Player, [L] - Local Two Player, [O] - Online Two Player, [B] - Bots",
                validationRule: GameConfigurationValidator.ValidateMode
            ).ToUpper() switch
            {
                "S" => EGameMode.SinglePlayer,
                "L" => EGameMode.LocalTwoPlayer,
                "O" => EGameMode.OnlineTwoPlayer,
                "B" => EGameMode.Bots,
                _ => throw new ArgumentOutOfRangeException()
            };

        var boardWidth = GetValidatedInt(
            prompt: "Enter board width:",
            validationRule: GameConfigurationValidator.ValidateBoardWidth
        );

        var boardHeight = GetValidatedInt(
            prompt: "Enter board height:",
            validationRule: GameConfigurationValidator.ValidateBoardHeight
        );

        var gridWidth = GetValidatedInt(
            prompt: "Enter grid width:",
            validationRule: width => GameConfigurationValidator.ValidateGridWidth(width, boardWidth)
        );

        var gridHeight = GetValidatedInt(
            prompt: "Enter grid height:",
            validationRule: height => GameConfigurationValidator.ValidateGridHeight(height, boardHeight)
        );

        var winCondition = GetValidatedInt(
            prompt: "Enter win condition (how many in a row needed to win):",
            validationRule: condition => GameConfigurationValidator.ValidateWinCondition(condition, gridWidth, gridHeight)
        );

        var numberOfMarkers = GetValidatedInt(
            prompt: "Enter number of markers each player can place:",
            validationRule: markers => GameConfigurationValidator.ValidateNumberOfMarkers(markers, winCondition)
        );

        var unlockSpecialMovesAfterNMoves = GetValidatedInt(
            prompt: "After how many moves (by both players) should special moves (moving grid, moving placed markers) be enabled:",
            validationRule: GameConfigurationValidator.ValidateUnlockSpecialMovesAfterNMoves
        );

        var startingPlayer = GetValidatedString(
                prompt: "Enter starting player: [1] - Player 1, [2] - Player 2",
                validationRule: GameConfigurationValidator.ValidateStartingPlayer
            ).Trim() == "1"
            ? EGamePiece.Player1
            : EGamePiece.Player2;

        var startingGridXPosition = GetValidatedInt(
            prompt: "Enter starting grid X position:",
            validationRule: x => GameConfigurationValidator.ValidateStartingGridXPosition(x, boardWidth, gridWidth)
        );

        var startingGridYPosition = GetValidatedInt(
            prompt: "Enter starting grid Y position:",
            validationRule: y => GameConfigurationValidator.ValidateStartingGridYPosition(y, boardHeight, gridHeight)
        );

        return new GameConfiguration(
            name,
            gameMode,
            startingPlayer,
            winCondition,
            boardWidth,
            boardHeight,
            gridWidth,
            gridHeight,
            unlockSpecialMovesAfterNMoves,
            numberOfMarkers,
            startingGridXPosition,
            startingGridYPosition
        );
    }

    private Game CreateNewGame(GameConfiguration gameConfiguration)
    {
        var gameName = GetValidatedName(
            prompt: "Enter a name for your game:",
            existingNames: gameRepository.GetSavedGamesNamesAsync().Result,
            validationRule: GameConfigurationValidator.ValidateInputAsAlphanumeric
        );

        var passwordP1 = string.Empty;
        var passwordP2 = string.Empty;
        switch (gameConfiguration.Mode)
        {
            case EGameMode.SinglePlayer:
                passwordP1 = GetValidatedString(
                    prompt: "Enter a password to access this game:",
                    validationRule: GameConfigurationValidator.ValidateInputAsAlphanumeric
                );
                break;
            case EGameMode.OnlineTwoPlayer:
                passwordP1 = GetValidatedString(
                    prompt: "Enter a password for Player 1:",
                    validationRule: GameConfigurationValidator.ValidateInputAsAlphanumeric
                );
                while (string.IsNullOrEmpty(passwordP2) || passwordP1 == passwordP2)
                {
                    passwordP2 = GetValidatedString(
                        prompt: "Enter a password for Player 2:",
                        validationRule: GameConfigurationValidator.ValidateInputAsAlphanumeric
                    );
                    if (passwordP1 == passwordP2) Console.WriteLine("Passwords can not match.");
                }
                break;
        }

        var gameState = new GameState(gameConfiguration);
        return new Game(
            Name: gameName, 
            Configuration: gameConfiguration, 
            State: gameState, 
            PasswordP1: passwordP1, 
            PasswordP2: passwordP2
        );
    }

    private bool ConfirmExit(string confirmationMessage)
    {
        Console.WriteLine(confirmationMessage);
        return Console.ReadKey().Key.ToString().ToUpper() == Constants.ConfirmSymbol;
    }
}
