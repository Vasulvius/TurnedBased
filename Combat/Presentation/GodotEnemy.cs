using Godot;

public partial class GodotEnemy : Character
{
    [Signal]
    public delegate void AttackRequestedEventHandler();

    public override async void Attack()
    {
        await ToSignal(GetTree().CreateTimer(0.8), Timer.SignalName.Timeout);
        base.Attack();
        EmitSignal(SignalName.AttackRequested);
    }
}
