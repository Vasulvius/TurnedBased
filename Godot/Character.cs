using Combat.Domain;
using Godot;

public partial class Character : Node2D
{
    [Export]
    protected AnimatedSprite2D[] _bodyParts;

    [Export]
    protected GodotLifeBar _lifeBar;

    [Export]
    public CombatantStats Stats { get; private set; }
    public CombatantId Id { get; protected set; } = CombatantId.Create();

    public override void _Ready()
    {
        _lifeBar.Init(Stats.MaxHealth);
    }

    public void UpdateHealth(int Health)
    {
        _lifeBar.SetValue(Health);
    }

    public virtual void Attack()
    {
        foreach (AnimatedSprite2D bodyPart in _bodyParts)
        {
            if (bodyPart.SpriteFrames.HasAnimation("Slash"))
            {
                bodyPart.Play("Slash");
            }
        }
    }
}
