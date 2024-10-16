using ConsoleApp.ConsoleUI;
using GameBrain;

namespace ConsoleApp;

public static class GameController
{
    public static void StartGame(GameConfiguration config)
    {
        do
        {   
            var isGameOver = false;
            var gameInstance = new GameBrain.GameBrain(config);

            do
            {
                Visualizer.DrawBoard(gameInstance);

                Console.Write("Give me coordinates <x,y>: ");
                var input = Console.ReadLine()!;
                var inputSplit = input.Split(",");

                if (inputSplit.Length == 2 
                    && int.TryParse(inputSplit[0], out var inputX) 
                    && int.TryParse(inputSplit[1], out var inputY))
                {
                    if (gameInstance.MakeAMove(inputX, inputY))
                    {
                        isGameOver = gameInstance.IsGameOver();
                    }
                    else
                    {
                        Console.WriteLine("Invalid move. Try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please provide coordinates in the format <x,y>.");
                }

            } while (!isGameOver);

            // End of game
            Console.Write("Game over! Press any key to restart the game.");
            Console.WriteLine();
            Console.ReadKey();

        } while (true); // Infinite game currently.
    }
}
