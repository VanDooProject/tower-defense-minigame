# Changelog

All notable changes to the Tower Defense Minigame project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added (Initial Implementation)

#### Core Systems
- **Multiplayer Infrastructure**: ENet-based networking for 1-4 players
- **Game State Management**: Turn-based planning and real-time combat phases
- **Procedural Map Generation**: Seed-synchronized maps with spawn gates and paths
- **Wave System**: Difficulty-scaling enemy waves with spawn management
- **Player Management**: Resource (gold) system with player ownership tracking

#### Gameplay Features
- **3 Tower Types**:
  - Shooter Tower: Single-target projectile attacks
  - Slower Tower: Area debuff reducing enemy speed
  - Splash Tower: Area damage with explosive projectiles
- **Player Ownership**: Color-coded ownership rings (Red, Blue, Green, Yellow)
- **Tower Placement**: Mouse-based placement with validation
- **Enemy System**: Pathfinding enemies with health, speed, and damage scaling
- **Castle Defense**: Castle health system with damage tracking

#### Visual & UI
- **3D Medieval Theme**: Primitive-based assets (castle, towers, enemies)
- **Fixed Camera**: Isometric top-down view
- **In-Game HUD**: Displays gold, wave, phase, and castle health
- **Main Menu**: Solo, Host, and Join game options
- **Visual Feedback**: Health bars, player colors, tower previews

#### Technical
- **C# Scripting**: Full game logic in C# for Godot 4.3
- **Scene System**: Modular scene files for game elements
- **Export Support**: Windows, Linux, and Web builds
- **CI/CD Pipelines**: Automated testing and build workflows

#### Testing
- **Unit Tests**: Core system testing with GUT framework
- **Integration Tests**: System interaction testing
- **E2E Tests**: Complete game flow testing
- **Test Coverage**: GameStateManager, MapGenerator, game flow

#### Documentation
- **README**: Project overview and quick start guide
- **Game Design Document**: Complete design specifications
- **API Documentation**: Comprehensive API reference
- **Setup Guide**: Development environment setup instructions
- **Quick Reference**: Player controls and strategy guide
- **Contributing Guide**: Contribution guidelines and standards
- **LICENSE**: MIT License

#### Developer Tools
- **Setup Verification Script**: Automated setup checking
- **EditorConfig**: Consistent code formatting rules
- **Git Attributes**: Line ending normalization
- **GitHub Actions**: CI for testing, CD for builds

### Project Structure
```
tower-defense-minigame/
├── game/
│   ├── scripts/         # 15 C# game scripts
│   ├── scenes/          # 7 scene files
│   ├── tests/           # Unit, integration, and e2e tests
│   ├── assets/          # Future asset directory
│   ├── materials/       # Future material directory
│   └── project.godot    # Godot project configuration
├── docs/                # 6 documentation files
├── web/                 # Web export HTML template
├── .github/workflows/   # 2 CI/CD workflows
└── scripts/            # Setup verification script
```

### Technical Details

#### Dependencies
- Godot Engine 4.3+ with .NET support
- .NET SDK 7.0+
- GUT testing framework (manual install)

#### Architecture
- **Network**: ENet UDP on port 7777
- **Physics**: 3D physics with collision layers
- **Rendering**: Forward+ renderer with MSAA
- **Max Performance**: Designed for 100+ concurrent units

#### Code Statistics
- **C# Scripts**: ~15 files, ~500+ lines each
- **Test Files**: 4 GDScript test files
- **Documentation**: ~25,000 words
- **Scene Files**: 7 .tscn files
- **Total Commits**: 2 major implementation commits

## [0.1.0] - 2026-01-09

### Initial Release
- Project initialization
- Core game systems implementation
- Basic gameplay loop functional
- Documentation suite complete
- CI/CD workflows configured

---

## Development Roadmap

### Planned for v0.2.0
- [ ] GUT plugin bundled in repository
- [ ] RPC multiplayer synchronization
- [ ] Additional tower types
- [ ] More enemy varieties
- [ ] Sound effects and music

### Planned for v0.3.0
- [ ] Save/load system
- [ ] Achievement system
- [ ] Player statistics tracking
- [ ] Enhanced visual effects
- [ ] Tutorial system

### Planned for v1.0.0
- [ ] Complete multiplayer testing
- [ ] Performance optimizations
- [ ] Full documentation
- [ ] Stable release
- [ ] Steam/Itch.io distribution

---

## Contributing

See [CONTRIBUTING.md](../CONTRIBUTING.md) for how to contribute to this project.

## License

This project is licensed under the MIT License - see the [LICENSE](../LICENSE) file for details.
