using Common;
using Common.Entities;
using GameLogic;

namespace ConsoleApp;

public class GameController
{
    private const string ConfirmExitText = "Are you sure you want to close the game? (Y/N)";
    private const string ConfirmLeaveGameText = "Are you sure you want to leave the game? (Y/N)";
    
    private int _currentX;
    private int _currentY;
    private EGameAction _action;
    private Game _game;
    private readonly EGamePiece _userPlayerType;
    private readonly Func<Game> _getRefreshedGame;
    private readonly Action<Game> _saveGameState;
    private readonly Action<Game> _deleteGame;

    public GameController(
        Game game, 
        EGamePiece userPlayerType, 
        Func<Game> getRefreshedGame, 
        Action<Game> saveGameState, 
        Action<Game> deleteGame)
    {
        _game = game;
        _userPlayerType = userPlayerType;
        _getRefreshedGame = getRefreshedGame;
        _saveGameState = saveGameState;
        _deleteGame = deleteGame;
        _currentX = _game.State.GridX;
        _currentY = _game.State.GridY;
        _action = EGameAction.PlaceMarker;
    }

    public string Play()
    {
        DrawBoard();
        while (_game.State.GameOutcome == EGameOutcome.None)
        {
            DrawBoard();
            Console.CursorVisible = false;

            if (_game.IsBotsTurn(_userPlayerType))
            {
                if (HandleBotTurn()) return Constants.ReturnToMainShortcut;
            }
            else if (_game.IsUsersTurn(_userPlayerType))
            {
                if (HandleUserTurn()) return Constants.LeaveGameShortcut;
            }
            else
            {
                if (HandleOpponentTurn()) return Constants.LeaveGameShortcut;
                continue;
            }

            _game.UpdateGameOutcome();
            _saveGameState(_game);
        }

        HandleGameEnd();
        return Constants.ReturnToMainShortcut;
    }

    private void DrawBoard() => GameVisualizer.DrawBoard(_game, _currentX, _currentY);

    private bool HandleBotTurn()
    {
        Console.WriteLine("It is BOT's turn.");
        Console.WriteLine("Press [L] to leave or any other key to make BOT make a move.");
        var key = Console.ReadKey();
        if (key.Key == ConsoleKey.L) return true;
        GameBot.MakeMove(_game);
        return false;
    }

    private bool HandleUserTurn()
    {
        if (!SelectAction()) return true;
        return !ExecuteUserAction();
    }

    // Assumption: no more than 2 players trying to play together.
    private bool HandleOpponentTurn()
    {
        Console.WriteLine("It is the opponents turn. ");
        Console.WriteLine("Press [L] to leave the game or any other key to refresh and see if the opponent has made a move.");
        while (_game.State.NextMoveBy != _userPlayerType)
        {
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.L) return true;

            var refreshedGame = _getRefreshedGame();
            if (_game.State.NextMoveBy == refreshedGame.State.NextMoveBy)
            {
                Console.WriteLine("They have not made a move. Try again.");
            }
            else
            {
                Console.WriteLine("Opponent has made a move! Updating game state...");
                _currentX = refreshedGame.State.GridX;
                _currentY = refreshedGame.State.GridY;
                _game = refreshedGame;
                break;
            }
        }

        return false;
    }

    private bool SelectAction()
    {
        _action = EGameAction.PlaceMarker;
        var nextMoveBy = _game.State.NextMoveBy;
        var canPlaceMarker = _game.CanPlaceMarker();
        
        if (!_game.CanPerformSpecialMoves() && canPlaceMarker)
        {
            Console.WriteLine($"Place a marker: {nextMoveBy}!");
            return true;
        }
        
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
                    }
                    else
                    {
                        Console.WriteLine("All markers placed. Choose another option");
                        Console.ReadKey();
                    }
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
                case ConsoleKey.L:
                    if (ConfirmExit(ConfirmLeaveGameText))
                        return false;
                    break;
                case ConsoleKey.E:
                    if (ConfirmExit(ConfirmExitText))
                    {
                        Console.WriteLine("See you soon!");
                        return false;
                    }
                    break;
            }

            if (!isSelected)
            {
                DrawBoard();
                Console.WriteLine($"{nextMoveBy}'s turn! Choose an option: [P] Place marker, [G] Move grid, [M] Move marker:");
            }
        }
        
        return true;
    }

    private bool ExecuteUserAction()
    {
        var isActionPerformed = false;
        var nextMoveBy = _game.State.NextMoveBy;
        
        while (!isActionPerformed)
        {
            if (_action == EGameAction.MoveGrid)
            {
                PerformGameAction();
                isActionPerformed = true;
                break;
            }
            else
            {
                var movementKey = Console.ReadKey(true).Key;
                switch (movementKey)
                {
                    case ConsoleKey.UpArrow:
                        _currentY = Math.Max(0, _currentY - 1);
                        break;
                    case ConsoleKey.DownArrow:
                        _currentY = Math.Min(_game.Configuration.BoardHeight - 1, _currentY + 1);
                        break;
                    case ConsoleKey.LeftArrow:
                        _currentX = Math.Max(0, _currentX - 1);
                        break;
                    case ConsoleKey.RightArrow:
                        _currentX = Math.Min(_game.Configuration.BoardWidth - 1, _currentX + 1);
                        break;
                    case ConsoleKey.Enter:
                        if (_action == EGameAction.MoveMarker && !_game.CanMoveThatMarker(_currentX, _currentY))
                        {
                            Console.WriteLine("You must pick your own marker from inside the grid!");
                            Console.ReadKey();
                            break;
                        }

                        PerformGameAction();
                        isActionPerformed = true;
                        break;
                    case ConsoleKey.L:
                        if (ConfirmExit(ConfirmLeaveGameText))
                            return false;
                        break;
                    case ConsoleKey.E:
                        if (ConfirmExit(ConfirmExitText))
                        {
                            Console.WriteLine("See you soon!");
                            return false;
                        }
                        break;
                }   
            }

            DrawBoard();
            switch (_action)
            {
                case EGameAction.PlaceMarker:
                    Console.WriteLine(!_game.CanPerformSpecialMoves()
                        ? $"Place a marker: {nextMoveBy}!"
                        : $"You chose to place a marker. Go ahead {nextMoveBy}!"
                    );
                    break;
                case EGameAction.MoveMarker:
                    Console.WriteLine($"You chose to move a marker. Go ahead {nextMoveBy}!");
                    break;
                case EGameAction.MoveGrid:
                    Console.WriteLine($"You chose to move the grid. Go ahead {nextMoveBy}!");
                    break;
            }
        }
        
        return true;
    }

    private void PerformGameAction()
    {
        switch (_action)
        {
            case EGameAction.PlaceMarker:
                HandleMarkerPlacement();
                break;
            case EGameAction.MoveMarker:
                HandleMarkerMovement();
                break;
            case EGameAction.MoveGrid:
                HandleGridMovement();
                break;
        }
    }

    private void HandleMarkerPlacement()
    {
        if (!_game.PlaceMarker(_currentX, _currentY))
        {
            Console.WriteLine("Invalid move. Cell is already occupied or outside the grid.");
            Console.ReadKey();
        }
    }

    private void HandleMarkerMovement()
    {
        Console.WriteLine("Select the destination cell for your marker (use arrow keys and Enter).");

        var originalX = _currentX;
        var originalY = _currentY;

        var isDone = false;
        while (!isDone)
        {
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    _currentY = Math.Max(0, _currentY - 1);
                    break;
                case ConsoleKey.DownArrow:
                    _currentY = Math.Min(_game.Configuration.BoardHeight - 1, _currentY + 1);
                    break;
                case ConsoleKey.LeftArrow:
                    _currentX = Math.Max(0, _currentX - 1);
                    break;
                case ConsoleKey.RightArrow:
                    _currentX = Math.Min(_game.Configuration.BoardWidth - 1, _currentX + 1);
                    break;
                case ConsoleKey.Enter:
                    if (_game.MoveMarker(originalX, originalY, _currentX, _currentY))
                    {
                        isDone = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid move. Destination must be empty and inside the grid.");
                        Console.ReadKey();
                    }
                    break;
                case ConsoleKey.Escape:
                    _currentX = originalX;
                    _currentY = originalY;
                    isDone = true;
                    break;
            }

            DrawBoard();
            Console.WriteLine("Press ESC to cancel");
        }
    }

    private void HandleGridMovement()
    {
        var gridX = _game.State.GridX;
        var gridY = _game.State.GridY;

        Console.WriteLine("Use arrow keys (↑↓←→) to move grid, or ESC to cancel");

        while (true)
        {
            var key = Console.ReadKey(true).Key;

            var prevX = gridX;
            var prevY = gridY;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    gridY = Math.Max(0, gridY - 1);
                    break;
                case ConsoleKey.DownArrow:
                    gridY = Math.Min(_game.Configuration.BoardHeight - _game.Configuration.GridHeight, gridY + 1);
                    break;
                case ConsoleKey.LeftArrow:
                    gridX = Math.Max(0, gridX - 1);
                    break;
                case ConsoleKey.RightArrow:
                    gridX = Math.Min(_game.Configuration.BoardWidth - _game.Configuration.GridWidth, gridX + 1);
                    break;
                case ConsoleKey.Escape:
                    Console.WriteLine("Grid move canceled.");
                    return;
                default:
                    continue;
            }
            
            if (gridX != _game.State.GridX || gridY != _game.State.GridY)
            {
                if (_game.MoveGrid(gridX, gridY))
                {
                    Console.Clear();
                    DrawBoard();
                    _currentX = _game.State.GridX;
                    _currentY = _game.State.GridY;
                    return;
                }
                
                
                Console.WriteLine("Invalid grid move");
                Console.ReadKey(true);
                return;
            }
        }
    }

    private void HandleGameEnd()
    {
        Console.Clear();
        DrawBoard();
        
        switch (_game.State.GameOutcome)
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

        Console.WriteLine("Game over! Press any key to delete the game and return to the main menu.");
        Console.ReadKey();
        
        _deleteGame(_game); 
    }
    
    private bool ConfirmExit(string confirmationMessage)
    {
        Console.WriteLine(confirmationMessage);
        return Console.ReadKey().Key.ToString().ToUpper() == Constants.ConfirmSymbol;
    }
}
