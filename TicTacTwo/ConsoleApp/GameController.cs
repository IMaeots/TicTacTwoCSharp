using Common;
using Common.Entities;
using ConsoleApp.MenuSystem;
using GameLogic;

namespace ConsoleApp;

public static class GameController
{
    private static int _currentX;
    private static int _currentY;
    private static EGameAction _action;

    public static string PlayGame(Game game, EGamePiece userPlayerType, Func<Game> getRefreshedGame, Action<Game> saveGameState, Action<Game> deleteGame)
    {
        Visualizer.DrawBoard(game, _currentX, _currentY);
        while (game.State.GameOutcome == EGameOutcome.None)
        {
            Visualizer.DrawBoard(game, _currentX, _currentY);
            Console.CursorVisible = false;

            if (game.IsBotsTurn(userPlayerType))
            {
                Console.WriteLine("It is BOT's turn.");
                Console.WriteLine("Press [L] to leave or any other key to make BOT make a move.");
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.L) return Constants.ReturnToMainShortcut;
                GameBot.MakeMove(game);
            }
            else if (game.IsUsersTurn(userPlayerType))
            {
                _action = EGameAction.PlaceMarker;
                var nextMoveBy = game.State.NextMoveBy;
                var canPlaceMarker = game.CanPlaceMarker();
                if (!game.CanPerformSpecialMoves() && canPlaceMarker)
                {
                    Console.WriteLine($"Place a marker: {nextMoveBy}!");
                }
                else
                {
                    Console.WriteLine(
                        $"{nextMoveBy}'s turn! Choose an option: [P] Place marker, [G] Move grid, [M] Move marker:");
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

                                Console.WriteLine(
                                    $"Choose another option as you have already placed all your markers! ");
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
                                if (ConsoleMenu.ConfirmExit(Constants.ConfirmLeaveGameText))
                                    return Constants.LeaveGameShortcut;
                                break;
                            case ConsoleKey.E:
                                if (ConsoleMenu.ConfirmExit(Constants.ConfirmExitText))
                                {
                                    Console.WriteLine("See you soon!");
                                    return Constants.ManualExitShortcut;
                                }

                                break;
                        }

                        if (!isSelected)
                        {
                            Visualizer.DrawBoard(game, _currentX, _currentY);
                            Console.WriteLine(
                                $"{nextMoveBy}'s turn! Choose an option: [P] Place marker, [G] Move grid, [M] Move marker:");
                        }
                    }
                }

                var isActionPerformed = false;
                while (!isActionPerformed)
                {
                    var movementKey = Console.ReadKey(true).Key;
                    switch (movementKey)
                    {
                        case ConsoleKey.UpArrow:
                            _currentY = (_currentY > 0) ? _currentY - 1 : 0;
                            break;
                        case ConsoleKey.DownArrow:
                            _currentY = (_currentY < game.Configuration.BoardHeight - 1)
                                ? _currentY + 1
                                : game.Configuration.BoardHeight - 1;
                            break;
                        case ConsoleKey.LeftArrow:
                            _currentX = (_currentX > 0) ? _currentX - 1 : 0;
                            break;
                        case ConsoleKey.RightArrow:
                            _currentX = (_currentX < game.Configuration.BoardWidth - 1)
                                ? _currentX + 1
                                : game.Configuration.BoardWidth - 1;
                            break;
                        case ConsoleKey.Enter:
                            if (_action == EGameAction.MoveMarker && !game.CanMoveThatMarker(_currentX, _currentY))
                            {
                                Console.WriteLine("You must pick your own marker from inside the grid!");
                                Console.ReadKey();
                                break;
                            }

                            ExecuteGameAction(game);
                            isActionPerformed = true;
                            break;
                        case ConsoleKey.L:
                            if (ConsoleMenu.ConfirmExit(Constants.ConfirmLeaveGameText))
                                return Constants.LeaveGameShortcut;
                            break;
                        case ConsoleKey.E:
                            if (ConsoleMenu.ConfirmExit(Constants.ConfirmExitText))
                            {
                                Console.WriteLine("See you soon!");
                                return Constants.ManualExitShortcut;
                            }

                            break;
                    }

                    Visualizer.DrawBoard(game, _currentX, _currentY);
                    switch (_action)
                    {
                        case EGameAction.PlaceMarker:
                            Console.WriteLine(!game.CanPerformSpecialMoves()
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
            }
            else
            {
                Console.WriteLine("It is the oppositions turn. ");
                Console.WriteLine("Press [L] to leave the game or any other key to refresh and see if the opponent has made a move.");
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.L) return Constants.LeaveGameShortcut;
                game = getRefreshedGame();
                continue;
            }

            game.UpdateGameOutcome();
            saveGameState(game);
        }

        Console.Clear();
        Visualizer.DrawBoard(game, _currentX, _currentY);
        switch (game.State.GameOutcome)
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
        try { deleteGame(game); }
        catch (Exception) { /* ignored */ }

        return Constants.ReturnToMainShortcut;
    }

    private static void ExecuteGameAction(Game game)
    {
        switch (_action)
        {
            case EGameAction.PlaceMarker:
                if (!game.PlaceMarker(_currentX, _currentY))
                {
                    Console.WriteLine("Invalid Move. Press any key to continue.");
                    Console.ReadKey();
                }

                break;
            case EGameAction.MoveMarker:
                HandleMarkerMovement(game);
                break;
            case EGameAction.MoveGrid:
                HandleGridMovement(game);
                break;
        }
    }

    private static void HandleMarkerMovement(Game game)
    {
        var boardWidth = game.State.GameBoard.Length;
        var boardHeight = game.State.GameBoard[0].Length;
        var oldX = _currentX;
        var oldY = _currentY;

        Console.WriteLine("Select where to move the marker (use arrow keys):");
        var isMarkerMoveConfirmed = false;
        while (!isMarkerMoveConfirmed)
        {
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    _currentY = (_currentY > 0) ? _currentY - 1 : 0;
                    break;
                case ConsoleKey.DownArrow:
                    _currentY = (_currentY < boardHeight - 1) ? _currentY + 1 : boardHeight - 1;
                    break;
                case ConsoleKey.LeftArrow:
                    _currentX = (_currentX > 0) ? _currentX - 1 : 0;
                    break;
                case ConsoleKey.RightArrow:
                    _currentX = (_currentX < boardWidth - 1) ? _currentX + 1 : boardWidth - 1;
                    break;
                case ConsoleKey.Enter:
                    if (!game.MoveMarker(oldX, oldY, _currentX, _currentY))
                    {
                        Console.WriteLine(
                            "Invalid Move. You cannot move that marker there. Press any key to continue.");
                        Console.ReadKey();
                        break;
                    }

                    isMarkerMoveConfirmed = true;
                    break;
            }

            Visualizer.DrawBoard(game, _currentX, _currentY);
        }
    }

    private static void HandleGridMovement(Game game)
    {
        if (game.MoveGrid(_currentX, _currentY)) return;

        Console.WriteLine("Invalid Move. The grid cannot move there. Press any key to continue.");
        Console.ReadKey();
    }
}
