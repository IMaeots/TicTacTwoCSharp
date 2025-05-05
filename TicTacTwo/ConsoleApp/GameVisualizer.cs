using Common.Entities;
using GameLogic;

namespace ConsoleApp;

public static class GameVisualizer
{
    public static void DrawBoard(Game game, int highlightX, int highlightY)
    {
        Console.Clear();
        var boardWidth = game.Configuration.BoardWidth;
        var boardHeight = game.Configuration.BoardHeight;
        var gridWidth = game.Configuration.GridWidth;
        var gridHeight = game.Configuration.GridHeight;
        var gridX = game.State.GridX;
        var gridY = game.State.GridY;
        
        for (var y = 0; y < boardHeight; y++)
        {
            for (var x = 0; x < boardWidth; x++)
            {
                SetCellBackgroundColor(x, y, gridX, gridY, gridWidth, gridHeight, highlightX, highlightY);
                DrawCell(game.State.GameBoard[x][y]);
                Console.ResetColor();
                
                if (x < boardWidth - 1)
                {
                    Console.Write("|");
                }
            }

            Console.WriteLine();
            
            if (y < boardHeight - 1)
            {
                DrawRowSeparator(boardWidth);
            }
        }
        
        Console.ResetColor();
        Console.WriteLine();
    }

    private static void DrawRowSeparator(int boardWidth)
    {
        for (var x = 0; x < boardWidth; x++)
        {
            Console.Write("---");
            if (x < boardWidth - 1)
            {
                Console.Write("+");
            }
        }
        Console.WriteLine();
    }

    private static void SetCellBackgroundColor(int x, int y, int gridX, int gridY, int gridWidth, int gridHeight, int highlightX, int highlightY)
    {
        bool isInGrid = x >= gridX && x < gridX + gridWidth && 
                       y >= gridY && y < gridY + gridHeight;
                       
        bool isHighlighted = x == highlightX && y == highlightY;
        
        if (isHighlighted)
        {
            Console.BackgroundColor = isInGrid ? ConsoleColor.DarkCyan : ConsoleColor.DarkGray;
        }
        else if (isInGrid)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
        }
    }

    private static void DrawCell(EGamePiece piece)
    {
        var symbol = piece.ToSymbol();
        
        switch (piece)
        {
            case EGamePiece.Player1:
                Console.ForegroundColor = ConsoleColor.Red;
                break;
            case EGamePiece.Player2:
                Console.ForegroundColor = ConsoleColor.Green;
                break;
        }
        
        Console.Write($" {symbol} ");
        Console.ResetColor();
    }
}
