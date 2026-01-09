using Godot;

namespace TowerDefense.Player
{
    /// <summary>
    /// Manages player resources (gold) and tower placement
    /// </summary>
    public partial class PlayerManager : Node
    {
        [Export]
        public int StartingGold { get; set; } = 500;
        
        [Export]
        public int GoldPerWave { get; set; } = 200;
        
        private int _currentGold;
        private int _playerIndex;
        
        [Signal]
        public delegate void GoldChangedEventHandler(int amount);
        
        [Signal]
        public delegate void TowerPlacedEventHandler(Vector3 position, string towerType);
        
        public int CurrentGold => _currentGold;
        public int PlayerIndex => _playerIndex;
        
        public override void _Ready()
        {
            _currentGold = StartingGold;
        }
        
        public void Initialize(int playerIndex)
        {
            _playerIndex = playerIndex;
            _currentGold = StartingGold;
            EmitSignal(SignalName.GoldChanged, _currentGold);
        }
        
        public void AddGold(int amount)
        {
            _currentGold += amount;
            EmitSignal(SignalName.GoldChanged, _currentGold);
            GD.Print($"Player {_playerIndex} gold: {_currentGold}");
        }
        
        public bool SpendGold(int amount)
        {
            if (_currentGold >= amount)
            {
                _currentGold -= amount;
                EmitSignal(SignalName.GoldChanged, _currentGold);
                return true;
            }
            
            GD.Print($"Not enough gold! Have: {_currentGold}, Need: {amount}");
            return false;
        }
        
        public bool CanAfford(int cost)
        {
            return _currentGold >= cost;
        }
        
        public void OnWaveCompleted()
        {
            AddGold(GoldPerWave);
        }
        
        public void OnEnemyKilled(int bounty)
        {
            AddGold(bounty);
        }
        
        public bool TryPlaceTower(Vector3 position, string towerType, int cost)
        {
            if (!CanAfford(cost))
            {
                return false;
            }
            
            if (SpendGold(cost))
            {
                EmitSignal(SignalName.TowerPlaced, position, towerType);
                return true;
            }
            
            return false;
        }
    }
}
