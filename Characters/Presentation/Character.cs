using Godot;

public partial class Character : Node2D
{
    [Export]
    protected AnimatedSprite2D[] _bodyParts;

    [Export]
    protected GodotLifeBar _lifeBar;
    protected int _maxLife = 100;
    protected int _currentLife;
    protected int _damage = 15;

    public override void _Ready()
    {
        _lifeBar.Init(_maxLife);
        _currentLife = _maxLife;
    }

    public void TakeDamage(int Damage)
    {
        _currentLife -= Damage;
        _lifeBar.SetValue(_currentLife);
    }
}
