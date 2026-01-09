using Godot;
using System;
using System.Collections.Generic;

namespace TowerDefense.Map
{
    /// <summary>
    /// Generates procedural maps with spawn gates and pathfinding for enemies
    /// </summary>
    public partial class MapGenerator : Node3D
    {
        [Export]
        public int MapSize { get; set; } = 50;
        
        [Export]
        public int NumGates { get; set; } = 4;
        
        [Export]
        public ulong MapSeed { get; set; } = 12345;
        
        private RandomNumberGenerator _rng;
        private Vector3 _castlePosition = Vector3.Zero;
        private List<Vector3> _gatePositions = new List<Vector3>();
        private List<List<Vector3>> _paths = new List<List<Vector3>>();
        
        public Vector3 CastlePosition => _castlePosition;
        public List<Vector3> GatePositions => _gatePositions;
        public List<List<Vector3>> Paths => _paths;
        
        [Signal]
        public delegate void MapGeneratedEventHandler();
        
        public override void _Ready()
        {
            _rng = new RandomNumberGenerator();
            _rng.Seed = MapSeed;
        }
        
        public void GenerateMap(ulong seed)
        {
            MapSeed = seed;
            _rng.Seed = seed;
            
            ClearMap();
            GenerateTerrain();
            GenerateCastle();
            GenerateGates();
            GeneratePaths();
            
            GD.Print($"Map generated with seed: {seed}");
            EmitSignal(SignalName.MapGenerated);
        }
        
        private void ClearMap()
        {
            foreach (var child in GetChildren())
            {
                if (child is Node3D)
                {
                    child.QueueFree();
                }
            }
            
            _gatePositions.Clear();
            _paths.Clear();
        }
        
        private void GenerateTerrain()
        {
            // Create ground plane
            var groundMesh = new BoxMesh();
            groundMesh.Size = new Vector3(MapSize, 1, MapSize);
            
            var groundInstance = new MeshInstance3D();
            groundInstance.Mesh = groundMesh;
            groundInstance.Position = new Vector3(0, -0.5f, 0);
            
            var groundMaterial = new StandardMaterial3D();
            groundMaterial.AlbedoColor = new Color(0.3f, 0.5f, 0.2f); // Green grass
            groundInstance.MaterialOverride = groundMaterial;
            
            // Add collision
            var staticBody = new StaticBody3D();
            var collisionShape = new CollisionShape3D();
            var boxShape = new BoxShape3D();
            boxShape.Size = groundMesh.Size;
            collisionShape.Shape = boxShape;
            
            staticBody.AddChild(collisionShape);
            groundInstance.AddChild(staticBody);
            
            AddChild(groundInstance);
        }
        
        private void GenerateCastle()
        {
            _castlePosition = Vector3.Zero;
            
            // Main keep
            var keepMesh = new BoxMesh();
            keepMesh.Size = new Vector3(8, 10, 8);
            
            var keepInstance = new MeshInstance3D();
            keepInstance.Mesh = keepMesh;
            keepInstance.Position = new Vector3(0, 5, 0);
            
            var keepMaterial = new StandardMaterial3D();
            keepMaterial.AlbedoColor = new Color(0.5f, 0.5f, 0.5f); // Gray stone
            keepInstance.MaterialOverride = keepMaterial;
            
            AddChild(keepInstance);
            
            // Add towers at corners
            for (int i = 0; i < 4; i++)
            {
                float angle = i * Mathf.Pi / 2;
                Vector3 offset = new Vector3(Mathf.Cos(angle) * 6, 0, Mathf.Sin(angle) * 6);
                
                var towerMesh = new CylinderMesh();
                towerMesh.TopRadius = 1.5f;
                towerMesh.BottomRadius = 1.5f;
                towerMesh.Height = 8;
                
                var towerInstance = new MeshInstance3D();
                towerInstance.Mesh = towerMesh;
                towerInstance.Position = offset + new Vector3(0, 4, 0);
                towerInstance.MaterialOverride = keepMaterial;
                
                AddChild(towerInstance);
            }
        }
        
        private void GenerateGates()
        {
            float radius = MapSize / 2.5f;
            
            for (int i = 0; i < NumGates; i++)
            {
                float angle = (i * 2 * Mathf.Pi / NumGates) + _rng.RandfRange(-0.3f, 0.3f);
                Vector3 position = new Vector3(
                    Mathf.Cos(angle) * radius,
                    0,
                    Mathf.Sin(angle) * radius
                );
                
                _gatePositions.Add(position);
                
                // Create gate visual
                var gateMesh = new BoxMesh();
                gateMesh.Size = new Vector3(4, 3, 1);
                
                var gateInstance = new MeshInstance3D();
                gateInstance.Mesh = gateMesh;
                gateInstance.Position = position + new Vector3(0, 1.5f, 0);
                gateInstance.RotateY(angle + Mathf.Pi / 2);
                
                var gateMaterial = new StandardMaterial3D();
                gateMaterial.AlbedoColor = new Color(0.4f, 0.2f, 0.1f); // Brown wood
                gateInstance.MaterialOverride = gateMaterial;
                
                AddChild(gateInstance);
            }
        }
        
        private void GeneratePaths()
        {
            foreach (var gatePos in _gatePositions)
            {
                var path = new List<Vector3>();
                Vector3 currentPos = gatePos;
                path.Add(currentPos);
                
                // Generate waypoints toward castle
                int numWaypoints = _rng.RandiRange(2, 4);
                
                for (int i = 0; i < numWaypoints; i++)
                {
                    float t = (i + 1) / (float)(numWaypoints + 1);
                    Vector3 targetPos = gatePos.Lerp(_castlePosition, t);
                    
                    // Add some randomness
                    targetPos.X += _rng.RandfRange(-3, 3);
                    targetPos.Z += _rng.RandfRange(-3, 3);
                    targetPos.Y = 0;
                    
                    path.Add(targetPos);
                    
                    // Visualize waypoint
                    CreateWaypoint(targetPos);
                }
                
                path.Add(_castlePosition);
                _paths.Add(path);
                
                // Draw path line
                DrawPath(path);
            }
        }
        
        private void CreateWaypoint(Vector3 position)
        {
            var waypointMesh = new SphereMesh();
            waypointMesh.Radius = 0.5f;
            waypointMesh.Height = 1.0f;
            
            var waypointInstance = new MeshInstance3D();
            waypointInstance.Mesh = waypointMesh;
            waypointInstance.Position = position + new Vector3(0, 0.5f, 0);
            
            var material = new StandardMaterial3D();
            material.AlbedoColor = new Color(0.8f, 0.6f, 0.2f);
            material.Transparency = BaseMaterial3D.TransparencyEnum.Alpha;
            material.AlbedoColor = new Color(0.8f, 0.6f, 0.2f, 0.5f);
            waypointInstance.MaterialOverride = material;
            
            AddChild(waypointInstance);
        }
        
        private void DrawPath(List<Vector3> path)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Vector3 start = path[i];
                Vector3 end = path[i + 1];
                Vector3 direction = end - start;
                float length = direction.Length();
                
                var pathMesh = new BoxMesh();
                pathMesh.Size = new Vector3(0.5f, 0.1f, length);
                
                var pathInstance = new MeshInstance3D();
                pathInstance.Mesh = pathMesh;
                pathInstance.Position = (start + end) / 2 + new Vector3(0, 0.05f, 0);
                pathInstance.LookAt(end, Vector3.Up);
                pathInstance.RotateObjectLocal(Vector3.Right, Mathf.Pi / 2);
                
                var material = new StandardMaterial3D();
                material.AlbedoColor = new Color(0.6f, 0.4f, 0.2f);
                pathInstance.MaterialOverride = material;
                
                AddChild(pathInstance);
            }
        }
    }
}
