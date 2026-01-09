using Godot;
using System.Collections.Generic;

namespace TowerDefense.Towers
{
    /// <summary>
    /// Slower tower that applies a slow debuff to enemies in range
    /// </summary>
    public partial class SlowerTower : Tower
    {
        [Export]
        public float SlowAmount { get; set; } = 0.5f; // 50% slow
        
        [Export]
        public float SlowDuration { get; set; } = 3.0f;
        
        private MeshInstance3D _towerBody;
        private MeshInstance3D _crystalTop;
        private List<Node3D> _affectedEnemies = new List<Node3D>();
        
        public override void _Ready()
        {
            base._Ready();
            CreateTowerMesh();
            FireRate = 0.5f; // Slower fire rate for debuffs
        }
        
        public override void _Process(double delta)
        {
            // Don't use base targeting, we affect all in range
            _fireTimer -= (float)delta;
            
            if (_fireTimer <= 0)
            {
                ApplySlowToEnemiesInRange();
                _fireTimer = 1.0f / FireRate;
            }
            
            // Animate crystal
            if (_crystalTop != null)
            {
                _crystalTop.RotateY((float)delta);
            }
        }
        
        private void CreateTowerMesh()
        {
            // Base
            var baseMesh = new CylinderMesh();
            baseMesh.TopRadius = 1.0f;
            baseMesh.BottomRadius = 1.2f;
            baseMesh.Height = 2.0f;
            
            _towerBody = new MeshInstance3D();
            _towerBody.Mesh = baseMesh;
            _towerBody.Position = new Vector3(0, 1.0f, 0);
            
            var bodyMaterial = new StandardMaterial3D();
            bodyMaterial.AlbedoColor = new Color(0.2f, 0.3f, 0.5f);
            _towerBody.MaterialOverride = bodyMaterial;
            
            AddChild(_towerBody);
            
            // Crystal top
            var crystalMesh = new PrismMesh();
            crystalMesh.Size = new Vector3(0.8f, 1.5f, 0.8f);
            
            _crystalTop = new MeshInstance3D();
            _crystalTop.Mesh = crystalMesh;
            _crystalTop.Position = new Vector3(0, 2.8f, 0);
            
            var crystalMaterial = new StandardMaterial3D();
            crystalMaterial.AlbedoColor = new Color(0.4f, 0.6f, 1.0f, 0.8f);
            crystalMaterial.Transparency = BaseMaterial3D.TransparencyEnum.Alpha;
            crystalMaterial.EmissionEnabled = true;
            crystalMaterial.Emission = new Color(0.4f, 0.6f, 1.0f);
            crystalMaterial.EmissionEnergyMultiplier = 3.0f;
            _crystalTop.MaterialOverride = crystalMaterial;
            
            AddChild(_crystalTop);
        }
        
        private void ApplySlowToEnemiesInRange()
        {
            var enemies = GetTree().GetNodesInGroup("enemies");
            _affectedEnemies.Clear();
            
            foreach (var enemy in enemies)
            {
                if (enemy is Node3D enemyNode && IsInstanceValid(enemyNode))
                {
                    float distance = GlobalPosition.DistanceTo(enemyNode.GlobalPosition);
                    if (distance <= Range)
                    {
                        _affectedEnemies.Add(enemyNode);
                        ApplySlow(enemyNode);
                    }
                }
            }
        }
        
        private void ApplySlow(Node3D enemy)
        {
            if (enemy.HasMethod("ApplySlow"))
            {
                enemy.Call("ApplySlow", SlowAmount, SlowDuration);
            }
        }
        
        protected override void Fire()
        {
            // Slow tower doesn't fire projectiles
            base.Fire();
        }
    }
}
