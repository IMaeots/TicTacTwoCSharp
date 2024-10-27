using Common.Entities;
using CheckDirection = Common.Entities.EGameOutcomeCheckDirection;

namespace GameBrain;

public class GameOutcomeChecker(GameState gameState)
{
    private readonly GameConfiguration _config = gameState.GameConfiguration;
    private readonly EGamePiece[][] _gameBoard = gameState.GameBoard;
    private readonly int _gridX = gameState.GridX;
    private readonly int _gridY = gameState.GridY;

    public EGameOutcome CheckGameOutcome()
    {
        var player1Wins = CheckForPlayerWin(EGamePiece.Player1);
        var player2Wins = CheckForPlayerWin(EGamePiece.Player2);

        return player1Wins switch
        {
            true when player2Wins => EGameOutcome.Draw,
            true => EGameOutcome.Player1Won,
            _ => player2Wins ? EGameOutcome.Player2Won : EGameOutcome.None
        };
    }
    
    private bool CheckForPlayerWin(EGamePiece player)
    {
        return CheckLines(player, CheckDirection.Vertical) 
            || CheckLines(player, CheckDirection.Horizontal)
            || CheckLines(player, CheckDirection.DiagonalTopLeftToBottomRight)
            || CheckLines(player, CheckDirection.DiagonalBottomLeftToTopRight);
    }
    
    private bool CheckLines(EGamePiece player, CheckDirection direction)
    {
        var xLimit = _gridX + _config.GridWidth - 1;
        var yLimit = _gridY + _config.GridHeight - 1;

        for (var x = _gridX; x <= xLimit; x++)
        {
            for (var y = _gridY; y <= yLimit; y++)
            {
                if (IsWinningLine(x, y, player, direction, xLimit, yLimit))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool IsWinningLine(int startX, int startY, EGamePiece player, CheckDirection direction, int xLimit, int yLimit)
    {
        if (!HasSpaceForWinCondition(startX, startY, direction, xLimit, yLimit))
            return false;

        try
        {
            for (var i = 0; i < _config.WinCondition; i++)
            {
                var currentPiece = direction switch
                {
                    CheckDirection.Vertical => _gameBoard[startX][startY + i],
                    CheckDirection.Horizontal => _gameBoard[startX + i][startY],
                    CheckDirection.DiagonalTopLeftToBottomRight => _gameBoard[startX + i][startY + i],
                    CheckDirection.DiagonalBottomLeftToTopRight => _gameBoard[startX + i][startY - i],
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

    private bool HasSpaceForWinCondition(int startX, int startY, CheckDirection direction, int xLimit, int yLimit)
    {
        return direction switch
        {
            CheckDirection.Vertical => startY + _config.WinCondition - 1 <= yLimit,
            CheckDirection.Horizontal => startX + _config.WinCondition - 1 <= xLimit,
            CheckDirection.DiagonalTopLeftToBottomRight =>
                startX + _config.WinCondition - 1 <= xLimit && startY + _config.WinCondition - 1 <= yLimit,
            CheckDirection.DiagonalBottomLeftToTopRight =>
                startX + _config.WinCondition - 1 <= xLimit && startY - _config.WinCondition + 1 >= _gridY,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
