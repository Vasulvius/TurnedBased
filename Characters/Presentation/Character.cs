using Combat.Domain;
using Godot;

public partial class Character : Node2D
{
    [Export]
    protected AnimatedSprite2D[] _bodyParts;

    [Export]
    protected GodotLifeBar _lifeBar;
    public CombatantId Id { get; protected set; } = CombatantId.Create();
    public int MaxHealth { get; protected set; } = 100;
    public int AttackPower { get; protected set; } = 40;
    public int Defense { get; protected set; } = 5;

    public override void _Ready()
    {
        _lifeBar.Init(MaxHealth);
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
