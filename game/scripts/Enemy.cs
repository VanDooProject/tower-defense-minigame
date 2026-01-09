using Godot;
using System.Collections.Generic;

namespace TowerDefense.Enemies
{
    /// <summary>
    /// Base enemy class that follows paths and attacks the castle
    /// </summary>
    public partial class Enemy : CharacterBody3D
    {
        [Export]
        public int MaxHealth { get; set; } = 100;
        
        [Export]
        public float MoveSpeed { get; set; } = 3.0f;
        
        [Export]
        public int Damage { get; set; } = 10;
        
        [Export]
        public int Bounty { get; set; } = 50;
        
        private int _currentHealth;
        private List<Vector3> _path;
        private int _currentWaypoint = 0;
        private float _slowMultiplier = 1.0f;
        private float _slowTimer = 0;
        private MeshInstance3D _mesh;
        private MeshInstance3D _healthBar;
        
        [Signal]
        public delegate void EnemyDiedEventHandler(Enemy enemy);
        
        [Signal]
        public delegate void ReachedCastleEventHandler(Enemy enemy);
        
        public override void _Ready()
        {
            _currentHealth = MaxHealth;
            AddToGroup("enemies");
            CreateMesh();
            CreateHealthBar();
        }
        
        public override void _Process(double delta)
        {
            // Update slow timer
            if (_slowTimer > 0)
            {
                _slowTimer -= (float)delta;
                if (_slowTimer <= 0)
                {
                    _slowMultiplier = 1.0f;
                }
            }
            
            UpdateHealthBar();
        }
        
        public override void _PhysicsProcess(double delta)
        {
            if (_path == null || _path.Count == 0)
                return;
                
            if (_currentWaypoint >= _path.Count)
            {
                ReachCastle();
                return;
            }
            
            Vector3 target = _path[_currentWaypoint];
            target.Y = GlobalPosition.Y; // Keep same height
            
            Vector3 direction = (target - GlobalPosition).Normalized();
            float distance = GlobalPosition.DistanceTo(target);
            
            if (distance < 0.5f)
            {
                _currentWaypoint++;
            }
            else
            {
                Velocity = direction * MoveSpeed * _slowMultiplier;
                MoveAndSlide();
                
                // Face movement direction
                if (direction.Length() > 0)
                {
                    LookAt(GlobalPosition + direction, Vector3.Up);
                }
            }
        }
        
        private void CreateMesh()
        {
            // Create a simple capsule enemy
            var capsuleMesh = new CapsuleMesh();
            capsuleMesh.Radius = 0.5f;
            capsuleMesh.Height = 2.0f;
            
            _mesh = new MeshInstance3D();
            _mesh.Mesh = capsuleMesh;
            _mesh.Position = new Vector3(0, 1.0f, 0);
            
            var material = new StandardMaterial3D();
            material.AlbedoColor = new Color(0.8f, 0.2f, 0.2f); // Red enemy
            _mesh.MaterialOverride = material;
            
            AddChild(_mesh);
            
            // Add collision
            var collisionShape = new CollisionShape3D();
            var capsuleShape = new CapsuleShape3D();
            capsuleShape.Radius = 0.5f;
            capsuleShape.Height = 2.0f;
            collisionShape.Shape = capsuleShape;
            collisionShape.Position = new Vector3(0, 1.0f, 0);
            
            AddChild(collisionShape);
        }
        
        private void CreateHealthBar()
        {
            var barMesh = new BoxMesh();
            barMesh.Size = new Vector3(1.0f, 0.1f, 0.1f);
            
            _healthBar = new MeshInstance3D();
            _healthBar.Mesh = barMesh;
            _healthBar.Position = new Vector3(0, 2.5f, 0);
            
            var material = new StandardMaterial3D();
            material.AlbedoColor = new Color(0.0f, 1.0f, 0.0f);
            material.ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded;
            _healthBar.MaterialOverride = material;
            
            AddChild(_healthBar);
        }
        
        private void UpdateHealthBar()
        {
            if (_healthBar == null)
                return;
                
            float healthPercent = (float)_currentHealth / MaxHealth;
            _healthBar.Scale = new Vector3(healthPercent, 1.0f, 1.0f);
            
            // Change color based on health
            var material = _healthBar.MaterialOverride as StandardMaterial3D;
            if (material != null)
            {
                if (healthPercent > 0.5f)
                    material.AlbedoColor = new Color(0.0f, 1.0f, 0.0f);
                else if (healthPercent > 0.25f)
                    material.AlbedoColor = new Color(1.0f, 1.0f, 0.0f);
                else
                    material.AlbedoColor = new Color(1.0f, 0.0f, 0.0f);
            }
            
            // Make health bar face camera (billboard)
            var camera = GetViewport().GetCamera3D();
            if (camera != null)
            {
                _healthBar.LookAt(camera.GlobalPosition, Vector3.Up);
            }
        }
        
        public void SetPath(List<Vector3> path)
        {
            _path = new List<Vector3>(path);
            _currentWaypoint = 0;
        }
        
        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            
            if (_currentHealth <= 0)
            {
                Die();
            }
        }
        
        public void ApplySlow(float amount, float duration)
        {
            _slowMultiplier = 1.0f - amount;
            _slowTimer = duration;
            
            // Visual feedback
            if (_mesh != null)
            {
                var material = _mesh.MaterialOverride as StandardMaterial3D;
                if (material != null)
                {
                    material.AlbedoColor = new Color(0.4f, 0.4f, 0.8f); // Blue tint when slowed
                }
            }
        }
        
        private void Die()
        {
            EmitSignal(SignalName.EnemyDied, this);
            QueueFree();
        }
        
        private void ReachCastle()
        {
            EmitSignal(SignalName.ReachedCastle, this);
            QueueFree();
        }
    }
}
