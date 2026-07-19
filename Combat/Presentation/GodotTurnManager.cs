using Godot;

public partial class GodotTurnManager : Node
{
    [Export]
    private Character _player;

    [Export]
    private Character _enemy;

    [Export]
    private Label _turnLabel;
    private Character _currentCharacter;
    public static GodotTurnManager Instance { get; private set; }

    public override void _Ready()
    {
        if (Instance != null && Instance != this)
        {
            GD.PrintErr("Another instance of GodotTurnManager already exists!");
            QueueFree();
            return;
        }

        Instance = this;

        SwitchCurrentCharacter();
    }

    public override void _Process(double delta) { }

    public void SwitchCurrentCharacter()
    {
        if (_currentCharacter == _enemy || _currentCharacter is null)
        {
            _currentCharacter = _player;
            EnablePlayerActions();
            _turnLabel.Text = "Player's turn";
        }
        else
        {
            _currentCharacter = _enemy;
            DisablePlayerActions();
            _turnLabel.Text = "Enemy's turn";
        }
    }

    private void EnablePlayerActions() { }

    private void DisablePlayerActions() { }
}
