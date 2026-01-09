using Godot;

namespace TowerDefense.Towers
{
    /// <summary>
    /// Projectile that moves toward a target and deals damage on impact
    /// </summary>
    public partial class ProjectileMover : Node3D
    {
        private Node3D _target;
        private float _speed;
        private int _damage;
        
        public ProjectileMover()
        {
        }
        
        public ProjectileMover(Node3D target, float speed, int damage)
        {
            _target = target;
            _speed = speed;
            _damage = damage;
        }
        
        public override void _Ready()
        {
            if (HasMeta("target"))
                _target = GetMeta("target").As<Node3D>();
            if (HasMeta("speed"))
                _speed = GetMeta("speed").As<float>();
            if (HasMeta("damage"))
                _damage = GetMeta("damage").As<int>();
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
                HitTarget();
                QueueFree();
            }
        }
        
        private void HitTarget()
        {
            if (_target != null && IsInstanceValid(_target))
            {
                // Try to call TakeDamage on the target
                if (_target.HasMethod("TakeDamage"))
                {
                    _target.Call("TakeDamage", _damage);
                }
            }
        }
    }
}
