using Godot;

namespace TowerDefense.Towers
{
    /// <summary>
    /// Base class for all tower types with player ownership
    /// </summary>
    public partial class Tower : Node3D
    {
        [Export]
        public float Range { get; set; } = 10.0f;
        
        [Export]
        public float FireRate { get; set; } = 1.0f;
        
        [Export]
        public int Cost { get; set; } = 100;
        
        [Export]
        public int PlayerIndex { get; set; } = 0;
        
        protected float _fireTimer = 0;
        protected Node3D _target = null;
        protected MeshInstance3D _ownershipRing;
        
        private static readonly Color[] PlayerColors = new Color[]
        {
            new Color(1.0f, 0.0f, 0.0f), // Red
            new Color(0.0f, 0.0f, 1.0f), // Blue
            new Color(0.0f, 1.0f, 0.0f), // Green
            new Color(1.0f, 1.0f, 0.0f)  // Yellow
        };
        
        [Signal]
        public delegate void TowerFiredEventHandler(Node3D target);
        
        public override void _Ready()
        {
            AddToGroup("towers");
            CreateOwnershipRing();
            UpdateOwnershipColor();
        }
        
        public override void _Process(double delta)
        {
            _fireTimer -= (float)delta;
            
            if (_fireTimer <= 0)
            {
                AcquireTarget();
                
                if (_target != null && IsInstanceValid(_target))
                {
                    Fire();
                    _fireTimer = 1.0f / FireRate;
                }
            }
        }
        
        protected virtual void CreateOwnershipRing()
        {
            var ringMesh = new TorusMesh();
            ringMesh.InnerRadius = 1.2f;
            ringMesh.OuterRadius = 1.5f;
            ringMesh.Rings = 16;
            ringMesh.RingSegments = 32;
            
            _ownershipRing = new MeshInstance3D();
            _ownershipRing.Mesh = ringMesh;
            _ownershipRing.Position = new Vector3(0, 0.1f, 0);
            
            AddChild(_ownershipRing);
        }
        
        protected void UpdateOwnershipColor()
        {
            if (_ownershipRing == null)
                return;
                
            var material = new StandardMaterial3D();
            material.AlbedoColor = GetPlayerColor();
            material.EmissionEnabled = true;
            material.Emission = GetPlayerColor() * 0.5f;
            material.EmissionEnergyMultiplier = 2.0f;
            
            _ownershipRing.MaterialOverride = material;
        }
        
        protected Color GetPlayerColor()
        {
            if (PlayerIndex >= 0 && PlayerIndex < PlayerColors.Length)
                return PlayerColors[PlayerIndex];
            return new Color(0.5f, 0.5f, 0.5f);
        }
        
        protected virtual void AcquireTarget()
        {
            _target = null;
            var spaceState = GetWorld3D().DirectSpaceState;
            var enemies = GetTree().GetNodesInGroup("enemies");
            
            float closestDistance = Range;
            
            foreach (var enemy in enemies)
            {
                if (enemy is Node3D enemyNode && IsInstanceValid(enemyNode))
                {
                    float distance = GlobalPosition.DistanceTo(enemyNode.GlobalPosition);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        _target = enemyNode;
                    }
                }
            }
        }
        
        protected virtual void Fire()
        {
            EmitSignal(SignalName.TowerFired, _target);
        }
        
        public void SetPlayer(int playerIndex)
        {
            PlayerIndex = playerIndex;
            UpdateOwnershipColor();
        }
    }
}
