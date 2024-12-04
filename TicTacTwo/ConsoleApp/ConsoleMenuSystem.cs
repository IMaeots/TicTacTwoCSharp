using Common;
using Common.Entities;
using Data.Repositories;
using GameBrain;
using MenuSystem;
using static Common.InputHelper;

namespace ConsoleApp;

public class ConsoleMenuSystem(
    IConfigRepository configRepository,
    IGameRepository gameRepository
) : BaseMenuSystem<ConsoleMenu>(configRepository, gameRepository)
{
    protected override string? StartGameWithConfig(string configName)
    {
        var config = ConfigRepository.GetConfigurationByName(configName);
        if (config == null)
        {
            return "Invalid configuration selected.";
        }
        
        var gameResult = GameController.StartNewGame(config);
        return HandleGameResult(gameResult);
    }

    protected override string? StartSavedGame(string savedGameName)
    {
        var savedGameState = GameRepository.GetGameStateByName(savedGameName);
        if (savedGameState == null)
        {
            Console.WriteLine("Invalid game save selected.");
            return null;
        }
        
        var gameResult = GameController.StartSavedGame(savedGameState);
        return HandleGameResult(gameResult);
    }

    private static string? ValidateSaveGameName(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return null;
        }
        return GameConfigurationValidator.IsAlphanumericRegex().IsMatch(input) ? input : null;
    }

    protected override GameConfiguration CreateNewConfig()
    {
        var name = GetValidatedString(
            prompt: "Enter game name:",
            validationRule: GameConfigurationValidator.ValidateName
        );

        var mode = GetValidatedString(
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

        return new GameConfiguration(
            Name:name,
            Mode:mode,
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

    private string? HandleGameResult((GameState?, string?) gameResult)
    {
        if (gameResult.Item1 != null)
        {
            Console.WriteLine("Enter name for the saved game: ");
            string? saveName;
            do
            {
                var input = Console.ReadLine() ?? string.Empty;
                saveName = ValidateSaveGameName(input);
            } while (saveName == null);
            
            GameRepository.SaveGame(gameState: gameResult.Item1, savedGameName: saveName);
            return Constants.ReturnToMainShortcut;
        }

        return gameResult.Item2 ?? null;
    }
}
