using System;
using System.Linq;
using Godot;
using Player.Domain.Models;

public partial class GodotEnemy : Character
{
    [Export]
    private AnimatedSprite2D[] _bodyParts;

    public override void _Ready() { }

    public override void _Process(double delta) { }

    public async void Play()
    {
        // TODO: add AI logic
        await ToSignal(GetTree().CreateTimer(1.0), Timer.SignalName.Timeout);
        Attack();
        GodotTurnManager.Instance.EndTurn(this);
    }

    private void Attack()
    {
        foreach (AnimatedSprite2D bodyPart in _bodyParts)
        {
            if (bodyPart.SpriteFrames.HasAnimation(CharacterActions.Slash.ToString()))
            {
                bodyPart.Play(CharacterActions.Slash.ToString());
            }
        }
    }
}
