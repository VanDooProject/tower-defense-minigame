#!/bin/bash
# Setup verification script for Tower Defense Minigame

echo "🏰 Tower Defense Minigame - Setup Verification"
echo "=============================================="
echo ""

# Check Godot
echo -n "Checking Godot installation... "
if command -v godot &> /dev/null; then
    GODOT_VERSION=$(godot --version 2>&1 | head -n 1)
    echo "✓ Found: $GODOT_VERSION"
else
    echo "✗ Godot not found in PATH"
    echo "  Download from: https://godotengine.org/download"
    exit 1
fi

# Check .NET SDK
echo -n "Checking .NET SDK... "
if command -v dotnet &> /dev/null; then
    DOTNET_VERSION=$(dotnet --version)
    echo "✓ Found: $DOTNET_VERSION"
else
    echo "✗ .NET SDK not found"
    echo "  Download from: https://dotnet.microsoft.com/download"
    exit 1
fi

# Check Git
echo -n "Checking Git... "
if command -v git &> /dev/null; then
    GIT_VERSION=$(git --version)
    echo "✓ Found: $GIT_VERSION"
else
    echo "✗ Git not found"
    echo "  Download from: https://git-scm.com/"
    exit 1
fi

echo ""
echo "Checking project structure..."

# Check project file
if [ -f "game/project.godot" ]; then
    echo "✓ Project file found"
else
    echo "✗ Project file not found (game/project.godot)"
    exit 1
fi

# Check for scripts
SCRIPT_COUNT=$(find game/scripts -name "*.cs" 2>/dev/null | wc -l)
if [ $SCRIPT_COUNT -gt 0 ]; then
    echo "✓ Found $SCRIPT_COUNT C# scripts"
else
    echo "✗ No C# scripts found"
fi

# Check for scenes
SCENE_COUNT=$(find game/scenes -name "*.tscn" 2>/dev/null | wc -l)
if [ $SCENE_COUNT -gt 0 ]; then
    echo "✓ Found $SCENE_COUNT scene files"
else
    echo "✗ No scene files found"
fi

# Check for GUT
if [ -d "game/addons/gut" ]; then
    echo "✓ GUT testing framework installed"
else
    echo "⚠ GUT testing framework not found"
    echo "  Install from: https://github.com/bitwes/Gut"
fi

echo ""
echo "=============================================="
echo "Setup verification complete!"
echo ""
echo "Next steps:"
echo "1. Install GUT if not present: https://github.com/bitwes/Gut"
echo "2. Open project: godot game/project.godot"
echo "3. Run tests: godot --headless -s addons/gut/gut_cmdln.gd -gtest=res://tests/"
echo "4. Start developing! See docs/setup.md for details"
