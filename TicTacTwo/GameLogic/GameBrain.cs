using Common.Entities;

namespace GameLogic;

public static class GameExtension
{
    public static bool CanPlaceMarker(this Game game)
    {
        return game.State.NextMoveBy switch
        {
            EGamePiece.Player1 => game.State.Player1MarkersPlaced < game.Configuration.NumberOfMarkers,
            EGamePiece.Player2 => game.State.Player2MarkersPlaced < game.Configuration.NumberOfMarkers,
            _ => false
        };
    }

    public static bool PlaceMarker(this Game game, int x, int y)
    {
        if (game.State.GameBoard[x][y] != EGamePiece.Empty)
        {
            return false;
        }

        game.State.GameBoard[x][y] = game.State.NextMoveBy;

        if (game.State.NextMoveBy == EGamePiece.Player1)
        {
            game.State.Player1MarkersPlaced++;
        }
        else
        {
            game.State.Player2MarkersPlaced++;
        }
        
        game.MoveMade();
        return true;
    }
    
    public static bool CanMoveThatMarker(this Game game, int currentX, int currentY)
    {
        return game.State.GameBoard[currentX][currentY] == game.State.NextMoveBy;
    }

    public static bool MoveMarker(this Game game, int oldX, int oldY, int newX, int newY)
    {
        var canMakeThatMove = game.State.GameBoard[oldX][oldY] == game.State.NextMoveBy ||
                              game.State.GameBoard[newX][newY] == EGamePiece.Empty;
        if (!canMakeThatMove) return false;
        
        game.State.GameBoard[oldX][oldY] = EGamePiece.Empty;
        game.State.GameBoard[newX][newY] = game.State.NextMoveBy;
            
        game.MoveMade();
        return true;
    }
    
    public static bool CanPerformSpecialMoves(this Game game)
    {
        return game.State.MoveCount / 2 >= game.Configuration.UnlockSpecialMovesAfterNMoves;
    }

    public static bool MoveGrid(this Game game, int newGridX, int newGridY)
    {
        var boardWidth = game.Configuration.BoardWidth;
        var boardHeight = game.Configuration.BoardHeight;
        var gridWidth = game.Configuration.GridWidth;
        var gridHeight = game.Configuration.GridHeight;
        var oldGridX = game.State.GridX;
        var oldGridY = game.State.GridY;

        var isOneUnitAway = Math.Abs(newGridX - oldGridX) <= 1 && Math.Abs(newGridY - oldGridY) <= 1 &&
                            !(newGridX == oldGridX && newGridY == oldGridY);
        
        if (!isOneUnitAway || newGridX < 0 || newGridX + gridWidth > boardWidth 
            || newGridY < 0 || newGridY + gridHeight > boardHeight)
        {
            return false;
        }

        game.State.GridX = newGridX;
        game.State.GridY = newGridY;

        game.MoveMade();
        return true;
    }

    public static void UpdateGameOutcome(this Game game)
    {
        game.State.GameOutcome = GameOutcomeChecker.CheckGameOutcome(game.State, game.Configuration);
    }

    private static void MoveMade(this Game game)
    {
        game.State.NextMoveBy = game.State.NextMoveBy == EGamePiece.Player1 ? EGamePiece.Player2 : EGamePiece.Player1;
        game.State.MoveCount++;
    }
}
