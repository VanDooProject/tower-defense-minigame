using Godot;

namespace TowerDefense.UI
{
    /// <summary>
    /// Main menu UI controller
    /// </summary>
    public partial class MainMenu : Control
    {
        private Button _hostButton;
        private Button _joinButton;
        private Button _soloButton;
        private LineEdit _addressInput;
        
        public override void _Ready()
        {
            _hostButton = GetNode<Button>("VBoxContainer/HostButton");
            _joinButton = GetNode<Button>("VBoxContainer/JoinButton");
            _soloButton = GetNode<Button>("VBoxContainer/SoloButton");
            _addressInput = GetNode<LineEdit>("VBoxContainer/AddressInput");
            
            _hostButton.Pressed += OnHostPressed;
            _joinButton.Pressed += OnJoinPressed;
            _soloButton.Pressed += OnSoloPressed;
        }
        
        private void OnHostPressed()
        {
            GD.Print("Starting as host...");
            GetTree().ChangeSceneToFile("res://scenes/Game.tscn");
        }
        
        private void OnJoinPressed()
        {
            string address = _addressInput.Text;
            GD.Print($"Joining server at {address}...");
            GetTree().ChangeSceneToFile("res://scenes/Game.tscn");
        }
        
        private void OnSoloPressed()
        {
            GD.Print("Starting solo game...");
            GetTree().ChangeSceneToFile("res://scenes/Game.tscn");
        }
    }
}
