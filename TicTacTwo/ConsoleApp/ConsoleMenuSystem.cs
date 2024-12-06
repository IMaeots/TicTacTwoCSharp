using Common.Entities;
using Data.Repositories;
using GameLogic;
using MenuSystem;
using static Common.InputHelper;

namespace ConsoleApp;

public class ConsoleMenuSystem(
    IConfigRepository configRepository,
    IGameRepository gameRepository
) : BaseMenuSystem<ConsoleMenu>(configRepository, gameRepository)
{
    protected override string? StartNewGameWithConfig(string configName)
    {
        var gameConfiguration = ConfigRepository.GetConfigurationByName(configName);
        if (gameConfiguration == null)
        {
            Console.WriteLine("Invalid configuration selected.");
            return null;
        }

        var newGame = CreateNewGame(gameConfiguration);
        GameRepository.SaveNewGame(newGame);
        return GameController.PlayGame(newGame, GameRepository.SaveGameState, GameRepository.DeleteGame);
    }

    protected override string StartSavedGame(string savedGameName)
    {
        var savedGame = GameRepository.GetSavedGameByName(savedGameName);
        return GameController.PlayGame(savedGame, GameRepository.SaveGameState, GameRepository.DeleteGame);
    }

    protected override GameConfiguration CreateNewConfig()
    {
        Console.WriteLine("---------------------------");
        Console.WriteLine("Let's make a configuration!");
        Console.WriteLine("---------------------------\n");
        
        var name = GetValidatedString(
            prompt: "Enter a name for the configuration:",
            validationRule: GameConfigurationValidator.ValidateName
        );

        var gameMode = GetValidatedString(
                prompt: "Enter game mode: [S] - Single Player, [L] - Local Two Player, [O] - Online Two Player, [B] - Bots",
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
            validationRule: validatedInput => GameConfigurationValidator.ValidateWinCondition(validatedInput, gridHeight, gridWidth)
        );

        var moveGridAfterNMoves = GetValidatedInt(
            prompt: "Enter number of moves to gain the ability to move the grid or already placed down marker (must be > 1):",
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
            validationRule: input => GameConfigurationValidator.ValidateStartingGridYPosition(input, boardHeight, gridHeight)
        ) ?? (boardHeight - gridHeight) / 2;

        var startingPlayer = GetValidatedNullableInt(
            prompt: "Enter starting player - [1] or [2] (optional):",
            validationRule: GameConfigurationValidator.ValidateStartingPlayer
        ) == 2 ? EGamePiece.Player2 : EGamePiece.Player1;

        Console.WriteLine("---------------------------------------");
        Console.WriteLine("Great! Configuration successfully made.");
        Console.WriteLine("---------------------------------------\n");
        Console.WriteLine("Press any key to return to the list of configurations!");
        Console.ReadKey();

        return new GameConfiguration(
            Name:name,
            Mode:gameMode,
            WinCondition:winCondition,
            BoardWidth:boardWidth,
            BoardHeight: boardHeight,
            GridWidth: gridWidth,
            GridHeight: gridHeight,
            UnlockSpecialMovesAfterNMoves:moveGridAfterNMoves,
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
        
        string? gameName;
        do
        {
            Console.WriteLine("Enter a name for the game: ");
            var input = Console.ReadLine() ?? string.Empty;
            gameName = ValidateNormalInput(input);
        } while (gameName == null);

        string? passwordP1 = null;
        string? passwordP2 = null;
        if (gameConfiguration.Mode == EGameMode.OnlineTwoPlayer)
        {
            do
            {
                Console.WriteLine("Enter a password for Player 1: ");
                var inputPassword = Console.ReadLine() ?? String.Empty;
                passwordP1 = ValidateNormalInput(inputPassword);
            } while(passwordP1 == null);
            
            do
            {
                Console.WriteLine("Enter a password for Player 2: ");
                var inputPassword = Console.ReadLine() ?? String.Empty;
                passwordP2 = ValidateNormalInput(inputPassword);
            } while(passwordP2 == null);
        } else if (gameConfiguration.Mode == EGameMode.SinglePlayer)
        {
            do
            {
                Console.WriteLine("Enter a password for Player 1: ");
                var inputPassword = Console.ReadLine() ?? String.Empty;
                passwordP1 = ValidateNormalInput(inputPassword);
            } while (passwordP1 == null);
        }

        Console.WriteLine("-----------------------");
        Console.WriteLine("Game successfully made!");
        Console.WriteLine("-----------------------\n");
        Console.WriteLine("Press any key to start playing!");
        Console.ReadKey();

        var gameState = new GameState(gameConfiguration);

        return new Game(gameName, gameConfiguration, gameState, passwordP1, passwordP2);
    }
    
    private static string? ValidateNormalInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;
        
        return GameConfigurationValidator.IsAlphanumericRegex().IsMatch(input) ? input : null;
    }
}
