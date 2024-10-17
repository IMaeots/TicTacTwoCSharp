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
        var xLimit = _gridX + _config.GridSize - 1;
        var yLimit = _gridY + _config.GridSize - 1;

        for (var x = _gridX; x <= xLimit; x++)
        {
            for (var y = _gridY; y <= yLimit; y++)
            {
                if (IsWinningLine(x, y, player, direction))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool IsWinningLine(int startX, int startY, EGamePiece player, CheckDirection direction)
    {
        for (var i = 0; i < _config.WinCondition; i++)
        {
            var currentPiece = direction switch
            {
                CheckDirection.Vertical => _gameBoard[startX][startY + i],
                CheckDirection.Horizontal => _gameBoard[startX + i][startY],
                CheckDirection.DiagonalTopLeftToBottomRight => _gameBoard[startX + i][startY + i],
                CheckDirection.DiagonalBottomLeftToTopRight => _gameBoard[startX + i][startY - i], // TODO: Crash in classical if 5th marker put on (1,1) spot... + got another crash.
                _ => throw new ArgumentOutOfRangeException()
            };

            if (currentPiece != player) return false;
        }

        return true;
    }
}
