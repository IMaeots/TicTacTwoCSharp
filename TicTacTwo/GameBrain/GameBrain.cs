using Common.Entities;

namespace GameBrain;

public class GameBrain
{
    private readonly GameState _gameState;
    
    public int BoardDimX => _gameState.GameBoard.Length;
    public int BoardDimY => _gameState.GameBoard[0].Length;
    public int GridSize => _gameState.GameConfiguration.GridSize;
    public EGamePiece NextMoveBy => _gameState.NextMoveBy;
    public int GridX { get; private set; }
    public int GridY { get; private set; }
    private int MoveCount { get; set; }

    public GameBrain(GameConfiguration gameConfiguration)
    {
        var gameBoard = new EGamePiece[gameConfiguration.BoardSize][];
        for (var x = 0; x < gameBoard.Length; x++)
        {
            gameBoard[x] = new EGamePiece[gameConfiguration.BoardSize];
        }

        _gameState = new GameState(
            gameBoard,
            gameConfiguration
        );

        GridX = (gameConfiguration.BoardSize - gameConfiguration.GridSize) / 2;
        GridY = (gameConfiguration.BoardSize - gameConfiguration.GridSize) / 2;
        MoveCount = 0;
    }

    public string GetGameConfigName()
    {
        return _gameState.GameConfiguration.Name;
    }

    public EGamePiece[][] GameBoard => GetBoard();

    public bool IsGameOver()
    {
        return false;
    }
    
    private EGamePiece[] GetColumn(int index)
    {
        return Enumerable.Range(0, BoardDimY).Select(y => GameBoard[index][y]).ToArray();
    }

    private EGamePiece[][] GetBoard()
    {
        var copyOfBoard = new EGamePiece[_gameState.GameBoard.GetLength(0)][];
        for (var x = 0; x < _gameState.GameBoard.Length; x++)
        {
            copyOfBoard[x] = new EGamePiece[_gameState.GameBoard[x].Length];
            for (var y = 0; y < _gameState.GameBoard[x].Length; y++)
            {
                copyOfBoard[x][y] = _gameState.GameBoard[x][y];
            }
        }

        return copyOfBoard;
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
        return MoveCount / 2 >= _gameState.GameConfiguration.MoveGridAfterNMoves;
    }

    public bool MoveGrid(int newGridX, int newGridY)
    {
        var gridSize = _gameState.GameConfiguration.GridSize;
        var boardSize = _gameState.GameConfiguration.BoardSize;
        
        if (newGridX < 0 || newGridX + gridSize > boardSize || newGridY < 0 || newGridY + gridSize > boardSize)
        {
            return false;
        }

        GridX = newGridX;
        GridY = newGridY;

        MoveMade();
        return true;
    }

    private void MoveMade()
    {
        _gameState.NextMoveBy = _gameState.NextMoveBy == EGamePiece.Player1 ? EGamePiece.Player2 : EGamePiece.Player1;
        MoveCount++;
    }

    public void ResetGame()
    {
        var boardSize = _gameState.GameConfiguration.BoardSize;
        
        _gameState.GameBoard = Enumerable
            .Range(0, boardSize)
            .Select(_ => new EGamePiece[boardSize])
            .ToArray();
        _gameState.NextMoveBy = EGamePiece.Player1;
        _gameState.Player1MarkersPlaced = _gameState.Player2MarkersPlaced = 0;
    }
}
