using Godot;

[GlobalClass]
public partial class CombatantStats : Resource
{
    [Export]
    public int MaxHealth { get; set; } = 100;

    [Export]
    public int AttackPower { get; set; } = 40;

    [Export]
    public int Defense { get; set; } = 5;
}
