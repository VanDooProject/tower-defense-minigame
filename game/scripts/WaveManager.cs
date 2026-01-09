using Godot;
using System.Collections.Generic;
using TowerDefense.Map;

namespace TowerDefense.Enemies
{
    /// <summary>
    /// Manages enemy waves, spawning, and difficulty scaling
    /// </summary>
    public partial class WaveManager : Node
    {
        [Export]
        public PackedScene EnemyScene { get; set; }
        
        [Export]
        public float SpawnInterval { get; set; } = 1.0f;
        
        [Export]
        public int BaseEnemiesPerWave { get; set; } = 5;
        
        private MapGenerator _mapGenerator;
        private int _currentWave = 0;
        private int _enemiesSpawned = 0;
        private int _enemiesToSpawn = 0;
        private int _enemiesAlive = 0;
        private float _spawnTimer = 0;
        private int _difficultyMultiplier = 1;
        private bool _isSpawning = false;
        
        [Signal]
        public delegate void WaveCompletedEventHandler();
        
        [Signal]
        public delegate void EnemyReachedCastleEventHandler(int damage);
        
        public int EnemiesAlive => _enemiesAlive;
        
        public override void _Ready()
        {
            _mapGenerator = GetNode<MapGenerator>("../MapGenerator");
            
            if (EnemyScene == null)
            {
                GD.PrintErr("Enemy scene not set in WaveManager");
            }
        }
        
        public override void _Process(double delta)
        {
            if (!_isSpawning)
                return;
                
            _spawnTimer -= (float)delta;
            
            if (_spawnTimer <= 0 && _enemiesSpawned < _enemiesToSpawn)
            {
                SpawnEnemy();
                _enemiesSpawned++;
                _spawnTimer = SpawnInterval;
            }
            
            if (_enemiesSpawned >= _enemiesToSpawn && _enemiesAlive == 0)
            {
                CompleteWave();
            }
        }
        
        public void StartWave(int waveNumber, int difficultyMultiplier = 1)
        {
            _currentWave = waveNumber;
            _difficultyMultiplier = difficultyMultiplier;
            _enemiesSpawned = 0;
            _enemiesToSpawn = CalculateEnemyCount();
            _spawnTimer = 0;
            _isSpawning = true;
            
            GD.Print($"Starting wave {waveNumber} with {_enemiesToSpawn} enemies");
        }
        
        private int CalculateEnemyCount()
        {
            // Scale enemies based on wave number and difficulty multiplier
            return BaseEnemiesPerWave + (_currentWave * 2) * _difficultyMultiplier;
        }
        
        private void SpawnEnemy()
        {
            if (EnemyScene == null || _mapGenerator == null)
            {
                GD.PrintErr("Cannot spawn enemy: missing scene or map generator");
                return;
            }
            
            var gates = _mapGenerator.GatePositions;
            var paths = _mapGenerator.Paths;
            
            if (gates.Count == 0 || paths.Count == 0)
            {
                GD.PrintErr("No gates or paths available for spawning");
                return;
            }
            
            // Choose random gate
            int gateIndex = GD.RandRange(0, gates.Count - 1);
            Vector3 spawnPosition = gates[gateIndex];
            List<Vector3> path = paths[gateIndex];
            
            // Instantiate enemy
            var enemy = EnemyScene.Instantiate<Enemy>();
            enemy.GlobalPosition = spawnPosition;
            enemy.SetPath(path);
            
            // Scale enemy stats with wave
            enemy.MaxHealth = 100 + (_currentWave * 20);
            enemy.MoveSpeed = 3.0f + (_currentWave * 0.1f);
            enemy.Damage = 10 + (_currentWave * 2);
            
            GetTree().Root.AddChild(enemy);
            
            _enemiesAlive++;
            
            // Connect to enemy signals
            enemy.EnemyDied += OnEnemyDied;
            enemy.ReachedCastle += OnEnemyReachedCastle;
        }
        
        private void OnEnemyDied(Enemy enemy)
        {
            _enemiesAlive--;
            GD.Print($"Enemy died. Remaining: {_enemiesAlive}");
        }
        
        private void OnEnemyReachedCastle(Enemy enemy)
        {
            _enemiesAlive--;
            EmitSignal(SignalName.EnemyReachedCastle, enemy.Damage);
            GD.Print($"Enemy reached castle! Damage: {enemy.Damage}");
        }
        
        private void CompleteWave()
        {
            _isSpawning = false;
            GD.Print($"Wave {_currentWave} completed!");
            EmitSignal(SignalName.WaveCompleted);
        }
    }
}
