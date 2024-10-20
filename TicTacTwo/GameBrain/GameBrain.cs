using Common.Entities;

namespace GameBrain;

public class GameBrain
{
    private readonly GameState _gameState;
    
    public EGamePiece[][] GameBoard => _gameState.GameBoard;
    public int BoardWidth => GameBoard.Length;
    public int BoardHeight => GameBoard[0].Length;
    public int GridWidth => _gameState.GameConfiguration.GridWidth;
    public int GridHeight => _gameState.GameConfiguration.GridHeight;
    public EGamePiece NextMoveBy => _gameState.NextMoveBy;
    public int GridX => _gameState.GridX;
    public int GridY => _gameState.GridY;

    public GameBrain(GameConfiguration gameConfiguration)
    {
        _gameState = new GameState(gameConfiguration);
    }
    
    public GameBrain(GameState gameState)
    {
        _gameState = gameState;
    }

    public EGameOutcome CheckForGameEnd()
    {
        var gameOutcomeChecker = new GameOutcomeChecker(_gameState);
        return gameOutcomeChecker.CheckGameOutcome();
    }

    public bool CanPlaceMarker()
    {
        switch (_gameState.NextMoveBy)
        {
            case EGamePiece.Player1:
                return _gameState.Player1MarkersPlaced < _gameState.GameConfiguration.NumberOfMarkers;
            case EGamePiece.Player2:
                return _gameState.Player2MarkersPlaced < _gameState.GameConfiguration.NumberOfMarkers;
            default:
                return false;
        }
    }

    public bool PlaceMarker(int x, int y)
    {
        if (_gameState.GameBoard[x][y] != EGamePiece.Empty)
        {
            return false;
        }

        _gameState.GameBoard[x][y] = _gameState.NextMoveBy;

        if (_gameState.NextMoveBy == EGamePiece.Player1)
        {
            _gameState.Player1MarkersPlaced++;
        }
        else
        {
            _gameState.Player2MarkersPlaced++;
        }
        
        MoveMade();
        return true;
    }
    
    public bool CanMoveThatMarker(int currentX, int currentY)
    {
        return _gameState.GameBoard[currentX][currentY] == _gameState.NextMoveBy;
    }

    public bool MoveMarker(int oldX, int oldY, int newX, int newY)
    {
        if (_gameState.GameBoard[oldX][oldY] == _gameState.NextMoveBy && _gameState.GameBoard[newX][newY] == EGamePiece.Empty)
        {
            _gameState.GameBoard[oldX][oldY] = EGamePiece.Empty;
            _gameState.GameBoard[newX][newY] = _gameState.NextMoveBy;
            
            MoveMade();
            return true;
        }
        return false;
    }

    public bool CanMoveGrid()
    {
        return _gameState.CanMoveGrid();
    }

    public bool MoveGrid(int newGridX, int newGridY)
    {
        var boardWidth = _gameState.GameConfiguration.BoardWidth;
        var boardHeight = _gameState.GameConfiguration.BoardHeight;
        var gridWidth = _gameState.GameConfiguration.GridWidth;
        var gridHeight = _gameState.GameConfiguration.GridHeight;
        
        
        if (newGridX < 0 || newGridX + gridWidth > boardWidth || newGridY < 0 || newGridY + gridHeight > boardHeight)
        {
            return false;
        }

        _gameState.GridX = newGridX;
        _gameState.GridY = newGridY;

        MoveMade();
        return true;
    }

    private void MoveMade()
    {
        _gameState.NextMoveBy = _gameState.NextMoveBy == EGamePiece.Player1 ? EGamePiece.Player2 : EGamePiece.Player1;
        _gameState.MoveCount++;
    }
    
    public void ResetGame()
    {
        _gameState.ResetGameBoard();
    }
}
