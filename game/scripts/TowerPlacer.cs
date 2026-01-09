using Godot;
using TowerDefense.Core;
using TowerDefense.Towers;
using TowerDefense.Player;

namespace TowerDefense.UI
{
    /// <summary>
    /// Handles tower placement via mouse input
    /// </summary>
    public partial class TowerPlacer : Node3D
    {
        [Export]
        public PackedScene ShooterTowerScene { get; set; }
        
        [Export]
        public PackedScene SlowerTowerScene { get; set; }
        
        [Export]
        public PackedScene SplashTowerScene { get; set; }
        
        [Export]
        public float MinDistanceFromCastle { get; set; } = 10.0f;
        
        [Export]
        public float MinDistanceBetweenTowers { get; set; } = 3.0f;
        
        private string _selectedTowerType = "shooter";
        private Node3D _previewTower = null;
        private bool _isPlacementMode = false;
        private PlayerManager _playerManager;
        private GameStateManager _gameStateManager;
        private Camera3D _camera;
        
        public override void _Ready()
        {
            _playerManager = GetNode<PlayerManager>("../PlayerManager");
            _gameStateManager = GetNode<GameStateManager>("../GameStateManager");
            _camera = GetViewport().GetCamera3D();
            
            // Load tower scenes if not set
            if (ShooterTowerScene == null)
                ShooterTowerScene = GD.Load<PackedScene>("res://scenes/ShooterTower.tscn");
            if (SlowerTowerScene == null)
                SlowerTowerScene = GD.Load<PackedScene>("res://scenes/SlowerTower.tscn");
            if (SplashTowerScene == null)
                SplashTowerScene = GD.Load<PackedScene>("res://scenes/SplashTower.tscn");
        }
        
        public override void _Process(double delta)
        {
            if (_isPlacementMode && _previewTower != null)
            {
                UpdatePreviewPosition();
            }
        }
        
        public override void _Input(InputEvent @event)
        {
            // Only allow placement during planning phase
            if (_gameStateManager?.CurrentPhase != GameStateManager.GamePhase.Planning)
                return;
                
            if (@event is InputEventMouseButton mouseButton && mouseButton.Pressed)
            {
                if (mouseButton.ButtonIndex == MouseButton.Left)
                {
                    TryPlaceTower();
                }
                else if (mouseButton.ButtonIndex == MouseButton.Right)
                {
                    CancelPlacement();
                }
            }
            
            // Keyboard shortcuts for tower selection
            if (@event is InputEventKey keyEvent && keyEvent.Pressed)
            {
                switch (keyEvent.Keycode)
                {
                    case Key.Key1:
                        SelectTowerType("shooter");
                        break;
                    case Key.Key2:
                        SelectTowerType("slower");
                        break;
                    case Key.Key3:
                        SelectTowerType("splash");
                        break;
                    case Key.Escape:
                        CancelPlacement();
                        break;
                }
            }
        }
        
        public void SelectTowerType(string towerType)
        {
            _selectedTowerType = towerType;
            _isPlacementMode = true;
            
            // Remove old preview
            if (_previewTower != null)
            {
                _previewTower.QueueFree();
            }
            
            // Create new preview
            _previewTower = CreateTowerInstance(towerType);
            if (_previewTower != null)
            {
                AddChild(_previewTower);
                MakeTransparent(_previewTower);
            }
        }
        
        private void UpdatePreviewPosition()
        {
            if (_camera == null)
                return;
                
            var mousePos = GetViewport().GetMousePosition();
            var from = _camera.ProjectRayOrigin(mousePos);
            var to = from + _camera.ProjectRayNormal(mousePos) * 1000;
            
            var spaceState = GetWorld3D().DirectSpaceState;
            var query = PhysicsRayQueryParameters3D.Create(from, to);
            var result = spaceState.IntersectRay(query);
            
            if (result.Count > 0)
            {
                var position = result["position"].AsVector3();
                position.Y = 0; // Keep on ground
                _previewTower.GlobalPosition = position;
                
                // Change color based on validity
                bool isValid = IsValidPlacement(position);
                UpdatePreviewColor(isValid);
            }
        }
        
        private void TryPlaceTower()
        {
            if (!_isPlacementMode || _previewTower == null)
                return;
                
            Vector3 position = _previewTower.GlobalPosition;
            
            if (!IsValidPlacement(position))
            {
                GD.Print("Invalid placement location");
                return;
            }
            
            int cost = GetTowerCost(_selectedTowerType);
            
            if (_playerManager.TryPlaceTower(position, _selectedTowerType, cost))
            {
                PlaceTower(position, _selectedTowerType);
                GD.Print($"Placed {_selectedTowerType} tower at {position}");
            }
        }
        
        private void PlaceTower(Vector3 position, string towerType)
        {
            var tower = CreateTowerInstance(towerType);
            if (tower != null)
            {
                tower.GlobalPosition = position;
                
                // Set player ownership
                if (tower is Tower towerComponent)
                {
                    towerComponent.SetPlayer(_playerManager.PlayerIndex);
                }
                
                GetTree().Root.AddChild(tower);
            }
        }
        
        private void CancelPlacement()
        {
            _isPlacementMode = false;
            if (_previewTower != null)
            {
                _previewTower.QueueFree();
                _previewTower = null;
            }
        }
        
        private Node3D CreateTowerInstance(string towerType)
        {
            PackedScene scene = towerType switch
            {
                "shooter" => ShooterTowerScene,
                "slower" => SlowerTowerScene,
                "splash" => SplashTowerScene,
                _ => null
            };
            
            return scene?.Instantiate<Node3D>();
        }
        
        private int GetTowerCost(string towerType)
        {
            return towerType switch
            {
                "shooter" => 100,
                "slower" => 150,
                "splash" => 200,
                _ => 0
            };
        }
        
        private bool IsValidPlacement(Vector3 position)
        {
            // Check distance from castle
            if (position.Length() < MinDistanceFromCastle)
                return false;
            
            // Check distance from other towers
            var towers = GetTree().GetNodesInGroup("towers");
            foreach (var tower in towers)
            {
                if (tower is Node3D towerNode)
                {
                    if (towerNode.GlobalPosition.DistanceTo(position) < MinDistanceBetweenTowers)
                        return false;
                }
            }
            
            return true;
        }
        
        private void MakeTransparent(Node3D node)
        {
            foreach (var child in node.GetChildren())
            {
                if (child is MeshInstance3D mesh)
                {
                    var material = mesh.MaterialOverride as StandardMaterial3D;
                    if (material != null)
                    {
                        material = (StandardMaterial3D)material.Duplicate();
                        material.Transparency = BaseMaterial3D.TransparencyEnum.Alpha;
                        material.AlbedoColor = new Color(material.AlbedoColor, 0.5f);
                        mesh.MaterialOverride = material;
                    }
                }
                
                if (child is Node3D node3D)
                {
                    MakeTransparent(node3D);
                }
            }
        }
        
        private void UpdatePreviewColor(bool isValid)
        {
            if (_previewTower == null)
                return;
                
            Color color = isValid ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.5f);
            
            foreach (var child in _previewTower.GetChildren())
            {
                if (child is MeshInstance3D mesh && mesh.MaterialOverride is StandardMaterial3D material)
                {
                    material.AlbedoColor = color;
                }
            }
        }
    }
}
