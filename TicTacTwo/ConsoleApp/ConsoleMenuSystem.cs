using Data;
using GameBrain;
using MenuSystem;

namespace ConsoleApp;

public class ConsoleMenuSystem(IConfigRepository configRepository) : BaseMenuSystem<ConsoleMenu>(configRepository)
{
    protected override GameConfiguration? CreateNewConfig()
    {
        Console.Write("Enter the name for the new configuration: ");
        var name = Console.ReadLine()?.Trim();
        
        if (string.IsNullOrEmpty(name))
        {
            Console.WriteLine("Configuration name cannot be empty. Please try again.");
            return null;
        }
        
        var winCondition = GetValidIntInput("Enter the win condition (number of markers to win): ");
        var boardSize = GetValidIntInput("Enter the board size (e.g., 5 for a 5x5 board): ");
        var gridSize = GetValidIntInput("Enter the grid size (e.g., 3 for a 3x3 grid): ");
        var moveGridAfterNMoves = GetValidIntInput("Enter the number of moves by a player before they can move the grid: ");
        var numberOfMarkers = GetValidIntInput("Enter the number of markers available per player: ");

        // TODO: Manually putting default values should be also fixed later...
        var conf = new GameConfiguration(
            Name: name,
            WinCondition: winCondition ?? 3,
            BoardSize: boardSize ?? 5,
            GridSize: gridSize ?? 3,
            MoveGridAfterNMoves: moveGridAfterNMoves ?? 2,
            NumberOfMarkers: numberOfMarkers ?? 4
        );
        
        return conf;
    }
    
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
    
    // TODO: Should be improved validation & then moved to BaseMenuSystem. Each conf item separate logic.
    private int? GetValidIntInput(string prompt)
    {
        Console.Write(prompt);
        var input = Console.ReadLine();
        
        if (int.TryParse(input, out var value) && value > 0)
        {
            return value;
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a positive integer.");
            return null;
        }
    }
}
