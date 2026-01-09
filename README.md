# Tower Defense Minigame

A 3D turn-based cooperative tower defense game built with Godot 4 and C#.

![Build Status](https://github.com/VanDooProject/tower-defense-minigame/workflows/CI%20Tests/badge.svg)


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

---

# tower-defense-minigame


# Castle Defenders: Cooperative Tower Defense
A godot based turn-based cooperative tower defense minigame where 1-4 players work together to defend their castle keep against relentless waves of enemies emerging from mystical spawn gates.

## 🏰 Game Setting

### The World

In the aftermath of a great magical cataclysm, the ancient castle keeps of the realm stand as the last bastions of civilization. Dark forces have discovered how to open **Siege Gates** - corrupted portals that spawn endless waves of twisted creatures seeking to destroy the remaining strongholds.

You and your fellow **Keep Defenders** must work together, strategically placing defensive towers across the castle courtyard to repel the invaders before they can breach the central keep.

### The Castle Keep

Each battle takes place in a **procedurally generated castle courtyard** surrounding your keep - a massive stone tower that serves as the heart of your defense. The courtyard features:

- **Cobblestone Pathways**: Ancient roads that enemies will follow toward the keep
- **Defensive Positions**: Grassy and earthen areas where towers can be constructed
- **Siege Gates**: Dark portals at the edges of the courtyard that spawn enemy forces
- **Runic Stones**: Scattered remnants of old magic that provide resources for tower construction

The layout changes with each battle, requiring defenders to adapt their strategies to new terrain configurations.

### Your Arsenal

As Keep Defenders, you command three types of defensive structures, each enhanced with **runes and gemstones** that grant them power:

#### **Archer Towers** (Shooter)
Wooden siege platforms topped with sharpshooters. These towers excel at picking off single targets with precise volleys. Enhanced with **Ruby Gems** that glow red, marking them as instruments of focused destruction.

#### **Frost Spires** (Slower)
Stone obelisks embedded with **Sapphire Runes** that channel ice magic across an area. Enemies caught in their frigid aura move sluggishly, buying precious time for your other defenses.

#### **Catapult Emplacements** (Splash)
Siege engines that hurl alchemical explosives into clusters of enemies. Powered by **Emerald Gemstones**, they deal devastating area damage but require careful positioning to avoid wasting their powerful payloads.

### Runes and Gemstones

The ancient magic that once protected these keeps has been shattered into fragments:

- **Runes**: Mystical inscriptions that appear as glowing patterns around tower bases, marking ownership and channeling power
- **Gemstones**: Crystalline focuses embedded in each tower type, determining their nature (Ruby/Sapphire/Emerald)
- **Resource Crystals**: Shared magical energy that all defenders draw from to construct new towers

Each defender's towers are marked with unique **colored runes** - mystical rings and ribbons that pulse with their signature hue (blue, red, green, or yellow), allowing everyone to see who placed which defenses.

### The Enemy

The creatures pouring through the Siege Gates come in many forms:

#### **Corrupted Scouts** (Basic)
Small, swift creatures that rush toward the keep in large numbers. Individually weak but overwhelming in swarms.

#### **Stone Behemoths** (Tank)
Massive constructs of dark magic and stone. Slow but nearly unstoppable, requiring concentrated fire to bring down before they reach your walls.

As waves progress, the forces grow stronger, more numerous, and more coordinated. Only perfect cooperation between defenders can hope to withstand the onslaught.

### The Challenge

Each battle consists of **10 waves** of increasing difficulty. Between waves, defenders take turns simultaneously placing towers, discussing strategy, and preparing for the next assault. The difficulty scales based on how many defenders stand together - more players mean tougher enemies but also more tactical possibilities.

When all defenders signal they're ready, the wave begins. Watch as your carefully placed defenses spring to life, runes glowing, gemstones pulsing, and siege weapons unleashing their fury upon the advancing horde.

Victory belongs to those who can balance individual tower placement with team coordination. Defeat comes to those who let the darkness breach the keep.

### Cooperative Philosophy

This is not a competition - it's a test of teamwork. All defenders share:
- A common resource pool
- The same objective (protect the keep)
- The triumph of victory or the weight of defeat

Communication and coordination are your greatest weapons. Will you cluster your towers to create killing zones? Spread them to cover all approaches? Mix tower types for synergistic effects? The choice is yours, but you must choose together.

## 🎮 Game Modes

both modes should be from the same starting screen, the only difference is that solo mode does not require other players in the party, the user should not feeld a difference

### Solo Defense
Stand alone as a single defender against scaled waves. Perfect for honing your strategy and learning enemy patterns.

### Cooperative Defense (2-4 Players)
Join forces with other defenders. The enemy grows stronger with each additional player, but so does your strategic depth and tower placement potential.

## 🎯 Victory Conditions

- **Victory**: Survive all 10 waves with the keep intact
- **Defeat**: The keep's health reaches zero

The fate of the castle is in your hands, Defender. Choose your difficulty, gather your allies, and let the defense begin.

---

*"In the glow of ancient runes and the gleam of mystic gemstones, we make our stand."*
