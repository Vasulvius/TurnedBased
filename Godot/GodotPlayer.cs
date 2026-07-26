using Godot;

public partial class GodotPlayer : Character
{
    [Export]
    private Control _actionPanel = null!;

    [Signal]
    public delegate void AttackRequestedEventHandler();

    public void ChangeActionPanelVibility(bool isVisible)
    {
        _actionPanel.Visible = isVisible;
    }

    public override void Attack()
    {
        base.Attack();
        EmitSignal(SignalName.AttackRequested);
    }
}
