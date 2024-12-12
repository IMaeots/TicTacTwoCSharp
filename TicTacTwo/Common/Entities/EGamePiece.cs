namespace Common.Entities;

public enum EGamePiece
{
    Empty,
    Player1,
    Player2
}

public static class EGamePieceExtensions
{
    public static string ToSymbol(this EGamePiece piece) =>
        piece switch
        {
            EGamePiece.Player1 => Constants.FirstPlayerSymbol,
            EGamePiece.Player2 => Constants.SecondPlayerSymbol,
            _ => " "
        }; 
}
