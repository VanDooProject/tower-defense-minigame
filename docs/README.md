# Tower Defense Minigame

A 3D turn-based cooperative tower defense game built with Godot 4 and C#.

## Features

- **Multiplayer**: 1-4 player cooperative gameplay via ENet networking
- **Turn-Based**: Strategic planning phase followed by action-packed combat phase
- **Procedural Maps**: Seed-synchronized map generation for consistent multiplayer experience
- **3 Tower Types**:
  - **Shooter Tower**: Fires projectiles at enemies
  - **Slower Tower**: Applies slow debuff to enemies in range
  - **Splash Tower**: Deals area damage with explosive projectiles
- **Player Ownership**: Color-coded ownership rings for each player's towers
- **Dynamic Difficulty**: Scales with player count
- **Medieval Theme**: Built entirely from Godot engine primitives

## Getting Started

### Prerequisites

- Godot 4.3+ with .NET support
- .NET 7.0 SDK or higher

### Building the Project

1. Clone the repository:
   ```bash
   git clone https://github.com/VanDooProject/tower-defense-minigame.git
   cd tower-defense-minigame
   ```

2. Open the project in Godot:
   ```bash
   godot game/project.godot
   ```

3. Build the C# solution:
   - In Godot Editor: Project → Tools → C# → Build Solution
   - Or from command line: `godot --build-solutions --quit`

### Running the Game

- From Godot Editor: Press F5 or click the Play button
- From command line: `godot game/project.godot`

## How to Play

1. **Main Menu**: Choose Solo, Host, or Join game
2. **Planning Phase**: 
   - Build towers by clicking on valid positions
   - Each player takes turns placing towers
   - Press SPACE to start the wave
3. **Combat Phase**:
   - Enemies spawn from gates and follow paths to the castle
   - Towers automatically target and attack enemies
   - Wave completes when all enemies are defeated
4. **Victory/Defeat**:
   - Win by surviving all 10 waves
   - Lose if castle health reaches zero

## Controls

- **Mouse**: Click to place towers (planning phase)
- **SPACE**: Start wave
- **ESC**: Return to main menu

## Project Structure

```
tower-defense-minigame/
├── game/                    # Godot project files
│   ├── scripts/            # C# scripts
│   ├── scenes/             # Godot scenes
│   ├── tests/              # GUT test files
│   └── project.godot       # Project configuration
├── docs/                    # Documentation
├── web/                     # Web export files
└── .github/workflows/      # CI/CD workflows
```

## Architecture

### Core Systems

- **NetworkManager**: Handles ENet multiplayer connections
- **GameStateManager**: Manages game phases and turn order
- **MapGenerator**: Creates procedural maps with seed synchronization
- **WaveManager**: Spawns and manages enemy waves

### Tower System

All towers inherit from base `Tower` class with:
- Player ownership tracking
- Color-coded visual rings
- Target acquisition
- Attack rate management

### Enemy System

- Pathfinding along generated routes
- Health and damage scaling
- Slow debuff support
- Castle damage on completion

## Testing

Run tests using GUT framework:

```bash
godot --headless -s addons/gut/gut_cmdln.gd -gtest=res://tests/
```

See [Testing Documentation](docs/testing.md) for details.

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Run tests to ensure quality
5. Submit a pull request

## License

This project is open source and available under the MIT License.

## Credits

Built with Godot Engine 4.3
Developed by VanDooProject
