using Combat.Domain;

namespace Tests;

public class AttackPowerTest
{
    private readonly AttackPower _defaultAttackPower;

    public AttackPowerTest()
    {
        _defaultAttackPower = AttackPower.Create(10);
    }

    [Fact]
    public void Instantiate_AttackPower_refuse_negative_values()
    {
        // Act
        var attackPower = () => AttackPower.Create(-10);

        // Assert
        Assert.Throws<ArgumentException>(attackPower);
    }

    [Fact]
    public void Instantiate_AttackPower_must_set_Value_correctly()
    { // Assert
        Assert.Equal(10, _defaultAttackPower.Value);
    }

    [Fact]
    public void ToDamage_AttackPower_must_convert_to_Damage_correctly()
    {
        // Act
        var damage = _defaultAttackPower.ToDamage();

        // Assert
        Assert.Equal(Damage.Create(10), damage);
    }
}
