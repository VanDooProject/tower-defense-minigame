# Quick Reference Guide

## Controls

| Action | Key/Input | Description |
|--------|-----------|-------------|
| Place Shooter Tower | `1` or Click Button | Select and place shooter tower (100g) |
| Place Slower Tower | `2` or Click Button | Select and place slower tower (150g) |
| Place Splash Tower | `3` or Click Button | Select and place splash tower (200g) |
| Confirm Placement | Left Click | Place selected tower at cursor position |
| Cancel Placement | Right Click or `ESC` | Cancel tower placement mode |
| Start Wave | `SPACE` or Button | Begin enemy wave (planning phase only) |
| Return to Menu | `ESC` (in menu) | Exit to main menu |

## Tower Types

### 🎯 Shooter Tower (100 Gold)
- **Type**: Single-target projectile
- **Range**: 10 units
- **Fire Rate**: 1 shot/second
- **Damage**: 10 per hit
- **Best for**: Consistent damage, general purpose

### ❄️ Slower Tower (150 Gold)
- **Type**: Area debuff
- **Range**: 10 units
- **Effect**: 50% speed reduction
- **Duration**: 3 seconds
- **Best for**: Supporting other towers, chokepoints

### 💥 Splash Tower (200 Gold)
- **Type**: Area damage projectile
- **Range**: 10 units
- **Fire Rate**: 0.3 shots/second
- **Damage**: 15 (area effect)
- **Splash Radius**: 5 units
- **Best for**: Groups of enemies, late waves

## Game Flow

```
Main Menu
    ↓
[Choose Mode: Solo/Host/Join]
    ↓
Planning Phase (Turn-Based)
    ↓
- Build towers
- Coordinate with teammates
- Press SPACE when ready
    ↓
Combat Phase (Real-Time)
    ↓
- Enemies spawn and path to castle
- Towers automatically attack
- Defend the castle!
    ↓
[Wave Complete or Castle Destroyed]
    ↓
Planning Phase (Next Wave)
    ↓
[Repeat until Wave 10 or Defeat]
    ↓
Victory or Defeat
```

## Economy

| Action | Gold Amount |
|--------|-------------|
| Starting Gold | 500 |
| Wave Completion Bonus | 200 |
| Enemy Kill Bounty | 50 |
| Shooter Tower Cost | 100 |
| Slower Tower Cost | 150 |
| Splash Tower Cost | 200 |

## Enemy Stats (Base)

| Wave | Health | Speed | Damage | Count (Solo) |
|------|--------|-------|--------|--------------|
| 1 | 120 | 3.1 | 12 | 7 |
| 2 | 140 | 3.2 | 14 | 9 |
| 3 | 160 | 3.3 | 16 | 11 |
| 5 | 200 | 3.5 | 20 | 15 |
| 10 | 300 | 4.0 | 30 | 25 |

*Multiplayer: Enemy count × player count*

## Strategy Tips

### Early Game (Waves 1-3)
- ✓ Focus on shooter towers for cost-efficiency
- ✓ Place towers along longest enemy paths
- ✓ Cover multiple lanes when possible
- ✗ Don't overbuild in one area

### Mid Game (Waves 4-7)
- ✓ Add slower towers at chokepoints
- ✓ Upgrade tower coverage
- ✓ Save gold for emergencies
- ✗ Don't neglect weak spots

### Late Game (Waves 8-10)
- ✓ Use splash towers for dense groups
- ✓ Layer different tower types
- ✓ Focus fire on dangerous paths
- ✗ Don't let resources sit unused

### Multiplayer
- ✓ Coordinate tower placement
- ✓ Specialize roles (builder/defender)
- ✓ Share information about threats
- ✓ Take turns efficiently

## Tower Placement Rules

1. **Minimum distance from castle**: 10 units
2. **Minimum distance between towers**: 3 units
3. **Valid during**: Planning phase only
4. **Requires**: Sufficient gold

## Win Conditions

**Victory**
- Survive all 10 waves
- Maintain castle health > 0

**Defeat**
- Castle health reaches 0
- Game ends immediately

## Multiplayer

### Player Colors
- Player 1: 🔴 Red
- Player 2: 🔵 Blue
- Player 3: 🟢 Green
- Player 4: 🟡 Yellow

### Connection
```bash
# Host
1. Click "Host Game"
2. Share your IP with friends
3. Wait for players to join

# Join
1. Click "Join Game"
2. Enter host's IP address
3. Click "Join"
```

### Default Port
- **Port**: 7777
- **Protocol**: ENet (UDP)
- **Max Players**: 4

## Console Commands

The game uses Godot's built-in console:

```
# Enable console
Editor → Project Settings → Debug → GDScript
# Set breakpoints in code for debugging
```

## File Locations

```
Tower Defense Minigame/
├── game/               # Open with Godot
├── docs/              # Documentation
├── web/               # Web build files
└── saves/             # Future: Save files
```

## Troubleshooting

| Issue | Solution |
|-------|----------|
| Can't place tower | Check gold, distance, and phase |
| Towers not firing | Enemies must be in range |
| Game stuttering | Reduce graphics settings |
| Can't connect | Check firewall and port 7777 |
| Missing GUT tests | Install from addons/ |

## Keyboard Shortcuts Summary

```
1, 2, 3    - Select tower type
SPACE      - Start wave
ESC        - Cancel/Menu
Left Click - Confirm action
Right Click- Cancel action
```

## Quick Stats

- **Map Size**: 50×50 units
- **Castle Health**: 100 HP
- **Max Waves**: 10
- **Max Players**: 4
- **Gate Count**: 4
- **Tower Types**: 3

## Credits

Built with Godot Engine 4.3  
Developed by VanDooProject  
Licensed under MIT

For detailed information, see:
- [Game Design Document](game-design.md)
- [API Documentation](api.md)
- [Setup Guide](setup.md)
