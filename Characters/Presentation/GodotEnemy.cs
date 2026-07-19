using Godot;
using Player.Domain.Models;

public partial class GodotEnemy : Character
{
    [Export]
    private AnimatedSprite2D[] _bodyParts;

    public override void _Ready() { }

    public override void _Process(double delta) { }

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
