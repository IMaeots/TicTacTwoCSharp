namespace Common.Entities;

public enum EGameOutcome
{
    None,
    Draw,
    Player1Won,
    Player2Won
}

public enum EGameOutcomeCheckDirection
{
    Vertical,
    Horizontal,
    DiagonalTopLeftToBottomRight,
    DiagonalBottomLeftToTopRight
}
