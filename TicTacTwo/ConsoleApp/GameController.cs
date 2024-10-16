using Common.Entities;
using ConsoleApp.ConsoleUI;
using GameBrain;

namespace ConsoleApp;

public static class GameController
{
    private static int _currentX = 0;
    private static int _currentY = 0;
    private static EGameAction _action;

    public static void StartGame(GameConfiguration config)
    {
        var gameOutcome = EGameOutcome.None;
        var gameInstance = new GameBrain.GameBrain(config);
        Console.CursorVisible = false;
        
        do
        {
            Visualizer.DrawBoard(gameInstance, _currentX, _currentY);
            
            _action = EGameAction.PlaceMarker;
            var nextMoveBy = gameInstance.NextMoveBy;
            var canPlaceMarker = gameInstance.CanPlaceMarker();

            if (gameInstance.CanMoveGrid() || !canPlaceMarker)
            {
                Console.WriteLine($"{nextMoveBy}'s turn! Choose an option: [P] Place marker, [G] Move grid, [M] Move marker:");

                var isSelected = false;
                while (!isSelected)
                {
                    var key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.P:
                            if (canPlaceMarker)
                            {
                                _action = EGameAction.PlaceMarker;
                                Console.WriteLine($"You chose to place a marker. Go ahead {nextMoveBy}!");
                                isSelected = true;
                                break;
                            }
                            
                            Console.WriteLine($"Choose another option as you have already placed all your markers! ");
                            break;   
                        case ConsoleKey.G:
                            _action = EGameAction.MoveGrid;
                            Console.WriteLine($"You chose to move the grid. Go ahead {nextMoveBy}!");
                            isSelected = true;
                            break;
                        case ConsoleKey.M:
                            _action = EGameAction.MoveMarker;
                            Console.WriteLine($"You chose to move a marker. Go ahead {nextMoveBy}!");
                            isSelected = true;
                            break;
                        case ConsoleKey.E:
                            Console.WriteLine("Goodbye!");
                            return;
                    }
                }
            }
            else
            {
                Console.WriteLine($"Place a marker: {nextMoveBy}!");
            }

            bool isActionPerformed = false;
            while (!isActionPerformed)
            {
                var movementKey = Console.ReadKey(true).Key;
                switch (movementKey)
                {
                    case ConsoleKey.UpArrow:
                        _currentY = (_currentY > 0) ? _currentY - 1 : 0;
                        break;
                    case ConsoleKey.DownArrow:
                        _currentY = (_currentY < gameInstance.BoardDimY - 1) ? _currentY + 1 : gameInstance.BoardDimY - 1;
                        break;
                    case ConsoleKey.LeftArrow:
                        _currentX = (_currentX > 0) ? _currentX - 1 : 0;
                        break;
                    case ConsoleKey.RightArrow:
                        _currentX = (_currentX < gameInstance.BoardDimX - 1) ? _currentX + 1 : gameInstance.BoardDimX - 1;
                        break;
                    case ConsoleKey.Enter:
                        if (_action == EGameAction.MoveMarker && !gameInstance.CanMoveThatMarker(_currentX, _currentY))
                        {
                            Console.WriteLine("This seems to not be your marker! Select another marker.");
                            Console.ReadKey();
                            break;
                        }
                        ExecuteGameAction(gameInstance);
                        isActionPerformed = true;
                        break;
                    case ConsoleKey.E:
                        Console.WriteLine("Goodbye!");
                        return;
                }
                
                Visualizer.DrawBoard(gameInstance, _currentX, _currentY);
                gameOutcome = gameInstance.CheckForGameEnd();
            }
        } while (gameOutcome == EGameOutcome.None);
        
        switch (gameOutcome)
        {
            case EGameOutcome.Draw:
                Console.WriteLine("It's a draw! Game ended in a draw because you both won.");
                break;
            case EGameOutcome.Player1Won:
                Console.WriteLine("Player 1 wins!");
                break;
            case EGameOutcome.Player2Won:
                Console.WriteLine("Player 2 wins!");
                break;
            case EGameOutcome.None:
                throw new ApplicationException("GameOutcome None but Game ended.");
        }
        Console.WriteLine("Game over! Press any key to go to the main menu.");
        Console.ReadKey();
        Menus.HomeMenu.Run();
    }

    private static void ExecuteGameAction(GameBrain.GameBrain gameInstance)
    {
        switch (_action)
        {
            case EGameAction.PlaceMarker:
                if (!gameInstance.PlaceMarker(_currentX, _currentY))
                {
                    Console.WriteLine("Invalid Move. The cell is already occupied. Press any key to continue.");
                    Console.ReadKey();
                }
                break;
            case EGameAction.MoveMarker:
                HandleMarkerMovement(gameInstance);
                break;
            case EGameAction.MoveGrid:
                HandleGridMovement(gameInstance);
                break;
        }
    }
    
    private static void HandleMarkerMovement(GameBrain.GameBrain gameInstance)
    {
        var oldX = _currentX;
        var oldY = _currentY;
        Console.WriteLine("Select where to move the marker (use arrow keys):");
        
        bool isMarkerMoveConfirmed = false;
        while (!isMarkerMoveConfirmed)
        {
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    _currentY = (_currentY > 0) ? _currentY - 1 : 0;
                    break;
                case ConsoleKey.DownArrow:
                    _currentY = (_currentY < gameInstance.BoardDimY - 1) ? _currentY + 1 : gameInstance.BoardDimY - 1;
                    break;
                case ConsoleKey.LeftArrow:
                    _currentX = (_currentX > 0) ? _currentX - 1 : 0;
                    break;
                case ConsoleKey.RightArrow:
                    _currentX = (_currentX < gameInstance.BoardDimX - 1) ? _currentX + 1 : gameInstance.BoardDimX - 1;
                    break;
                case ConsoleKey.Enter:
                    if (!gameInstance.MoveMarker(oldX, oldY, _currentX, _currentY))
                    {
                        Console.WriteLine("Invalid Move. You cannot move that marker there. Press any key to continue.");
                        Console.ReadKey();
                        break;
                    } 
                    isMarkerMoveConfirmed = true;
                    break;
                case ConsoleKey.E:
                    Console.WriteLine("Goodbye!");
                    return;
            }
            
            Visualizer.DrawBoard(gameInstance, _currentX, _currentY);
        }
    }

    private static void HandleGridMovement(GameBrain.GameBrain gameInstance)
    {
        if (!gameInstance.MoveGrid(_currentX, _currentY))
        {
            Console.WriteLine("Invalid Move. The grid cannot move there. Press any key to continue.");
            Console.ReadKey();
        }
    }

    private static void ResetGame(GameBrain.GameBrain gameInstance)
    {
        _currentX = 0;
        _currentY = 0;
        gameInstance.ResetGame();
    }
}
