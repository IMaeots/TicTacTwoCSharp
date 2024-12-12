namespace GameLogic;

public static class GameBot
{
    public static void MakeMove(Game game)
    {
        if (game.CanPlaceMarker())
        {
            var markerPlacement = GetBotMarkerPlace();
            game.PlaceMarker(markerPlacement.x, markerPlacement.y);
        }
        else if (game.CanPerformSpecialMoves())
        {
            var gridMove = GetBotGridMove();
            if (gridMove != (-1, -1))
            {
                game.MoveGrid(gridMove.x, gridMove.y);
            }
            else
            {
                var markerMove = GetBotMarkerMove();
                game.MoveMarker(markerMove.oldX, markerMove.oldY, markerMove.newX, markerMove.newY);
            }
        }
    }

    private static (int x, int y) GetBotMarkerPlace()
    {
        var winningMove = FindWinningMove();
        if (winningMove != (-1, -1)) return winningMove;

        var blockingMove = FindBlockingMove();
        return FindBlockingMove() != (-1, -1) ? blockingMove : ChooseStrategicMove();
    }

    private static (int x, int y) GetBotGridMove()
    {
        throw new NotImplementedException();
    }

    private static (int oldX, int oldY, int newX, int newY) GetBotMarkerMove()
    {
        throw new NotImplementedException();
    }

    private static (int x, int y) FindWinningMove()
    {
        throw new NotImplementedException();
    }

    private static (int x, int y) FindBlockingMove()
    {
        throw new NotImplementedException();
    }

    private static (int x, int y) ChooseStrategicMove()
    {
        throw new NotImplementedException();
    }

    private static void SimulateMove()
    {
        throw new NotImplementedException();
    }
}
