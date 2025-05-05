namespace Common.Entities;

/// <summary>
/// Represents the different actions a player can take during their turn.
/// </summary>
public enum EGameAction
{
    /// <summary>Place a new marker on the board.</summary>
    PlaceMarker,
    
    /// <summary>Move an existing marker to a new position.</summary>
    MoveMarker,
    
    /// <summary>Move the grid to a new position.</summary>
    MoveGrid
}
