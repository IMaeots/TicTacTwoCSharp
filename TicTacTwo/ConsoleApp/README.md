# TicTacTwo Console Application

The console application provides a terminal-based interface for playing the TicTacTwo game.
1 out of 7 TicTacTwos made in Uni for 2024-2025. (1 out of 2 in C#)

## Architecture

C# has common logic and data.
More console specific components are here:

- **Program.cs**: Entry point that sets up dependency injection and starts the application
- **GameController**: Handles the game play interactions and control flow
- **GameVisualizer**: Handles rendering the game board to the console
- **MenuSystem**: Provides a menu-based navigation system for the game

## Features

- Interactive menu system for game navigation
- Game configuration creation and management
- Visual game board with color-coded cells
- Support for all game modes:
  - Single player (against AI)
  - Local multiplayer
  - Online multiplayer (password-based)
  - AI vs AI simulation
- Flexible data storage:
  - SQLite database storage
  - JSON file-based storage

## How to Play

1. Start the application by running `dotnet run` in the ConsoleApp directory
2. Use the menu system to create a new game or load a saved game
3. Use arrow keys to navigate the game board
4. Press Enter to make a move
5. Special moves (grid movement, marker movement) are unlocked after a certain number of moves

## Controls

- **Arrow Keys**: Navigate the board/grid
- **Enter**: Confirm selection
- **P**: Select place marker action
- **G**: Select move grid action
- **M**: Select move marker action
- **L**: Leave current game
- **E**: Exit the application

## Configuration

The application uses an `appsettings.json` file for configuration:

```json
{
  "UseDatabase": true,
  "DatabasePath": "TicTacTwo.db",
  "GameSettings": {
    "DefaultBoardWidth": 5,
    "DefaultBoardHeight": 5,
    "DefaultGridWidth": 3,
    "DefaultGridHeight": 3,
    "DefaultWinCondition": 3
  }
}
```

You can modify these settings to customize the default game behavior. Set `"UseDatabase": false` to use file-based storage instead of the SQLite database.
