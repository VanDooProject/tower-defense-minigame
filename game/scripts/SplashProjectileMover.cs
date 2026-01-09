using Godot;
using System.Collections.Generic;

namespace TowerDefense.Towers
{
    /// <summary>
    /// Splash projectile that deals area damage on impact
    /// </summary>
    public partial class SplashProjectileMover : Node3D
    {
        private Node3D _target;
        private float _speed;
        private int _damage;
        private float _splashRadius;
        
        public override void _Ready()
        {
            if (HasMeta("target"))
                _target = GetMeta("target").As<Node3D>();
            if (HasMeta("speed"))
                _speed = GetMeta("speed").As<float>();
            if (HasMeta("damage"))
                _damage = GetMeta("damage").As<int>();
            if (HasMeta("splash_radius"))
                _splashRadius = GetMeta("splash_radius").As<float>();
        }
        
        public override void _Process(double delta)
        {
            if (_target == null || !IsInstanceValid(_target))
            {
                QueueFree();
                return;
            }
            
            Vector3 direction = (_target.GlobalPosition - GlobalPosition).Normalized();
            GlobalPosition += direction * _speed * (float)delta;
            
            // Check if reached target
            if (GlobalPosition.DistanceTo(_target.GlobalPosition) < 0.5f)
            {
                CreateExplosion();
                DealSplashDamage();
                QueueFree();
            }
        }
        
        private void CreateExplosion()
        {
            // Create explosion visual
            var explosion = new Node3D();
            
            var mesh = new SphereMesh();
            mesh.Radius = _splashRadius;
            
            var meshInstance = new MeshInstance3D();
            meshInstance.Mesh = mesh;
            
            var material = new StandardMaterial3D();
            material.AlbedoColor = new Color(1.0f, 0.5f, 0.0f, 0.6f);
            material.Transparency = BaseMaterial3D.TransparencyEnum.Alpha;
            material.EmissionEnabled = true;
            material.Emission = new Color(1.0f, 0.3f, 0.0f);
            material.EmissionEnergyMultiplier = 5.0f;
            meshInstance.MaterialOverride = material;
            
            explosion.AddChild(meshInstance);
            explosion.GlobalPosition = GlobalPosition;
            
            GetTree().Root.AddChild(explosion);
            
            // Remove explosion after a short delay
            var timer = explosion.GetTree().CreateTimer(0.3f);
            timer.Timeout += () => explosion.QueueFree();
        }
        
        private void DealSplashDamage()
        {
            var enemies = GetTree().GetNodesInGroup("enemies");
            
            foreach (var enemy in enemies)
            {
                if (enemy is Node3D enemyNode && IsInstanceValid(enemyNode))
                {
                    float distance = GlobalPosition.DistanceTo(enemyNode.GlobalPosition);
                    if (distance <= _splashRadius)
                    {
                        // Damage falls off with distance
                        float damageMultiplier = 1.0f - (distance / _splashRadius);
                        int actualDamage = (int)(_damage * damageMultiplier);
                        
                        if (enemyNode.HasMethod("TakeDamage"))
                        {
                            enemyNode.Call("TakeDamage", actualDamage);
                        }
                    }
                }
            }
        }
    }
}
