# TicTacTwo Console Application

A terminal-based implementation of the TicTacTwo game with interactive navigation and colorful interface.
1 out of 7 TicTacTwos made in Uni for 2024-2025. (1 out of 2 in C#)

## 🎮 Game Description

TicTacTwo is an advanced version of Tic-Tac-Toe with a unique twist: the game is played on a larger board (default 5×5), but only a smaller section (default 3×3) is active at any time. After a configurable number of total moves, players unlock special abilities to either place new markers, move existing ones, or shift the active grid. Each player has a maximum of 6 markers by default.

## Features

- **Multiple Game Modes**:
  - Play against a friend locally
  - Play against the AI
  - Online multiplayer (password-based)
  - Bot vs Bot simulation
- **Dynamic Gameplay**:
  - Place markers in the first phase
  - Move your markers or shift the active grid in the second phase
- **Interactive UI**:
  - Colorful terminal interface
  - Keyboard-based navigation
  - Intuitive controls
- **Game Configuration Management**:
  - Create, edit and save game configurations
  - Customize board size, grid size, win conditions, and more
- **Flexible Data Storage**:
  - SQLite database via Entity Framework Core
  - JSON file-based storage as an alternative

## Architecture

The console application is built on top of the shared game logic components:

- **Program.cs**: Entry point that sets up dependency injection and starts the application
- **GameController**: Handles the game play interactions and control flow
- **GameVisualizer**: Handles rendering the game board to the console
- **MenuSystem**: Provides a menu-based navigation system for the game

## 🚀 Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download) or later
- Terminal/Command Prompt with support for ANSI colors

### Running the Application

1. Navigate to the ConsoleApp directory:
   ```bash
   cd TicTacTwo/ConsoleApp
   ```

2. Build and run the application:
   ```bash
   dotnet run
   ```

3. Follow the on-screen menu instructions to create or load a game

## 🎯 How to Play

1. **Start a Game**:
   - Choose "New Game" from the main menu
   - Select a configuration or create a new one

2. **During Gameplay**:
   - Use arrow keys to navigate the board
   - Press Enter to select a cell or confirm an action
   - Use keyboard shortcuts to change actions when special moves are unlocked

3. **Controls**:
   - **Arrow Keys**: Navigate the board/grid
   - **Enter**: Confirm selection
   - **P**: Select place marker action
   - **G**: Select move grid action
   - **M**: Select move marker action
   - **L**: Leave current game
   - **E**: Exit the application

4. **Winning**:
   - Get markers in a row according to the win condition (default: 3 in a row)
   - If both players create a winning line simultaneously, the game ends in a draw

## Configuration

The application uses the `appsettings.json` file for configuration:

```json
{
  "UseDatabase": true,
  "ConnectionStrings": {
    "DefaultConnection": "DataSource=app.db;Cache=Shared"
  }
}
```

You can toggle between database and file storage by setting `UseDatabase` to `true` or `false`.

## 📝 License

All Rights Reserved

## Author

Name: Indrek Mäeots
School email: inmaeo@taltech.ee
