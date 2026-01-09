# Tower Defense Minigame - Testing Guide

## Test Framework

This project uses the GUT (Godot Unit Test) framework for testing.

### Installing GUT

1. Download GUT from: https://github.com/bitwes/Gut
2. Extract to `game/addons/gut/`
3. Enable the plugin in Project Settings -> Plugins

### Running Tests

```bash
# From command line
godot --path game --headless -s addons/gut/gut_cmdln.gd -gtest=res://tests/

# From editor
# Go to the GUT panel at the bottom of the editor and click "Run All"
```

### Test Structure

```
tests/
  unit/           # Unit tests for individual classes
  integration/    # Integration tests for system interactions
  e2e/            # End-to-end gameplay tests
```

## Test Categories

### Unit Tests
- NetworkManager connection handling
- GameStateManager phase transitions
- MapGenerator seed consistency
- Tower targeting and firing
- Enemy pathfinding

### Integration Tests
- Multiplayer synchronization
- Wave spawning and completion
- Tower-Enemy interactions
- Damage and health systems

### E2E Tests
- Complete game flow from start to finish
- Multiplayer game sessions
- Victory and defeat conditions
