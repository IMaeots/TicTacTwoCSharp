using Common;
using Common.Entities;

namespace ConsoleApp.ConsoleUI;

public static class Visualizer
{
    public static void DrawBoard(GameBrain.GameBrain gameInstance, int currentX, int currentY)
    {
        Console.Clear();

        var gridSize = gameInstance.GridSize;
        var gridX = gameInstance.GridX;
        var gridY = gameInstance.GridY;
        
        for (var y = 0; y < gameInstance.BoardDimY; y++)
        {
            for (var x = 0; x < gameInstance.BoardDimX; x++)
            {
                if (x >= gridX && x < gridX + gridSize && y >= gridY && y < gridY + gridSize)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                }
                
                if (x == currentX && y == currentY)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                }
                
                Console.Write(" " + DrawGamePiece(gameInstance.GameBoard[x][y]) + " ");
                Console.ResetColor();
                
                if (x < gameInstance.BoardDimX - 1)
                {
                    Console.Write("|");   
                }
            }
            
            Console.WriteLine();
            if (y < gameInstance.BoardDimY - 1)
            {
                for (var x = 0; x < gameInstance.BoardDimX; x++)
                {
                    Console.Write("---");
                    if (x != gameInstance.BoardDimX - 1)
                    {
                        Console.Write("+");
                    }

                    Console.ResetColor();
                }
            }
            Console.WriteLine();
        }
        Console.ResetColor();
    }

    private static string DrawGamePiece(EGamePiece piece) =>
        piece switch
        {
            EGamePiece.Player1 => Constants.FirstPlayerSymbol,
            EGamePiece.Player2 => Constants.SecondPlayerSymbol,
            _ => " "
        };
}
