using System.Runtime.InteropServices.Marshalling;
using Godot;
using Player.Domain.Models;

public partial class GodotPlayer : Character
{
    [Export]
    private AnimatedSprite2D[] _bodyParts;

    [Export]
    private Control _actionPanel;

    [Export]
    private GodotLifeBar _lifeBar;
    private int _maxLife = 100;
    private int _currentLife;
    private int _damage = 15;

    public override void _Ready()
    {
        _lifeBar.Init(_maxLife);
        _currentLife = _maxLife;
    }

    public void TakeDamage(int Damage)
    {
        _currentLife -= Damage;
        _lifeBar.SetValue(_currentLife);
    }

    private async void Attack()
    {
        foreach (AnimatedSprite2D bodyPart in _bodyParts)
        {
            if (bodyPart.SpriteFrames.HasAnimation(CharacterActions.Slash.ToString()))
            {
                bodyPart.Play(CharacterActions.Slash.ToString());
            }
        }
        // TODO: replace with awaiting end of animation
        await ToSignal(GetTree().CreateTimer(0.3), Timer.SignalName.Timeout);
        GodotTurnManager.Instance.DealDamage(_damage, this);

        GodotTurnManager.Instance.EndTurn(this);
    }

    public void EnableActionPanel()
    {
        _actionPanel.Visible = true;
    }

    public void DisableActionPanel()
    {
        _actionPanel.Visible = false;
    }
}
