using Common.Entities;
using CheckDirection = Common.Entities.EGameOutcomeCheckDirection;

namespace GameLogic;

public static class GameOutcomeChecker
{
    public static EGameOutcome CheckGameOutcome(GameState state, GameConfiguration configuration)
    {
        var player1Wins = CheckForPlayerWin(EGamePiece.Player1, state, configuration);
        var player2Wins = CheckForPlayerWin(EGamePiece.Player2, state, configuration);

        return player1Wins switch
        {
            true when player2Wins => EGameOutcome.Draw,
            true => EGameOutcome.Player1Won,
            _ => player2Wins ? EGameOutcome.Player2Won : EGameOutcome.None
        };
    }

    private static bool CheckForPlayerWin(EGamePiece player, GameState state, GameConfiguration configuration)
    {
        return CheckLines(player, CheckDirection.Vertical, state, configuration)
               || CheckLines(player, CheckDirection.Horizontal, state, configuration)
               || CheckLines(player, CheckDirection.DiagonalTopLeftToBottomRight, state, configuration)
               || CheckLines(player, CheckDirection.DiagonalBottomLeftToTopRight, state, configuration);
    }

    private static bool CheckLines(EGamePiece player, CheckDirection direction, GameState state,
        GameConfiguration configuration)
    {
        var gridX = state.GridX;
        var gridY = state.GridY;

        var xLimit = gridX + configuration.GridWidth - 1;
        var yLimit = gridY + configuration.GridHeight - 1;

        for (var x = gridX; x <= xLimit; x++)
        {
            for (var y = gridY; y <= yLimit; y++)
            {
                if (IsWinningLine(x, y, player, direction, xLimit, yLimit, state, configuration))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static bool IsWinningLine(int startX, int startY, EGamePiece player, CheckDirection direction, int xLimit,
        int yLimit, GameState state, GameConfiguration configuration)
    {
        if (!HasSpaceForWinCondition(startX, startY, direction, xLimit, yLimit, state, configuration))
            return false;

        try
        {
            for (var i = 0; i < configuration.WinCondition; i++)
            {
                var currentPiece = direction switch
                {
                    CheckDirection.Vertical => state.GameBoard[startX][startY + i],
                    CheckDirection.Horizontal => state.GameBoard[startX + i][startY],
                    CheckDirection.DiagonalTopLeftToBottomRight => state.GameBoard[startX + i][startY + i],
                    CheckDirection.DiagonalBottomLeftToTopRight => state.GameBoard[startX + i][startY - i],
                    _ => throw new ArgumentOutOfRangeException()
                };

                if (currentPiece != player)
                    return false;
            }
        }
        catch (IndexOutOfRangeException)
        {
            return false;
        }

        return true;
    }

    private static bool HasSpaceForWinCondition(int startX, int startY, CheckDirection direction, int xLimit, int yLimit,
        GameState state, GameConfiguration configuration)
    {
        var winCondition = configuration.WinCondition;
        return direction switch
        {
            CheckDirection.Vertical => startY + winCondition - 1 <= yLimit,
            CheckDirection.Horizontal => startX + winCondition - 1 <= xLimit,
            CheckDirection.DiagonalTopLeftToBottomRight =>
                startX + winCondition - 1 <= xLimit && startY + configuration.WinCondition - 1 <= yLimit,
            CheckDirection.DiagonalBottomLeftToTopRight =>
                startX + winCondition - 1 <= xLimit && startY - winCondition + 1 >= state.GridY,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
