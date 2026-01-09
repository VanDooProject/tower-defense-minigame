# Contributing to Tower Defense Minigame

Thank you for your interest in contributing! This document provides guidelines for contributing to the project.

## Code of Conduct

- Be respectful and inclusive
- Focus on constructive feedback
- Help others learn and grow
- Keep discussions relevant and professional

## Getting Started

1. Fork the repository
2. Clone your fork locally
3. Follow the [Setup Guide](docs/setup.md)
4. Create a feature branch
5. Make your changes
6. Submit a pull request

## Development Guidelines

### Code Style

#### C# Code
- Use 4 spaces for indentation
- Follow Microsoft C# naming conventions
- Use meaningful variable and method names
- Add XML documentation comments for public APIs
- Keep methods focused and under 50 lines when possible

Example:
```csharp
/// <summary>
/// Spawns an enemy at the specified gate
/// </summary>
/// <param name="gateIndex">Index of the gate to spawn from</param>
/// <returns>The spawned enemy instance</returns>
public Enemy SpawnEnemy(int gateIndex)
{
    // Implementation
}
```

#### GDScript Tests
- Use tabs for indentation
- Follow Godot's GDScript style guide
- Name test functions with `test_` prefix
- Keep tests focused on single functionality

Example:
```gdscript
func test_tower_placement():
	var tower = Tower.new()
	add_child(tower)
	assert_not_null(tower)
```

### Git Commit Messages

Follow the conventional commits specification:

- `feat:` New feature
- `fix:` Bug fix
- `docs:` Documentation changes
- `test:` Test additions/changes
- `refactor:` Code refactoring
- `style:` Code style changes
- `chore:` Build/tooling changes

Examples:
```
feat: add splash damage tower
fix: correct enemy pathfinding on steep slopes
docs: update API documentation for WaveManager
test: add unit tests for MapGenerator
```

### Testing Requirements

All code changes should include appropriate tests:

#### Unit Tests
- Test individual components in isolation
- Mock dependencies when needed
- Cover edge cases and error conditions

#### Integration Tests
- Test component interactions
- Verify system behavior
- Test multiplayer synchronization

#### E2E Tests
- Test complete game flows
- Verify user scenarios
- Test win/loss conditions

Run tests before submitting:
```bash
godot --headless -s addons/gut/gut_cmdln.gd -gtest=res://tests/
```

### Pull Request Process

1. **Create a descriptive PR title**
   - Use conventional commit format
   - Be specific about changes

2. **Provide detailed description**
   - What problem does this solve?
   - How does it work?
   - Any breaking changes?
   - Screenshots for UI changes

3. **Ensure CI passes**
   - All tests pass
   - Code formatting is correct
   - No new warnings

4. **Request review**
   - Tag relevant maintainers
   - Respond to feedback promptly
   - Make requested changes

5. **Keep PR focused**
   - One feature/fix per PR
   - Avoid unrelated changes
   - Keep commits atomic

### Areas for Contribution

#### Features
- Additional tower types
- New enemy varieties
- Power-ups and abilities
- Map themes and biomes
- Sound effects and music
- Particle effects
- Achievement system
- Player statistics

#### Improvements
- Performance optimizations
- UI/UX enhancements
- Code refactoring
- Test coverage
- Documentation
- Accessibility features

#### Bug Fixes
- Check the [Issues](https://github.com/VanDooProject/tower-defense-minigame/issues) page
- Look for `good first issue` labels
- Reproduce the bug
- Write a failing test
- Fix the issue
- Verify the test passes

### Documentation

When adding features, update:
- API documentation (`docs/api.md`)
- Game design document (`docs/game-design.md`)
- README if user-facing changes
- Code comments for complex logic

### Code Review

Reviews focus on:
- **Correctness**: Does it work as intended?
- **Testing**: Are there adequate tests?
- **Readability**: Is the code clear?
- **Performance**: Are there efficiency concerns?
- **Security**: Are there vulnerabilities?
- **Design**: Does it fit the architecture?

### Performance Considerations

- Profile before optimizing
- Avoid premature optimization
- Document performance-critical sections
- Test with realistic scenarios (100+ enemies)
- Consider multiplayer implications

### Security Guidelines

- Never commit secrets or API keys
- Validate all user input
- Use parameterized queries for data
- Follow OWASP guidelines
- Report security issues privately

### Multiplayer Considerations

When working on multiplayer features:
- Test with 2-4 players
- Ensure deterministic behavior
- Synchronize critical state
- Handle disconnections gracefully
- Consider network latency

### Asset Guidelines

If contributing 3D assets:
- Use Godot primitives (CSG nodes)
- Keep polygon count reasonable
- Use appropriate materials
- Include LOD if needed
- Document in commit message

### Questions?

- Check existing [Issues](https://github.com/VanDooProject/tower-defense-minigame/issues)
- Review [Documentation](docs/)
- Ask in pull request discussions
- Be patient and respectful

## Recognition

Contributors will be:
- Listed in project credits
- Mentioned in release notes
- Acknowledged in documentation

Thank you for contributing! 🎮🏰
