using Godot;
using Player.Domain.Models;

public partial class GodotPlayer : Character
{
    [Export]
    private Control _actionPanel;

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
