using Common.Entities;

namespace GameLogic;

public static class GameExtension
{
    public static bool CanPlaceMarker(this Game game) =>
        game.State.NextMoveBy switch
        {
            EGamePiece.Player1 => game.State.Player1MarkersPlaced < game.Configuration.NumberOfMarkers,
            EGamePiece.Player2 => game.State.Player2MarkersPlaced < game.Configuration.NumberOfMarkers,
            _ => false
        };

    public static bool PlaceMarker(this Game game, int x, int y)
    {
        if (game.State.GameBoard[x][y] != EGamePiece.Empty) return false;

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

    public static bool CanMoveThatMarker(this Game game, int currentX, int currentY) =>
        game.State.GameBoard[currentX][currentY] == game.State.NextMoveBy;

    public static bool MoveMarker(this Game game, int oldX, int oldY, int newX, int newY)
    {
        try
        {
            var canMakeThatMove = game.State.GameBoard[oldX][oldY] == game.State.NextMoveBy &&
                                  game.State.GameBoard[newX][newY] == EGamePiece.Empty;
            if (!canMakeThatMove) return false;
        }
        catch { return false; }

        game.State.GameBoard[oldX][oldY] = EGamePiece.Empty;
        game.State.GameBoard[newX][newY] = game.State.NextMoveBy;

        game.MoveMade();
        return true;
    }

    public static bool CanPerformSpecialMoves(this Game game) =>
        game.State.MoveCount / 2 >= game.Configuration.UnlockSpecialMovesAfterNMoves;

    public static bool MoveGrid(this Game game, int newGridX, int newGridY)
    {
        var isOneUnitAway = Math.Abs(newGridX - game.State.GridX) <= 1 && Math.Abs(newGridY - game.State.GridY) <= 1 &&
                            !(newGridX == game.State.GridX && newGridY == game.State.GridY);

        if (!isOneUnitAway || newGridX < 0 || newGridX + game.Configuration.GridWidth > game.Configuration.BoardWidth
            || newGridY < 0 || newGridY + game.Configuration.GridHeight > game.Configuration.BoardHeight)
        {
            return false;
        }

        game.State.GridX = newGridX;
        game.State.GridY = newGridY;

        game.MoveMade();
        return true;
    }

    public static void UpdateGameOutcome(this Game game) =>
        game.State.GameOutcome = GameOutcomeChecker.CheckGameOutcome(game.State, game.Configuration);

    private static void MoveMade(this Game game)
    {
        game.State.NextMoveBy = game.State.NextMoveBy == EGamePiece.Player1 ? EGamePiece.Player2 : EGamePiece.Player1;
        game.State.MoveCount++;
    }

    public static bool IsBotsTurn(this Game game, EGamePiece? userPlayerType) => userPlayerType != null && 
        (game.Configuration.Mode is EGameMode.Bots || (game.Configuration.Mode is EGameMode.SinglePlayer && userPlayerType != game.State.NextMoveBy));

    public static bool IsUsersTurn(this Game game, EGamePiece? userPlayerType) => userPlayerType != null 
        && (game.State.NextMoveBy == userPlayerType || (userPlayerType == EGamePiece.Empty && !game.IsBotsTurn(userPlayerType)));

    public static bool IsPasswordNeeded(this Game game) => game.Configuration.Mode is EGameMode.SinglePlayer or EGameMode.OnlineTwoPlayer;
    
    public static EGamePiece? GetPlayerTypeByPassword(this Game game, string? password)
    {
        return game.Configuration.Mode switch
        {
            EGameMode.SinglePlayer or EGameMode.OnlineTwoPlayer when password == game.PasswordP1 => EGamePiece.Player1,
            EGameMode.OnlineTwoPlayer when password == game.PasswordP2 => EGamePiece.Player2,
            _ => null
        };
    }
}
