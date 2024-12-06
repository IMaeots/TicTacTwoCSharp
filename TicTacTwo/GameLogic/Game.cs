namespace GameLogic;

public record Game(
    string Name,
    GameConfiguration Configuration,
    GameState State,
    string? PasswordP1,
    string? PasswordP2
);
