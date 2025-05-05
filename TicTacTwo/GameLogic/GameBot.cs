using Common.Entities;

namespace GameLogic;

public static class GameBot
{
    private static readonly Random Random = new();

    public static void MakeMove(this Game game)
    {
        if (game.CanPlaceMarker())
        {
            var markerPlacement = GetBotMarkerPlace(game);
            game.PlaceMarker(markerPlacement.x, markerPlacement.y);
        }
        else if (game.CanPerformSpecialMoves())
        {
            var gridMove = GetBotGridMove(game);
            var markerMove = GetBotMarkerMove(game);
            
            if (Random.NextDouble() > 0.5)
            {
                if (gridMove != (-1, -1))
                {
                    game.MoveGrid(gridMove.x, gridMove.y);
                }
                else if (markerMove != (-1, -1, -1, -1))
                {
                    game.MoveMarker(markerMove.oldX, markerMove.oldY, markerMove.newX, markerMove.newY);
                }
            }
            else
            {
                if (markerMove != (-1, -1, -1, -1))
                {
                    game.MoveMarker(markerMove.oldX, markerMove.oldY, markerMove.newX, markerMove.newY);
                }
                else if (gridMove != (-1, -1))
                {
                    game.MoveGrid(gridMove.x, gridMove.y);
                }
            }
        }
    }

    private static (int x, int y) GetBotMarkerPlace(Game game)
    {
        var winningMove = FindWinningMove(game);
        if (winningMove != (-1, -1)) return winningMove;

        var blockingMove = FindBlockingMove(game);
        return blockingMove != (-1, -1) ? blockingMove : ChooseStrategicMove(game);
    }

    private static (int x, int y) FindWinningMove(Game game)
    {
        var bot = game.State.NextMoveBy;
        for (var x = game.State.GridX; x < game.State.GridX + game.Configuration.GridWidth; x++)
        {
            for (var y = game.State.GridY; y < game.State.GridY + game.Configuration.GridHeight; y++)
            {
                if (game.State.GameBoard[x][y] == EGamePiece.Empty)
                {
                    if (SimulateMove(game, x, y, bot)) return (x, y);
                }
            }
        }

        return (-1, -1);
    }

    private static (int x, int y) FindBlockingMove(Game game)
    {
        var opponent = game.State.NextMoveBy == EGamePiece.Player1 ? EGamePiece.Player2 : EGamePiece.Player1;
        for (var x = game.State.GridX; x < game.State.GridX + game.Configuration.GridWidth; x++)
        {
            for (var y = game.State.GridY; y < game.State.GridY + game.Configuration.GridHeight; y++)
            {
                if (game.State.GameBoard[x][y] == EGamePiece.Empty)
                {
                    if (SimulateMove(game, x, y, opponent)) return (x, y);
                }
            }
        }

        return (-1, -1);
    }

    private static (int x, int y) ChooseStrategicMove(Game game)
    {
        var strategicPositions = new (int x, int y)[]
        {
            (game.State.GridX + game.Configuration.GridWidth / 2, game.State.GridY + game.Configuration.GridHeight / 2),
            (game.State.GridX, game.State.GridY),
            (game.State.GridX, game.State.GridY + game.Configuration.GridHeight - 1),
            (game.State.GridX + game.Configuration.GridWidth - 1, game.State.GridY),
            (game.State.GridX + game.Configuration.GridWidth - 1, game.State.GridY + game.Configuration.GridHeight - 1)
        };

        foreach (var (x, y) in strategicPositions)
        {
            if (x >= game.State.GridX && x < game.State.GridX + game.Configuration.GridWidth &&
                y >= game.State.GridY && y < game.State.GridY + game.Configuration.GridHeight &&
                game.State.GameBoard[x][y] == EGamePiece.Empty)
            {
                return (x, y);
            }
        }

        for (var x = game.State.GridX; x < game.State.GridX + game.Configuration.GridWidth; x++)
        {
            for (var y = game.State.GridY; y < game.State.GridY + game.Configuration.GridHeight; y++)
            {
                if (game.State.GameBoard[x][y] == EGamePiece.Empty) return (x, y);
            }
        }

        return (-1, -1);
    }

    private static (int x, int y) GetBotGridMove(Game game)
    {
        var allOneUnitAways = new List<(int x, int y)>();
        for (var x = game.State.GridX - 1; x <= game.State.GridX + 1; x++)
        {
            for (var y = game.State.GridY - 1; y <= game.State.GridY + 1; y++)
            {
                if (x >= 0 && y >= 0 && x + game.Configuration.GridWidth <= game.Configuration.BoardWidth &&
                    y + game.Configuration.GridHeight <= game.Configuration.BoardHeight)
                {
                    if (x != game.State.GridX || y != game.State.GridY)
                    {
                        allOneUnitAways.Add((x, y));
                    }
                }
            }
        }

        if (allOneUnitAways.Count <= 0) return (-1, -1);

        var randomIndex = Random.Next(allOneUnitAways.Count);
        return allOneUnitAways[randomIndex];
    }


    private static (int oldX, int oldY, int newX, int newY) GetBotMarkerMove(Game game)
    {
        var markerToMove = FindMarkerToMove(game);
        
        if (markerToMove == (-1, -1)) return (-1, -1, -1, -1);
        
        var winningMove = FindWinningMove(game);
        if (winningMove != (-1, -1))
        {
            return (markerToMove.x, markerToMove.y, winningMove.x, winningMove.y);
        }

        var blockingMove = FindBlockingMove(game);
        if (blockingMove != (-1, -1))
        {
            return (markerToMove.x, markerToMove.y, blockingMove.x, blockingMove.y);
        }

        for (var x = game.State.GridX; x < game.State.GridX + game.Configuration.GridWidth; x++)
        {
            for (var y = game.State.GridY; y < game.State.GridY + game.Configuration.GridHeight; y++)
            {
                if (game.State.GameBoard[x][y] == EGamePiece.Empty) 
                    return (markerToMove.x, markerToMove.y, x, y);
            }
        }

        return (-1, -1, -1, -1);
    }

    private static (int x, int y) FindMarkerToMove(Game game)
    {
        var allBotMarkers = new List<(int x, int y)>();
        for (var x = game.State.GridX; x < game.State.GridX + game.Configuration.GridWidth; x++)
        {
            for (var y = game.State.GridY; y < game.State.GridY + game.Configuration.GridHeight; y++)
            {
                if (game.State.GameBoard[x][y] == game.State.NextMoveBy)
                {
                    allBotMarkers.Add((x, y));
                }
            }
        }

        if (allBotMarkers.Count <= 0) return (-1, -1);

        var randomIndex = Random.Next(allBotMarkers.Count);
        return allBotMarkers[randomIndex];
    }

    private static bool SimulateMove(Game game, int x, int y, EGamePiece player)
    {
        var simulatedState = DeepCopyGameState(game.State);

        simulatedState.GameBoard[x][y] = player;

        var outcome = GameOutcomeChecker.CheckGameOutcome(simulatedState, game.Configuration);

        return outcome == (player == EGamePiece.Player2 ? EGameOutcome.Player2Won : EGameOutcome.Player1Won);
    }

    private static GameState DeepCopyGameState(GameState originalState)
    {
        var clonedBoard = originalState.GameBoard.Select(row => row.ToArray()).ToArray();

        return new GameState(
            clonedBoard,
            originalState.NextMoveBy,
            originalState.GameOutcome,
            originalState.Player1MarkersPlaced,
            originalState.Player2MarkersPlaced,
            originalState.GridX,
            originalState.GridY,
            originalState.MoveCount
        );
    }
}
