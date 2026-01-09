using Godot;
using TowerDefense.Core;
using TowerDefense.Map;
using TowerDefense.Enemies;
using TowerDefense.Towers;
using TowerDefense.Player;
using TowerDefense.UI;

namespace TowerDefense
{
    /// <summary>
    /// Main game controller that coordinates all systems
    /// </summary>
    public partial class GameController : Node
    {
        private NetworkManager _networkManager;
        private GameStateManager _gameStateManager;
        private MapGenerator _mapGenerator;
        private WaveManager _waveManager;
        private PlayerManager _playerManager;
        private TowerPlacer _towerPlacer;
        private GameHUD _gameHUD;
        private Camera3D _camera;
        private int _castleHealth = 100;
        
        [Export]
        public ulong MapSeed { get; set; } = 12345;
        
        [Signal]
        public delegate void CastleHealthChangedEventHandler(int health);
        
        public override void _Ready()
        {
            SetupNodes();
            SetupCamera();
            SetupSignals();
            
            // Initialize game
            InitializeGame();
        }
        
        private void SetupNodes()
        {
            _networkManager = GetNode<NetworkManager>("NetworkManager");
            _gameStateManager = GetNode<GameStateManager>("GameStateManager");
            _mapGenerator = GetNode<MapGenerator>("MapGenerator");
            _waveManager = GetNode<WaveManager>("WaveManager");
            _playerManager = GetNode<PlayerManager>("PlayerManager");
            _towerPlacer = GetNode<TowerPlacer>("TowerPlacer");
            _gameHUD = GetNode<GameHUD>("GameHUD");
        }
        
        private void SetupCamera()
        {
            _camera = GetNode<Camera3D>("Camera3D");
            
            // Position camera for top-down view
            _camera.Position = new Vector3(0, 40, 30);
            _camera.LookAt(Vector3.Zero, Vector3.Up);
        }
        
        private void SetupSignals()
        {
            _gameStateManager.WaveStarted += OnWaveStarted;
            _gameStateManager.WaveCompleted += OnWaveCompleted;
            _gameStateManager.GameOver += OnGameOver;
            
            _waveManager.WaveCompleted += OnWaveSystemCompleted;
            _waveManager.EnemyReachedCastle += OnEnemyReachedCastle;
            
            _networkManager.PlayerConnected += OnPlayerConnected;
            _networkManager.PlayerDisconnected += OnPlayerDisconnected;
            
            CastleHealthChanged += _gameHUD.UpdateHealth;
        }
        
        private void InitializeGame()
        {
            // Generate map
            _mapGenerator.GenerateMap(MapSeed);
            
            // Initialize player
            _playerManager.Initialize(0);
            
            // Start game for single player (can be extended for multiplayer)
            _gameStateManager.InitializeGame(1);
        }
        
        public void StartGame(int playerCount)
        {
            _castleHealth = 100;
            _playerManager.Initialize(0);
            _gameStateManager.InitializeGame(playerCount);
            EmitSignal(SignalName.CastleHealthChanged, _castleHealth);
        }
        
        private void OnWaveStarted(int waveNumber)
        {
            GD.Print($"Game: Wave {waveNumber} started");
            int difficulty = _gameStateManager.GetDifficultyMultiplier();
            _waveManager.StartWave(waveNumber, difficulty);
        }
        
        private void OnWaveCompleted(int waveNumber)
        {
            GD.Print($"Game: Wave {waveNumber} completed");
            _playerManager.OnWaveCompleted();
        }
        
        private void OnWaveSystemCompleted()
        {
            _gameStateManager.CompleteWave();
        }
        
        private void OnGameOver(bool victory)
        {
            GD.Print($"Game Over: {(victory ? "Victory!" : "Defeat!")}");
        }
        
        private void OnEnemyReachedCastle(int damage)
        {
            _castleHealth -= damage;
            EmitSignal(SignalName.CastleHealthChanged, _castleHealth);
            
            GD.Print($"Castle health: {_castleHealth}");
            
            if (_castleHealth <= 0)
            {
                _gameStateManager.EndGame(false);
            }
        }
        
        private void OnPlayerConnected(long peerId, int playerIndex)
        {
            GD.Print($"Player {playerIndex} connected");
        }
        
        private void OnPlayerDisconnected(long peerId)
        {
            GD.Print($"Player disconnected");
        }
        
        public override void _Input(InputEvent @event)
        {
            // Quick start wave for testing
            if (@event is InputEventKey keyEvent && keyEvent.Pressed)
            {
                if (keyEvent.Keycode == Key.Space)
                {
                    if (_gameStateManager.CurrentPhase == GameStateManager.GamePhase.Planning)
                    {
                        _gameStateManager.StartWave();
                    }
                }
            }
        }
    }
}
