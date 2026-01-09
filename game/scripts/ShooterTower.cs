using Godot;

namespace TowerDefense.Towers
{
    /// <summary>
    /// Shooter tower that fires projectiles at enemies
    /// </summary>
    public partial class ShooterTower : Tower
    {
        [Export]
        public float ProjectileSpeed { get; set; } = 20.0f;
        
        [Export]
        public int Damage { get; set; } = 10;
        
        [Export]
        public PackedScene ProjectileScene { get; set; }
        
        private MeshInstance3D _towerBody;
        private MeshInstance3D _turret;
        
        public override void _Ready()
        {
            base._Ready();
            CreateTowerMesh();
        }
        
        public override void _Process(double delta)
        {
            base._Process(delta);
            
            // Rotate turret toward target
            if (_target != null && IsInstanceValid(_target) && _turret != null)
            {
                Vector3 direction = _target.GlobalPosition - _turret.GlobalPosition;
                direction.Y = 0;
                if (direction.Length() > 0)
                {
                    _turret.LookAt(_turret.GlobalPosition + direction, Vector3.Up);
                }
            }
        }
        
        private void CreateTowerMesh()
        {
            // Base cylinder
            var baseMesh = new CylinderMesh();
            baseMesh.TopRadius = 1.0f;
            baseMesh.BottomRadius = 1.2f;
            baseMesh.Height = 2.0f;
            
            _towerBody = new MeshInstance3D();
            _towerBody.Mesh = baseMesh;
            _towerBody.Position = new Vector3(0, 1.0f, 0);
            
            var bodyMaterial = new StandardMaterial3D();
            bodyMaterial.AlbedoColor = new Color(0.4f, 0.4f, 0.4f);
            _towerBody.MaterialOverride = bodyMaterial;
            
            AddChild(_towerBody);
            
            // Turret
            var turretBase = new SphereMesh();
            turretBase.Radius = 0.6f;
            turretBase.Height = 1.2f;
            
            _turret = new MeshInstance3D();
            _turret.Mesh = turretBase;
            _turret.Position = new Vector3(0, 2.5f, 0);
            
            var turretMaterial = new StandardMaterial3D();
            turretMaterial.AlbedoColor = new Color(0.3f, 0.3f, 0.3f);
            _turret.MaterialOverride = turretMaterial;
            
            AddChild(_turret);
            
            // Barrel
            var barrelMesh = new CylinderMesh();
            barrelMesh.TopRadius = 0.15f;
            barrelMesh.BottomRadius = 0.15f;
            barrelMesh.Height = 1.0f;
            
            var barrel = new MeshInstance3D();
            barrel.Mesh = barrelMesh;
            barrel.Position = new Vector3(0, 0, -0.5f);
            barrel.RotateX(Mathf.Pi / 2);
            
            var barrelMaterial = new StandardMaterial3D();
            barrelMaterial.AlbedoColor = new Color(0.2f, 0.2f, 0.2f);
            barrel.MaterialOverride = barrelMaterial;
            
            _turret.AddChild(barrel);
        }
        
        protected override void Fire()
        {
            base.Fire();
            
            if (_target == null || !IsInstanceValid(_target))
                return;
                
            CreateProjectile();
        }
        
        private void CreateProjectile()
        {
            var projectile = new Node3D();
            projectile.AddToGroup("projectiles");
            
            var mesh = new SphereMesh();
            mesh.Radius = 0.3f;
            
            var meshInstance = new MeshInstance3D();
            meshInstance.Mesh = mesh;
            
            var material = new StandardMaterial3D();
            material.AlbedoColor = GetPlayerColor();
            material.EmissionEnabled = true;
            material.Emission = GetPlayerColor();
            material.EmissionEnergyMultiplier = 2.0f;
            meshInstance.MaterialOverride = material;
            
            projectile.AddChild(meshInstance);
            
            // Position at turret
            projectile.GlobalPosition = _turret.GlobalPosition + new Vector3(0, 0, -0.8f).Rotated(Vector3.Up, _turret.Rotation.Y);
            
            GetTree().Root.AddChild(projectile);
            
            // Add projectile script
            var script = new ProjectileMover(_target, ProjectileSpeed, Damage);
            projectile.SetScript(GD.Load<CSharpScript>("res://scripts/ProjectileMover.cs"));
            
            // Simple projectile movement (will be handled by ProjectileMover script)
            if (projectile is Node node)
            {
                node.SetMeta("target", _target);
                node.SetMeta("speed", ProjectileSpeed);
                node.SetMeta("damage", Damage);
            }
        }
    }
}
