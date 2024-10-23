using Data;
using GameBrain;
using MenuSystem;
using static Common.InputHelper;

namespace ConsoleApp;

public class ConsoleMenuSystem(IConfigRepository configRepository) : BaseMenuSystem<ConsoleMenu>(configRepository)
{
    protected override string? StartGameWithConfig(string configName)
    {
        var config = ConfigRepository.GetConfigurationByName(configName);
        if (config == null)
        {
            Console.WriteLine("Invalid configuration selected.");
            return null;
        }
        
        return GameController.StartGame(config);
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
}
