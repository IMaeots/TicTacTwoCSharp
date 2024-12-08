using Common;
using Common.Entities;
using GameLogic;

namespace ConsoleApp;

public static class Visualizer
{
    public static void DrawBoard(Game game, int currentX, int currentY)
    {
        Console.Clear();

        var boardWidth = game.State.GameBoard.Length;
        var boardHeight = game.State.GameBoard[0].Length;
        var gridWidth = game.Configuration.GridWidth;
        var gridHeight = game.Configuration.GridHeight;
        var gridX = game.State.GridX;
        var gridY = game.State.GridY;

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

                Console.Write(" " + DrawGamePiece(game.State.GameBoard[x][y]) + " ");
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
