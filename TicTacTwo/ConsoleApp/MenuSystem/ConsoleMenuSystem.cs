using Common;
using Common.Entities;
using Data.Repositories;
using GameLogic;
using static Common.InputHelper;

namespace ConsoleApp.MenuSystem;

public class ConsoleMenuSystem(IConfigRepository configRepository, IGameRepository gameRepository)
{
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
                case Constants.ManualExitShortcut:
                case Constants.ExitShortcut:
                case null:
                    return;
            }
        }
    }

    private static ConsoleMenu CreateMenu(EMenuLevel menuLevel, string menuHeader, string? menuDescription,
        List<MenuItem> menuItems) =>
        new(menuLevel, menuHeader, menuItems, menuDescription);

    private ConsoleMenu CreateHomeMenu()
    {
        var homeMenuItems = new List<MenuItem>
        {
            new(Constants.MenuNewGameTitle, Constants.MenuNewGameShortcut, () => CreateNewGameMenu().Run()),
            new(Constants.MenuSavedGamesTitle, Constants.MenuSavedGamesShortcut, () => CreateSavedGamesMenu().Run()),
            new(Constants.MenuInfoTitle, Constants.MenuInfoShortcut, () => CreateRulesAndInfoMenu().Run())
        };

        return CreateMenu(EMenuLevel.Primary, Constants.GameName, null, homeMenuItems);
    }

    private ConsoleMenu CreateNewGameMenu()
    {
        var gameConfigurations = new List<MenuItem>
        {
            new(
                title: Constants.MenuConfigCreationTitle,
                shortcut: Constants.MenuConfigCreationShortcut,
                action: CreateNewConfigAndSaveIt
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

        return CreateMenu(EMenuLevel.Secondary, Constants.MenuChooseConfigHeading, null, gameConfigurations);
    }

    private string? CreateNewConfigAndSaveIt()
    {
        var newConfig = CreateNewConfig();
        configRepository.SaveConfigAsync(newConfig);
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

        return CreateMenu(EMenuLevel.Secondary, Constants.MenuSavedGamesHeading, null, savedGamesMenuItems);
    }

    private ConsoleMenu CreateRulesAndInfoMenu()
    {
        var rulesMenuItems = new List<MenuItem>();
        return CreateMenu(EMenuLevel.Secondary, Constants.MenuRulesAndInfoHeading,
            Constants.MenuRulesAndInfoDescription, rulesMenuItems);
    }

    private string StartNewGameWithConfig(string configName)
    {
        var gameConfiguration = configRepository.GetConfigurationByNameAsync(configName).Result;

        var newGame = CreateNewGame(gameConfiguration);
        gameRepository.SaveNewGameAsync(newGame);

        var playerType = GetPlayerType(newGame);
        if (playerType == null) return Constants.ReturnToMainShortcut;
        
        return GameController.PlayGame(
            newGame,
            playerType.Value,
            () => gameRepository.GetSavedGameByNameAsync(newGame.Name).Result,
            game => gameRepository.SaveGameStateAsync(game).Wait(),
            game => gameRepository.DeleteGameAsync(game).Wait()
        );
    }

    private string StartSavedGame(string savedGameName)
    {
        var savedGame = gameRepository.GetSavedGameByNameAsync(savedGameName).Result;

        var playerType = GetPlayerType(savedGame);
        if (playerType == null) return Constants.ReturnToMainShortcut;
        
        return GameController.PlayGame(
            savedGame,
            playerType.Value,
            () => gameRepository.GetSavedGameByNameAsync(savedGame.Name).Result,
            game => gameRepository.SaveGameStateAsync(game).Wait(),
            game => gameRepository.DeleteGameAsync(game).Wait()
        );
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
            validationRule: validatedInput => GameConfigurationValidator.ValidateGridWidth(validatedInput, boardWidth)
        );

        var gridHeight = GetValidatedInt(
            prompt: "Enter grid height:",
            validationRule: validatedInput => GameConfigurationValidator.ValidateGridHeight(validatedInput, boardHeight)
        );

        var winCondition = GetValidatedInt(
            prompt: "Enter win condition:",
            validationRule: validatedInput =>
                GameConfigurationValidator.ValidateWinCondition(validatedInput, gridHeight, gridWidth)
        );

        var moveGridAfterNMoves = GetValidatedInt(
            prompt:
            "Enter number of moves to gain the ability to move the grid or already placed down marker (must be > 1):",
            validationRule: GameConfigurationValidator.ValidateMoveGridAfterNMoves
        );

        var numberOfMarkers = GetValidatedInt(
            prompt: "Enter number of markers per player:",
            validationRule: validatedInput => GameConfigurationValidator.ValidateMarkers(validatedInput, winCondition)
        );

        var startX = GetValidatedNullableInt(
            prompt: "Enter starting grid's top left corner's X position (optional):",
            validationRule: input =>
                GameConfigurationValidator.ValidateStartingGridXPosition(input, boardWidth, gridWidth)
        ) ?? (boardWidth - gridWidth) / 2;

        var startY = GetValidatedNullableInt(
            prompt: "Enter starting grid's top left corner's Y position (optional):",
            validationRule: input =>
                GameConfigurationValidator.ValidateStartingGridYPosition(input, boardHeight, gridHeight)
        ) ?? (boardHeight - gridHeight) / 2;

        var startingPlayer = GetValidatedNullableInt(
            prompt: "Enter starting player - [1] or [2] (optional):",
            validationRule: GameConfigurationValidator.ValidateStartingPlayer
        ) == 2
            ? EGamePiece.Player2
            : EGamePiece.Player1;

        Console.WriteLine("---------------------------------------");
        Console.WriteLine("Great! Configuration successfully made.");
        Console.WriteLine("---------------------------------------\n");
        Console.WriteLine("Press any key to return to the list of configurations!");
        Console.ReadKey();

        return new GameConfiguration(
            Name: name,
            Mode: gameMode,
            WinCondition: winCondition,
            BoardWidth: boardWidth,
            BoardHeight: boardHeight,
            GridWidth: gridWidth,
            GridHeight: gridHeight,
            UnlockSpecialMovesAfterNMoves: moveGridAfterNMoves,
            NumberOfMarkers: numberOfMarkers,
            StartingGridXPosition: startX,
            StartingGridYPosition: startY,
            StartingPlayer: startingPlayer
        );
    }

    private Game CreateNewGame(GameConfiguration gameConfiguration)
    {
        Console.WriteLine("----------------------");
        Console.WriteLine("Let's make a new game!");
        Console.WriteLine("----------------------\n");

        var name = GetValidatedName(
            prompt: "Enter a name for the game:",
            existingNames: gameRepository.GetSavedGamesNamesAsync().Result.ToList(),
            validationRule: GameConfigurationValidator.ValidateInputAsAlphanumeric
        );

        string? passwordP1;
        string? passwordP2;
        switch (gameConfiguration.Mode)
        {
            case EGameMode.OnlineTwoPlayer:
                passwordP1 = GetValidatedString(
                    prompt: "Enter a password for player 1:",
                    validationRule: GameConfigurationValidator.ValidateInputAsAlphanumeric
                );

                passwordP2 = GetValidatedString(
                    prompt: "Enter a password for player 2:",
                    validationRule: GameConfigurationValidator.ValidateInputAsAlphanumeric
                );
                break;
            case EGameMode.SinglePlayer:
                passwordP1 = GetValidatedString(
                    prompt: "Enter a password for player 1:",
                    validationRule: GameConfigurationValidator.ValidateInputAsAlphanumeric
                );
                passwordP2 = null;
                break;
            default:
                passwordP1 = null;
                passwordP2 = null;
                break;
        }

        Console.WriteLine("-----------------------");
        Console.WriteLine("Game successfully setup!");
        Console.WriteLine("-----------------------\n");
        Console.WriteLine("Press any key to save it and start playing!");
        Console.ReadKey();

        var gameState = new GameState(gameConfiguration);

        return new Game(name, gameConfiguration, gameState, passwordP1, passwordP2);
    }
}
