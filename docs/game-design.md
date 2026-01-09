# Tower Defense Minigame - Game Design Document

## Overview

A 3D turn-based cooperative tower defense game where 1-4 players defend a medieval castle from waves of enemies using three distinct tower types.

## Core Gameplay Loop

1. **Planning Phase** (Turn-Based)
   - Players take turns placing towers
   - Strategic positioning is key
   - Limited resources per turn
   - Players can pass to start combat

2. **Combat Phase** (Real-Time)
   - Enemies spawn from gates
   - Follow procedural paths to castle
   - Towers automatically engage
   - Wave ends when all enemies defeated

3. **Progression**
   - 10 waves total
   - Increasing difficulty
   - Enemy stats scale with wave number
   - Victory after wave 10

## Game Elements

### Castle Keep

- Central structure to defend
- 100 health points
- Takes damage from enemies that reach it
- Game over at 0 health

### Towers

#### Shooter Tower
- **Cost**: 100 gold
- **Range**: 10 units
- **Fire Rate**: 1 shot/second
- **Damage**: 10 per projectile
- **Description**: Basic single-target tower with good range

#### Slower Tower
- **Cost**: 150 gold
- **Range**: 10 units
- **Effect**: 50% movement speed reduction
- **Duration**: 3 seconds
- **Description**: Support tower that affects all enemies in range

#### Splash Tower
- **Cost**: 200 gold
- **Range**: 10 units
- **Fire Rate**: 0.3 shots/second
- **Damage**: 15 (area of effect)
- **Splash Radius**: 5 units
- **Description**: Area damage tower with falloff

### Enemies

- **Health**: 100 + (wave × 20)
- **Speed**: 3.0 + (wave × 0.1)
- **Damage**: 10 + (wave × 2)
- **Bounty**: 50 gold
- **Count**: 5 + (wave × 2) × player_count

### Map Generation

- **Size**: 50×50 unit grid
- **Gates**: 4 spawn points around perimeter
- **Paths**: 2-4 waypoints per path
- **Seed**: Synchronized across multiplayer
- **Terrain**: Flat with ground plane

## Multiplayer

### Network Architecture

- **Protocol**: ENet (UDP-based)
- **Port**: 7777 (default)
- **Max Players**: 4
- **Host**: Server + Client
- **Synchronization**: Seed-based map generation

### Turn Order

1. Player 0 (host) goes first
2. Clockwise rotation through players
3. Any player can vote to start wave
4. Combat phase is simultaneous

### Player Colors

- Player 0: Red
- Player 1: Blue
- Player 2: Green
- Player 3: Yellow

## Difficulty Scaling

### Formula
```
enemy_count = base_count × player_count
enemy_health = base_health + (wave × 20)
enemy_speed = base_speed + (wave × 0.1)
enemy_damage = base_damage + (wave × 2)
```

### Rationale

More players = more towers = more enemies needed for balance.
Wave progression provides steady difficulty curve.

## Visual Design

### Art Style

- Low-poly medieval theme
- Primitive-based geometry (CSG)
- Player-colored tower rings
- Clear visual feedback

### Materials

- Stone gray for castle
- Brown wood for gates
- Green grass terrain
- Colored emissions for towers
- Glowing projectiles

### Camera

- Fixed isometric view
- 40 units above ground
- 30° angle
- Centered on castle

## User Interface

### Main Menu
- Play Solo
- Host Game
- Join Game (with IP input)

### HUD (In-Game)
- Castle health bar
- Current wave number
- Player turn indicator
- Gold/resources
- Build menu

## Technical Requirements

### Performance Targets

- 60 FPS minimum
- < 100ms network latency acceptable
- Support 100+ active units

### Platform Support

- Windows (primary)
- Linux
- Web (HTML5 export)

## Future Enhancements

- More tower types
- Enemy variety
- Upgrade system
- Map themes
- Achievements
- Leaderboards
