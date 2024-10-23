using Common;
using Common.Entities;

namespace ConsoleApp;

public static class Visualizer
{
    public static void DrawBoard(GameBrain.GameBrain gameInstance, int currentX, int currentY)
    {
        Console.Clear();
        
        var boardWidth = gameInstance.BoardWidth;
        var boardHeight = gameInstance.BoardHeight;
        var gridWidth = gameInstance.GridWidth;
        var gridHeight = gameInstance.GridHeight;
        var gridX = gameInstance.GridX;
        var gridY = gameInstance.GridY;
        
        for (var y = 0; y < boardHeight; y++)
        {
            for (var x = 0; x < boardWidth; x++)
            {
                if (x >= gridX && x < gridX + gridWidth && y >= gridY && y < gridY + gridHeight)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                }
                
                if (x == currentX && y == currentY)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                }
                
                Console.Write(" " + DrawGamePiece(gameInstance.GameBoard[x, y]) + " ");
                Console.ResetColor();
                
                if (x < boardWidth - 1)
                {
                    Console.Write("|");   
                }
            }
            
            Console.WriteLine();
            if (y < boardHeight - 1)
            {
                for (var x = 0; x < boardWidth; x++)
                {
                    Console.Write("---");
                    if (x != boardWidth - 1)
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
