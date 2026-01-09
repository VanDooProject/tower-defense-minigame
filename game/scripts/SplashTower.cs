using Godot;
using System.Collections.Generic;

namespace TowerDefense.Towers
{
    /// <summary>
    /// Splash tower that deals area damage to multiple enemies
    /// </summary>
    public partial class SplashTower : Tower
    {
        [Export]
        public float SplashRadius { get; set; } = 5.0f;
        
        [Export]
        public int Damage { get; set; } = 15;
        
        private MeshInstance3D _towerBody;
        private MeshInstance3D _mortar;
        
        public override void _Ready()
        {
            base._Ready();
            CreateTowerMesh();
            FireRate = 0.3f; // Slower but more powerful
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
            bodyMaterial.AlbedoColor = new Color(0.5f, 0.3f, 0.2f);
            _towerBody.MaterialOverride = bodyMaterial;
            
            AddChild(_towerBody);
            
            // Mortar barrel
            var mortarBase = new BoxMesh();
            mortarBase.Size = new Vector3(1.0f, 0.5f, 1.0f);
            
            _mortar = new MeshInstance3D();
            _mortar.Mesh = mortarBase;
            _mortar.Position = new Vector3(0, 2.5f, 0);
            
            var mortarMaterial = new StandardMaterial3D();
            mortarMaterial.AlbedoColor = new Color(0.3f, 0.2f, 0.1f);
            _mortar.MaterialOverride = mortarMaterial;
            
            AddChild(_mortar);
            
            // Barrel tube
            var barrelMesh = new CylinderMesh();
            barrelMesh.TopRadius = 0.3f;
            barrelMesh.BottomRadius = 0.3f;
            barrelMesh.Height = 1.0f;
            
            var barrel = new MeshInstance3D();
            barrel.Mesh = barrelMesh;
            barrel.Position = new Vector3(0, 0.5f, 0);
            barrel.RotateX(Mathf.Pi / 4); // Angle upward
            
            var barrelMaterial = new StandardMaterial3D();
            barrelMaterial.AlbedoColor = new Color(0.2f, 0.2f, 0.2f);
            barrel.MaterialOverride = barrelMaterial;
            
            _mortar.AddChild(barrel);
        }
        
        protected override void Fire()
        {
            base.Fire();
            
            if (_target == null || !IsInstanceValid(_target))
                return;
                
            CreateSplashProjectile();
        }
        
        private void CreateSplashProjectile()
        {
            var projectile = new Node3D();
            projectile.AddToGroup("projectiles");
            
            var mesh = new SphereMesh();
            mesh.Radius = 0.5f;
            
            var meshInstance = new MeshInstance3D();
            meshInstance.Mesh = mesh;
            
            var material = new StandardMaterial3D();
            material.AlbedoColor = new Color(1.0f, 0.5f, 0.0f);
            material.EmissionEnabled = true;
            material.Emission = new Color(1.0f, 0.5f, 0.0f);
            material.EmissionEnergyMultiplier = 3.0f;
            meshInstance.MaterialOverride = material;
            
            projectile.AddChild(meshInstance);
            
            // Position at mortar
            projectile.GlobalPosition = _mortar.GlobalPosition + new Vector3(0, 0.5f, 0);
            
            GetTree().Root.AddChild(projectile);
            
            // Add splash projectile script
            if (projectile is Node node)
            {
                node.SetMeta("target", _target);
                node.SetMeta("speed", 15.0f);
                node.SetMeta("damage", Damage);
                node.SetMeta("splash_radius", SplashRadius);
            }
            
            projectile.SetScript(GD.Load<CSharpScript>("res://scripts/SplashProjectileMover.cs"));
        }
    }
}
