# Tower Defense Minigame

A 3D turn-based cooperative tower defense game built with Godot 4 and C#.

![Build Status](https://github.com/VanDooProject/tower-defense-minigame/workflows/CI%20Tests/badge.svg)
![License](https://img.shields.io/badge/license-MIT-blue.svg)

## 🎮 Features

- **Multiplayer**: 1-4 player cooperative gameplay via ENet networking
- **Turn-Based Strategy**: Plan together during turn-based tower placement
- **Real-Time Combat**: Watch towers defend against enemy waves
- **Procedural Maps**: Seed-synchronized generation for consistent multiplayer
- **3 Tower Types**: Shooter, Slower, and Splash towers with unique abilities
- **Player Ownership**: Color-coded ownership rings (Red, Blue, Green, Yellow)
- **Difficulty Scaling**: Automatically adjusts based on player count
- **Medieval Theme**: Built entirely from Godot engine primitives

## 🚀 Quick Start

### Prerequisites
- Godot 4.3+ with .NET support
- .NET 7.0 SDK or higher

### Installation
```bash
git clone https://github.com/VanDooProject/tower-defense-minigame.git
cd tower-defense-minigame
godot game/project.godot
```

### Building
```bash
cd game
godot --headless --build-solutions --quit
```

### Running Tests
```bash
godot --headless -s addons/gut/gut_cmdln.gd -gtest=res://tests/
```

## 📖 Documentation

- [Game Design Document](docs/game-design.md)
- [API Reference](docs/api.md)
- [Testing Guide](docs/testing.md)
- [Full Documentation](docs/README.md)

## 🏗️ Project Structure

```
tower-defense-minigame/
├── game/                    # Godot project
│   ├── scripts/            # C# game scripts
│   ├── scenes/             # Godot scene files
│   ├── tests/              # GUT test suite
│   └── project.godot       # Project config
├── docs/                    # Documentation
├── web/                     # Web export assets
└── .github/workflows/      # CI/CD pipelines
```

## 🎯 How to Play

1. **Main Menu**: Choose Solo, Host, or Join game
2. **Planning Phase**: Take turns placing towers (SPACE to start wave)
3. **Combat Phase**: Defend against enemy waves
4. **Victory**: Survive all 10 waves!

## 🛠️ Development

### Running CI Locally
```bash
# Run tests
godot --headless -s addons/gut/gut_cmdln.gd -gtest=res://tests/

# Format code
dotnet format
```

### Building Exports
```bash
# Windows
godot --headless --export-release "Windows Desktop" builds/windows/game.exe

# Linux
godot --headless --export-release "Linux/X11" builds/linux/game.x86_64

# Web
godot --headless --export-release "Web" builds/web/index.html
```

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📄 License

This project is licensed under the MIT License.

## 🙏 Credits

Built with [Godot Engine 4.3](https://godotengine.org/)  
Developed by [VanDooProject](https://github.com/VanDooProject)
