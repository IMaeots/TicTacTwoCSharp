using System.Text.RegularExpressions;
using Common;
using Data;
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

    private string? ValidateSaveGameName(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return null;
        }
        return Regex.IsMatch(input, @"^[a-zA-Z0-9]+$") ? input : null;
    }

    protected override GameConfiguration CreateNewConfig()
    {
        var name = GetValidatedString(
            prompt: "Enter game name:",
            validationRule: GameConfigurationValidator.ValidateName
        );

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
            prompt: "Enter number of moves to move the grid:",
            validationRule: GameConfigurationValidator.ValidateMoveGridAfterNMoves
        );

        var numberOfMarkers = GetValidatedInt(
            prompt: "Enter number of markers per player:",
            validationRule: validatedInput => GameConfigurationValidator.ValidateMarkers(validatedInput, winCondition)
        );

        var startX = GetValidatedNullableInt(
            prompt: "Enter starting grid's top left corner's X position (optional):",
            validationRule: input => GameConfigurationValidator.ValidateStartingGridXPosition(input, boardWidth, gridWidth)
        );

        var startY = GetValidatedNullableInt(
            prompt: "Enter starting grid's top left corner's Y position (optional):",
            validationRule: input => GameConfigurationValidator.ValidateStartingGridYPosition(input, boardHeight, gridHeight)
        );

        var startingPlayer = GetValidatedNullableInt(
            prompt: "Enter starting player - [1] or [2] (optional):",
            validationRule: GameConfigurationValidator.ValidateStartingPlayer
        );

        return new GameConfiguration(
            Name:name,
            WinCondition:winCondition,
            BoardWidth:boardWidth,
            BoardHeight: boardHeight,
            GridWidth: gridWidth,
            GridHeight: gridHeight,
            MoveGridAfterNMoves:moveGridAfterNMoves,
            NumberOfMarkers: numberOfMarkers,
            UserInputStartingGridXPosition: startX,
            UserInputStartingGridYPosition: startY,
            UserInputStartingPlayer: startingPlayer
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
