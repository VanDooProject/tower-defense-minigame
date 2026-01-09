# Development Setup Guide

## Prerequisites

### Required Software

1. **Godot Engine 4.3+**
   - Download from: https://godotengine.org/download
   - Choose the ".NET" version (not the standard version)
   - Ensure C# support is enabled

2. **.NET SDK 7.0 or higher**
   - Download from: https://dotnet.microsoft.com/download
   - Verify installation: `dotnet --version`

3. **Git**
   - Download from: https://git-scm.com/

### Optional Tools

- **Visual Studio Code** with C# extension
- **Visual Studio 2022** with .NET desktop development
- **Rider** (JetBrains IDE)

## Initial Setup

### 1. Clone the Repository

```bash
git clone https://github.com/VanDooProject/tower-defense-minigame.git
cd tower-defense-minigame
```

### 2. Open in Godot

```bash
# From command line
godot game/project.godot

# Or use Godot Project Manager
# Click "Import" and select game/project.godot
```

### 3. Build the C# Project

First time opening the project in Godot:

1. Wait for Godot to finish importing assets (can take a few minutes)
2. Go to **Project → Tools → C# → Create C# Solution**
3. Go to **Project → Tools → C# → Build Solution**

From command line:
```bash
cd game
godot --headless --build-solutions --quit
```

### 4. Install GUT Testing Framework

The GUT (Godot Unit Test) framework needs to be installed manually:

1. Download GUT from: https://github.com/bitwes/Gut/releases
2. Extract the `addons/gut` folder to `game/addons/gut`
3. In Godot: **Project → Project Settings → Plugins**
4. Enable the "Gut" plugin

Or install via command line (if gut_install tool is available):
```bash
cd game
# Follow GUT installation instructions from their repository
```

## Running the Game

### From Godot Editor

1. Open `game/project.godot` in Godot
2. Press **F5** or click the "Play" button
3. The main menu should appear

### From Command Line

```bash
cd game
godot project.godot
```

## Running Tests

### Unit Tests

```bash
cd game
godot --headless -s addons/gut/gut_cmdln.gd -gtest=res://tests/unit/
```

### Integration Tests

```bash
cd game
godot --headless -s addons/gut/gut_cmdln.gd -gtest=res://tests/integration/
```

### All Tests

```bash
cd game
godot --headless -s addons/gut/gut_cmdln.gd -gtest=res://tests/
```

### From Godot Editor

1. Open the game in Godot
2. Click the "Gut" tab at the bottom
3. Click "Run All" to run all tests
4. View results in the panel

## Building Exports

### Windows Build

```bash
cd game
mkdir -p exports/windows
godot --headless --export-release "Windows Desktop" exports/windows/TowerDefense.exe
```

### Linux Build

```bash
cd game
mkdir -p exports/linux
godot --headless --export-release "Linux/X11" exports/linux/TowerDefense.x86_64
```

### Web Build

```bash
cd game
mkdir -p exports/web
godot --headless --export-release "Web" exports/web/index.html
```

**Note**: You may need to install export templates first:
- In Godot: **Editor → Manage Export Templates → Download and Install**

## Development Workflow

### Code Style

The project uses:
- 4 spaces for C# indentation
- Tabs for GDScript indentation
- See `.editorconfig` for full details

Format C# code:
```bash
dotnet format
```

### Git Workflow

1. Create a feature branch:
   ```bash
   git checkout -b feature/my-feature
   ```

2. Make your changes and commit:
   ```bash
   git add .
   git commit -m "Description of changes"
   ```

3. Push and create a pull request:
   ```bash
   git push origin feature/my-feature
   ```

### CI/CD

The project uses GitHub Actions for:
- Automated testing on every push
- Code formatting checks
- Build generation for releases

Check `.github/workflows/` for workflow configurations.

## Troubleshooting

### "C# project not found" error

**Solution**: Run build solution:
```bash
cd game
godot --headless --build-solutions --quit
```

### Import errors in C# scripts

**Solution**: 
1. Delete the `.mono` folder in the game directory
2. Rebuild the solution
3. Restart Godot

### GUT tests not running

**Solution**:
1. Ensure GUT plugin is installed in `game/addons/gut`
2. Enable the plugin in Project Settings
3. Verify test files have `.gd` extension

### Export templates missing

**Solution**:
- Download from: **Editor → Manage Export Templates → Download and Install**
- Or manually from: https://godotengine.org/download

### Performance issues in editor

**Solution**:
1. Reduce viewport quality: **Project Settings → Rendering**
2. Disable shadows in development
3. Use a less complex map for testing

## Project Structure

```
tower-defense-minigame/
├── game/                       # Main Godot project
│   ├── addons/                # Third-party plugins (GUT)
│   ├── assets/                # Game assets
│   ├── materials/             # Materials
│   ├── scenes/                # Scene files (.tscn)
│   ├── scripts/               # C# scripts (.cs)
│   ├── tests/                 # Test files (.gd)
│   ├── project.godot          # Project configuration
│   └── export_presets.cfg     # Export settings
├── docs/                       # Documentation
├── web/                        # Web assets
├── .github/workflows/         # CI/CD workflows
├── .gitignore                 # Git ignore rules
├── .editorconfig             # Editor configuration
└── README.md                  # Project readme
```

## Next Steps

After setup:
1. Read the [Game Design Document](game-design.md)
2. Review the [API Documentation](api.md)
3. Try running the game and placing towers
4. Run the test suite
5. Start contributing!

## Getting Help

- **Issues**: https://github.com/VanDooProject/tower-defense-minigame/issues
- **Godot Docs**: https://docs.godotengine.org/en/stable/
- **GUT Docs**: https://github.com/bitwes/Gut/wiki
