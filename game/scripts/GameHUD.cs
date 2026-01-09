using Godot;
using TowerDefense.Core;
using TowerDefense.Player;

namespace TowerDefense.UI
{
    /// <summary>
    /// In-game HUD displaying player resources, wave info, and controls
    /// </summary>
    public partial class GameHUD : Control
    {
        private Label _goldLabel;
        private Label _waveLabel;
        private Label _phaseLabel;
        private Label _healthLabel;
        private Button _startWaveButton;
        private Button _shooterButton;
        private Button _slowerButton;
        private Button _splashButton;
        private Label _messageLabel;
        
        private PlayerManager _playerManager;
        private GameStateManager _gameStateManager;
        private TowerPlacer _towerPlacer;
        
        public override void _Ready()
        {
            SetupUI();
            ConnectToManagers();
        }
        
        private void SetupUI()
        {
            // Main container
            var vbox = new VBoxContainer();
            vbox.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(vbox);
            
            // Top bar
            var topBar = new HBoxContainer();
            topBar.AddThemeConstantOverride("separation", 20);
            vbox.AddChild(topBar);
            
            // Gold display
            _goldLabel = new Label();
            _goldLabel.Text = "Gold: 500";
            _goldLabel.AddThemeColorOverride("font_color", Colors.Gold);
            _goldLabel.AddThemeFontSizeOverride("font_size", 24);
            topBar.AddChild(_goldLabel);
            
            topBar.AddChild(new Control { CustomMinimumSize = new Vector2(50, 0) });
            
            // Wave display
            _waveLabel = new Label();
            _waveLabel.Text = "Wave: 0/10";
            _waveLabel.AddThemeFontSizeOverride("font_size", 24);
            topBar.AddChild(_waveLabel);
            
            topBar.AddChild(new Control { CustomMinimumSize = new Vector2(50, 0) });
            
            // Phase display
            _phaseLabel = new Label();
            _phaseLabel.Text = "Phase: Planning";
            _phaseLabel.AddThemeFontSizeOverride("font_size", 24);
            topBar.AddChild(_phaseLabel);
            
            topBar.AddChild(new Control { CustomMinimumSize = new Vector2(50, 0) });
            
            // Health display
            _healthLabel = new Label();
            _healthLabel.Text = "Castle: 100";
            _healthLabel.AddThemeColorOverride("font_color", Colors.Red);
            _healthLabel.AddThemeFontSizeOverride("font_size", 24);
            topBar.AddChild(_healthLabel);
            
            // Spacer
            var spacer = new Control();
            spacer.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
            vbox.AddChild(spacer);
            
            // Message area
            _messageLabel = new Label();
            _messageLabel.Text = "";
            _messageLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _messageLabel.AddThemeFontSizeOverride("font_size", 20);
            _messageLabel.AddThemeColorOverride("font_color", Colors.Yellow);
            vbox.AddChild(_messageLabel);
            
            // Bottom bar - Tower selection and actions
            var bottomBar = new HBoxContainer();
            bottomBar.AddThemeConstantOverride("separation", 10);
            vbox.AddChild(bottomBar);
            
            // Tower buttons
            _shooterButton = CreateTowerButton("Shooter (1)", "100g");
            _shooterButton.Pressed += () => OnTowerSelected("shooter");
            bottomBar.AddChild(_shooterButton);
            
            _slowerButton = CreateTowerButton("Slower (2)", "150g");
            _slowerButton.Pressed += () => OnTowerSelected("slower");
            bottomBar.AddChild(_slowerButton);
            
            _splashButton = CreateTowerButton("Splash (3)", "200g");
            _splashButton.Pressed += () => OnTowerSelected("splash");
            bottomBar.AddChild(_splashButton);
            
            bottomBar.AddChild(new Control { CustomMinimumSize = new Vector2(50, 0) });
            
            // Start wave button
            _startWaveButton = new Button();
            _startWaveButton.Text = "Start Wave (SPACE)";
            _startWaveButton.CustomMinimumSize = new Vector2(200, 50);
            _startWaveButton.Pressed += OnStartWavePressed;
            bottomBar.AddChild(_startWaveButton);
            
            // Add margins
            var margin = new MarginContainer();
            margin.AddThemeConstantOverride("margin_left", 10);
            margin.AddThemeConstantOverride("margin_top", 10);
            margin.AddThemeConstantOverride("margin_right", 10);
            margin.AddThemeConstantOverride("margin_bottom", 10);
            
            RemoveChild(vbox);
            margin.AddChild(vbox);
            AddChild(margin);
        }
        
        private Button CreateTowerButton(string name, string cost)
        {
            var button = new Button();
            button.Text = $"{name}\n{cost}";
            button.CustomMinimumSize = new Vector2(120, 60);
            return button;
        }
        
        private void ConnectToManagers()
        {
            // Try to find managers in the scene tree
            var gameController = GetNode("/root/GameController");
            if (gameController != null)
            {
                _playerManager = gameController.GetNodeOrNull<PlayerManager>("PlayerManager");
                _gameStateManager = gameController.GetNodeOrNull<GameStateManager>("GameStateManager");
                _towerPlacer = gameController.GetNodeOrNull<TowerPlacer>("TowerPlacer");
                
                if (_playerManager != null)
                {
                    _playerManager.GoldChanged += OnGoldChanged;
                }
                
                if (_gameStateManager != null)
                {
                    _gameStateManager.GamePhaseChanged += OnPhaseChanged;
                    _gameStateManager.WaveStarted += OnWaveStarted;
                }
            }
        }
        
        private void OnGoldChanged(int amount)
        {
            _goldLabel.Text = $"Gold: {amount}";
            
            // Update button states
            if (_playerManager != null)
            {
                _shooterButton.Disabled = !_playerManager.CanAfford(100);
                _slowerButton.Disabled = !_playerManager.CanAfford(150);
                _splashButton.Disabled = !_playerManager.CanAfford(200);
            }
        }
        
        private void OnPhaseChanged(int phase)
        {
            var phaseEnum = (GameStateManager.GamePhase)phase;
            _phaseLabel.Text = $"Phase: {phaseEnum}";
            
            // Update button visibility based on phase
            bool isPlanning = phaseEnum == GameStateManager.GamePhase.Planning;
            _startWaveButton.Visible = isPlanning;
            _shooterButton.Visible = isPlanning;
            _slowerButton.Visible = isPlanning;
            _splashButton.Visible = isPlanning;
            
            // Show messages
            if (phaseEnum == GameStateManager.GamePhase.Combat)
            {
                ShowMessage("Wave in progress!");
            }
            else if (phaseEnum == GameStateManager.GamePhase.Victory)
            {
                ShowMessage("VICTORY! You defended the castle!");
            }
            else if (phaseEnum == GameStateManager.GamePhase.Defeat)
            {
                ShowMessage("DEFEAT! The castle has fallen!");
            }
            else
            {
                _messageLabel.Text = "";
            }
        }
        
        private void OnWaveStarted(int waveNumber)
        {
            if (_gameStateManager != null)
            {
                _waveLabel.Text = $"Wave: {waveNumber}/{_gameStateManager.MaxWaves}";
            }
        }
        
        private void OnTowerSelected(string towerType)
        {
            _towerPlacer?.SelectTowerType(towerType);
            ShowMessage($"Place {towerType} tower (Right-click to cancel)");
        }
        
        private void OnStartWavePressed()
        {
            _gameStateManager?.StartWave();
        }
        
        public void UpdateHealth(int health)
        {
            _healthLabel.Text = $"Castle: {health}";
            
            // Change color based on health
            if (health > 60)
                _healthLabel.AddThemeColorOverride("font_color", Colors.Green);
            else if (health > 30)
                _healthLabel.AddThemeColorOverride("font_color", Colors.Orange);
            else
                _healthLabel.AddThemeColorOverride("font_color", Colors.Red);
        }
        
        private void ShowMessage(string message)
        {
            _messageLabel.Text = message;
            
            // Auto-hide after 3 seconds
            var timer = GetTree().CreateTimer(3.0f);
            timer.Timeout += () => {
                if (_messageLabel.Text == message)
                    _messageLabel.Text = "";
            };
        }
    }
}
