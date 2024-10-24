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
                if (IsWinningLine(x, y, player, direction))
                {
                    return true;
                }
            }
        }
        return false;
    }

    // TODO: Tweak out the crash cause & make it better like it was (except crash...)
    private bool IsWinningLine(int startX, int startY, EGamePiece player, CheckDirection direction)
    {
        for (var i = 0; i < _config.WinCondition; i++)
        {
            var currentX = startX;
            var currentY = startY;

            switch (direction)
            {
                case CheckDirection.Vertical:
                    currentY += i;
                    break;
                case CheckDirection.Horizontal:
                    currentX += i;
                    break;
                case CheckDirection.DiagonalTopLeftToBottomRight:
                    currentX += i;
                    currentY += i;
                    break;
                case CheckDirection.DiagonalBottomLeftToTopRight:
                    currentX += i;
                    currentY -= i;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            if (currentX < 0 || currentX >= _gameBoard.Length || 
                currentY < 0 || currentY >= _gameBoard[0].Length)
            {
                return false;
            }

            var currentPiece = _gameBoard[currentX][currentY];

            if (currentPiece != player) return false;
        }

        return true;
    }
}
