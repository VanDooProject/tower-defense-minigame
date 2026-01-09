using Godot;
using System.Collections.Generic;

namespace TowerDefense.Core
{
    /// <summary>
    /// Manages turn-based game state, player turns, and game phases
    /// </summary>
    public partial class GameStateManager : Node
    {
        public enum GamePhase
        {
            Lobby,
            Planning,
            Combat,
            Victory,
            Defeat
        }
        
        [Export]
        public int MaxWaves { get; set; } = 10;
        
        [Signal]
        public delegate void GamePhaseChangedEventHandler(GamePhase newPhase);
        
        [Signal]
        public delegate void TurnChangedEventHandler(int playerIndex);
        
        [Signal]
        public delegate void WaveStartedEventHandler(int waveNumber);
        
        [Signal]
        public delegate void WaveCompletedEventHandler(int waveNumber);
        
        [Signal]
        public delegate void GameOverEventHandler(bool victory);
        
        private GamePhase _currentPhase = GamePhase.Lobby;
        private int _currentPlayerTurn = 0;
        private int _currentWave = 0;
        private int _playerCount = 1;
        private List<long> _playerIds = new List<long>();
        
        public GamePhase CurrentPhase => _currentPhase;
        public int CurrentWave => _currentWave;
        public int CurrentPlayerTurn => _currentPlayerTurn;
        public int PlayerCount => _playerCount;
        
        public void InitializeGame(int playerCount)
        {
            _playerCount = playerCount;
            _currentWave = 0;
            _currentPlayerTurn = 0;
            ChangePhase(GamePhase.Planning);
        }
        
        public void StartWave()
        {
            if (_currentPhase != GamePhase.Planning)
            {
                GD.PrintErr("Cannot start wave outside of planning phase");
                return;
            }
            
            _currentWave++;
            GD.Print($"Starting wave {_currentWave}/{MaxWaves}");
            
            ChangePhase(GamePhase.Combat);
            EmitSignal(SignalName.WaveStarted, _currentWave);
        }
        
        public void CompleteWave()
        {
            if (_currentPhase != GamePhase.Combat)
            {
                GD.PrintErr("Cannot complete wave outside of combat phase");
                return;
            }
            
            GD.Print($"Wave {_currentWave} completed");
            EmitSignal(SignalName.WaveCompleted, _currentWave);
            
            if (_currentWave >= MaxWaves)
            {
                EndGame(true);
            }
            else
            {
                ChangePhase(GamePhase.Planning);
                NextTurn();
            }
        }
        
        public void NextTurn()
        {
            if (_currentPhase != GamePhase.Planning)
                return;
                
            _currentPlayerTurn = (_currentPlayerTurn + 1) % _playerCount;
            GD.Print($"Turn passed to player {_currentPlayerTurn}");
            EmitSignal(SignalName.TurnChanged, _currentPlayerTurn);
        }
        
        public void EndGame(bool victory)
        {
            ChangePhase(victory ? GamePhase.Victory : GamePhase.Defeat);
            GD.Print($"Game ended: {(victory ? "Victory" : "Defeat")}");
            EmitSignal(SignalName.GameOver, victory);
        }
        
        private void ChangePhase(GamePhase newPhase)
        {
            _currentPhase = newPhase;
            GD.Print($"Game phase changed to: {newPhase}");
            EmitSignal(SignalName.GamePhaseChanged, (int)newPhase);
        }
        
        public int GetDifficultyMultiplier()
        {
            // Scale difficulty based on player count
            return _playerCount;
        }
    }
}
