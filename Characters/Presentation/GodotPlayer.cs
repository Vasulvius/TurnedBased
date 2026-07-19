using Godot;
using Player.Domain.Models;

public partial class GodotPlayer : Character
{
    [Export]
    private AnimatedSprite2D[] _bodyParts;

    [Export]
    private Control _actionPanel;

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
