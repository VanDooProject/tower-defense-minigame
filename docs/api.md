# API Documentation

## Core Systems

### NetworkManager

Manages multiplayer connections using ENet.

#### Methods

```csharp
bool CreateServer(int port = 7777)
```
Creates a server instance.
- **Returns**: `true` if successful
- **Emits**: `ServerStarted` signal

```csharp
bool JoinServer(string address, int port = 7777)
```
Connects to a server.
- **Returns**: `true` if connection initiated
- **Emits**: `ConnectionSucceeded` or `ConnectionFailed` signal

```csharp
void DisconnectFromServer()
```
Closes current connection.

#### Signals

- `PlayerConnected(long peerId, int playerIndex)`: Fired when a player joins
- `PlayerDisconnected(long peerId)`: Fired when a player leaves
- `ServerStarted()`: Fired when server starts successfully
- `ConnectionSucceeded()`: Fired when client connects
- `ConnectionFailed()`: Fired when connection fails

#### Properties

- `int PlayerCount`: Current number of connected players

---

### GameStateManager

Manages game phases and turn order.

#### Methods

```csharp
void InitializeGame(int playerCount)
```
Starts a new game session.

```csharp
void StartWave()
```
Transitions from Planning to Combat phase.

```csharp
void CompleteWave()
```
Ends current wave and returns to Planning.

```csharp
void NextTurn()
```
Advances to next player's turn.

```csharp
void EndGame(bool victory)
```
Ends the game with victory or defeat.

```csharp
int GetDifficultyMultiplier()
```
Returns difficulty scaling factor based on player count.

#### Signals

- `GamePhaseChanged(GamePhase newPhase)`: Phase transition
- `TurnChanged(int playerIndex)`: Turn changed
- `WaveStarted(int waveNumber)`: Wave begun
- `WaveCompleted(int waveNumber)`: Wave finished
- `GameOver(bool victory)`: Game ended

#### Properties

- `GamePhase CurrentPhase`: Current game phase
- `int CurrentWave`: Current wave number
- `int CurrentPlayerTurn`: Active player index
- `int PlayerCount`: Total players

#### Enums

```csharp
enum GamePhase {
    Lobby,      // Pre-game
    Planning,   // Turn-based tower placement
    Combat,     // Real-time wave
    Victory,    // Game won
    Defeat      // Game lost
}
```

---

### MapGenerator

Generates procedural maps with seed synchronization.

#### Methods

```csharp
void GenerateMap(ulong seed)
```
Creates a new map from seed.
- **Emits**: `MapGenerated` signal

#### Signals

- `MapGenerated()`: Map generation complete

#### Properties

- `int MapSize`: Map dimensions (default: 50)
- `int NumGates`: Number of spawn gates (default: 4)
- `ulong MapSeed`: Current seed value
- `Vector3 CastlePosition`: Castle location
- `List<Vector3> GatePositions`: Spawn gate positions
- `List<List<Vector3>> Paths`: Enemy paths from gates to castle

---

### WaveManager

Spawns and manages enemy waves.

#### Methods

```csharp
void StartWave(int waveNumber, int difficultyMultiplier = 1)
```
Begins spawning a wave.

#### Signals

- `WaveCompleted()`: All enemies defeated
- `EnemyReachedCastle(int damage)`: Enemy reached castle

#### Properties

- `PackedScene EnemyScene`: Enemy prefab to spawn
- `float SpawnInterval`: Time between spawns (default: 1.0)
- `int BaseEnemiesPerWave`: Base enemy count (default: 5)
- `int EnemiesAlive`: Current living enemy count

---

## Tower System

### Tower (Base Class)

Base class for all towers.

#### Methods

```csharp
void SetPlayer(int playerIndex)
```
Assigns tower to a player (updates color).

#### Signals

- `TowerFired(Node3D target)`: Tower fired at target

#### Properties

- `float Range`: Attack range
- `float FireRate`: Attacks per second
- `int Cost`: Build cost
- `int PlayerIndex`: Owning player

### ShooterTower

Single-target projectile tower.

#### Properties

- `float ProjectileSpeed`: Projectile velocity (default: 20.0)
- `int Damage`: Damage per hit (default: 10)

### SlowerTower

Area debuff tower.

#### Properties

- `float SlowAmount`: Speed reduction (default: 0.5 = 50%)
- `float SlowDuration`: Debuff duration (default: 3.0)

### SplashTower

Area damage tower.

#### Properties

- `float SplashRadius`: Explosion radius (default: 5.0)
- `int Damage`: Base damage (default: 15)

---

## Enemy System

### Enemy

Base enemy class.

#### Methods

```csharp
void SetPath(List<Vector3> path)
```
Assigns movement path.

```csharp
void TakeDamage(int damage)
```
Reduces health, triggers death if <= 0.

```csharp
void ApplySlow(float amount, float duration)
```
Applies movement speed debuff.

#### Signals

- `EnemyDied(Enemy enemy)`: Enemy defeated
- `ReachedCastle(Enemy enemy)`: Enemy reached goal

#### Properties

- `int MaxHealth`: Maximum health
- `float MoveSpeed`: Movement speed
- `int Damage`: Damage to castle
- `int Bounty`: Gold reward on death

---

## Events & Flow

### Game Start

1. `NetworkManager.CreateServer()` or `JoinServer()`
2. `MapGenerator.GenerateMap(seed)`
3. `GameStateManager.InitializeGame(playerCount)`
4. Phase → Planning

### Wave Cycle

1. Player places towers (turn-based)
2. Player triggers `GameStateManager.StartWave()`
3. Phase → Combat
4. `WaveManager.StartWave()`
5. Enemies spawn and path
6. When all enemies dead: `WaveManager.WaveCompleted`
7. `GameStateManager.CompleteWave()`
8. Phase → Planning (or Victory/Defeat)

### Multiplayer Sync

- Map seed synchronized at game start
- Tower placement via RPC calls (to be implemented)
- Enemy spawning controlled by host
- Damage/health synced via RPC (to be implemented)
