using Godot;

public partial class GodotTurnManager : Node
{
    [Export]
    private GodotPlayer _player;

    [Export]
    private GodotEnemy _enemy;

    [Export]
    private Label _turnLabel;
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

        SwitchCurrentCharacterTo(_player);
    }

    public void DealDamage(int Damage, Character from)
    {
        if (from == _player)
        {
            _enemy.TakeDamage(Damage);
        }
        else
        {
            _player.TakeDamage(Damage);
        }
    }

    public async void EndTurn(Character from)
    {
        await ToSignal(GetTree().CreateTimer(1.0), Timer.SignalName.Timeout);
        if (from == _player)
        {
            SwitchCurrentCharacterTo(_enemy);
        }
        else
        {
            SwitchCurrentCharacterTo(_player);
        }
    }

    private void SwitchCurrentCharacterTo(Character character)
    {
        if (character == _player)
        {
            _turnLabel.Text = "Player's turn";
            _player.EnableActionPanel();
        }
        else
        {
            _turnLabel.Text = "Enemy's turn";
            _player.DisableActionPanel();
            _enemy.Play();
        }
    }
}
