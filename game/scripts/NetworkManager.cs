using Godot;

namespace TowerDefense.Core
{
    /// <summary>
    /// Manages multiplayer connection, player synchronization, and network events
    /// </summary>
    public partial class NetworkManager : Node
    {
        private const int MaxPlayers = 4;
        private const int DefaultPort = 7777;
        
        private ENetMultiplayerPeer _peer;
        
        [Signal]
        public delegate void PlayerConnectedEventHandler(long peerId, int playerIndex);
        
        [Signal]
        public delegate void PlayerDisconnectedEventHandler(long peerId);
        
        [Signal]
        public delegate void ServerStartedEventHandler();
        
        [Signal]
        public delegate void ConnectionSucceededEventHandler();
        
        [Signal]
        public delegate void ConnectionFailedEventHandler();
        
        public int PlayerCount { get; private set; }
        
        public override void _Ready()
        {
            Multiplayer.PeerConnected += OnPeerConnected;
            Multiplayer.PeerDisconnected += OnPeerDisconnected;
            Multiplayer.ConnectedToServer += OnConnectedToServer;
            Multiplayer.ConnectionFailed += OnConnectionFailed;
            Multiplayer.ServerDisconnected += OnServerDisconnected;
        }
        
        public bool CreateServer(int port = DefaultPort)
        {
            _peer = new ENetMultiplayerPeer();
            var error = _peer.CreateServer(port, MaxPlayers);
            
            if (error != Error.Ok)
            {
                GD.PrintErr($"Failed to create server: {error}");
                return false;
            }
            
            Multiplayer.MultiplayerPeer = _peer;
            PlayerCount = 1;
            
            GD.Print($"Server started on port {port}");
            EmitSignal(SignalName.ServerStarted);
            return true;
        }
        
        public bool JoinServer(string address, int port = DefaultPort)
        {
            _peer = new ENetMultiplayerPeer();
            var error = _peer.CreateClient(address, port);
            
            if (error != Error.Ok)
            {
                GD.PrintErr($"Failed to create client: {error}");
                return false;
            }
            
            Multiplayer.MultiplayerPeer = _peer;
            GD.Print($"Attempting to connect to {address}:{port}");
            return true;
        }
        
        public void DisconnectFromServer()
        {
            if (_peer != null)
            {
                _peer.Close();
                _peer = null;
            }
            
            Multiplayer.MultiplayerPeer = null;
            PlayerCount = 0;
        }
        
        private void OnPeerConnected(long id)
        {
            PlayerCount++;
            GD.Print($"Player {id} connected. Total players: {PlayerCount}");
            EmitSignal(SignalName.PlayerConnected, id, PlayerCount - 1);
        }
        
        private void OnPeerDisconnected(long id)
        {
            PlayerCount--;
            GD.Print($"Player {id} disconnected. Total players: {PlayerCount}");
            EmitSignal(SignalName.PlayerDisconnected, id);
        }
        
        private void OnConnectedToServer()
        {
            GD.Print("Successfully connected to server");
            EmitSignal(SignalName.ConnectionSucceeded);
        }
        
        private void OnConnectionFailed()
        {
            GD.PrintErr("Connection to server failed");
            EmitSignal(SignalName.ConnectionFailed);
        }
        
        private void OnServerDisconnected()
        {
            GD.Print("Disconnected from server");
            DisconnectFromServer();
        }
    }
}
