using Godot;
using Player.Domain.Models;

public partial class GodotEnemy : Character
{
    public override void _Ready()
    {
        _lifeBar.Init(_maxLife);
        _currentLife = _maxLife;
    }

    public async void Play()
    {
        // TODO: add AI logic
        await ToSignal(GetTree().CreateTimer(1.0), Timer.SignalName.Timeout);
        Attack();
        GodotTurnManager.Instance.EndTurn(this);
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
    }
}
