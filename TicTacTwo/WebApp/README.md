# TicTacTwo Web Application

A responsive web-based implementation of the TicTacTwo game using ASP.NET Core Razor Pages.
1 out of 7 TicTacTwos made in Uni for 2024-2025. (1 out of 2 in C#)

## 🎮 Game Description

TicTacTwo is an advanced version of Tic-Tac-Toe with a unique twist: the game is played on a configurable board (default 5×5), but only a smaller section (default 3×3) is active at any time. After a specific number of moves, players unlock special abilities to either place new markers, move existing ones, or shift the active grid. Each player has a maximum of 6 markers by default.

## Features

- **Modern Web Interface**: 
  - Clean, responsive Bootstrap-based UI
  - Color-coded player markers (X in red, O in blue)
  - Visual player status indicators showing turn and marker count
  - Mobile-friendly layout with responsive design
- **Multiple Game Modes**: 
  - Local Two Player mode
  - Single Player (vs Bot)
  - Online Two Player (password-protected)
  - Bots vs Bots mode for demonstration
- **Game Management**:
  - Create new games with different configurations
  - Save and resume games
  - Password protection for online games
  - Share game links with other players
  - Refresh button for online game synchronization
- **Game Configuration**:
  - Manage different game configurations
  - Create custom game settings
- **Visual Grid System**:
  - Dynamic grid rendering based on configuration
  - Visual indication of active grid area
  - Selected marker highlighting
  - Intuitive grid movement controls

## Architecture

The application follows the ASP.NET Core Razor Pages architecture:

- **Pages/**: Main Razor Pages folder
  - **Index.cshtml**: Home page with navigation cards
  - **Info.cshtml**: Game rules and information
  - **Play/**: Game playing related pages
    - **Setup.cshtml**: Game creation form
    - **Index.cshtml**: Main game board UI
    - **Result.cshtml**: Game outcome display
  - **Games/**: Game management pages
    - **Index.cshtml**: List of saved games
    - **Details.cshtml**: Game details view
    - **Edit.cshtml**: Edit game name
    - **Delete.cshtml**: Delete game confirmation
  - **Configurations/**: Configuration management pages
    - **Index.cshtml**: List of game configurations
    - **Create.cshtml**: Create new configuration
    - **Edit.cshtml**: Edit configuration
    - **Delete.cshtml**: Delete configuration confirmation

## 🚀 Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download) or later
- Any modern web browser (Chrome, Firefox, Safari, or Edge)

### Running the Application

1. Navigate to the WebApp directory:
   ```bash
   cd TicTacTwo/WebApp
   ```

2. Build and run the application:
   ```bash
   dotnet run
   ```

3. Open your browser to the displayed URL (typically https://localhost:5001)

## 🎯 How to Play

1. **Create a New Game**:
   - Click "Play" on the home page
   - Enter a game name
   - Select a configuration
   - Enter passwords if required
   - Click "Create & Play"

2. **During Gameplay**:
   - Initially, players take turns placing markers inside the highlighted grid
   - After enough markers are placed, special moves are unlocked
   - Special moves allow:
     - Moving the grid to an adjacent position by using the arrow buttons
     - Moving your markers by selecting your marker and then clicking on a destination
   - If playing against a bot, click "Ask Bot to make a move"
   - If playing online, share the game link with your opponent and use the refresh button

3. **Game Outcome**:
   - Win by getting markers in a row according to the win condition
   - Draw is possible if both players win simultaneously

## Data Storage

The application supports two types of data storage:

1. **Database**: Using SQLite via Entity Framework Core
2. **JSON Files**: Alternative storage method for simpler deployments

The storage type can be configured in appsettings.json:

```json
{
  "UseDatabase": true,
  "ConnectionStrings": {
    "DefaultConnection": "DataSource=<%location%>app.db;Cache=Shared"
  }
}
```

## Technologies Used

- ASP.NET Core 9.0
- Razor Pages
- Entity Framework Core
- Bootstrap 5
- SQLite Database

## 📝 License

All Rights Reserved

## Author

Name: Indrek Mäeots
School email: inmaeo@taltech.ee
