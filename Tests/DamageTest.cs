using Combat.Domain;

namespace Tests;

public class DamageTest
{
    private readonly Damage _defaultDamage;

    public DamageTest()
    {
        _defaultDamage = Damage.Create(10);
    }

    #region Create Tests
    [Fact]
    public void Instantiate_Damage_refuse_negative_values()
    {
        // Act
        var damage = () => Damage.Create(-10);

        // Assert
        Assert.Throws<ArgumentException>(damage);
    }

    [Fact]
    public void Instantiate_Damage_must_set_Value_correctly()
    {
        // Assert
        Assert.Equal(10, _defaultDamage.Value);
    }
    #endregion

    #region Reduce Tests
    [Fact]
    public void Reduce_Damage_must_set_Value_correctly()
    {
        // Arrange
        var defense = Defense.Create(5);

        // Act
        var act = _defaultDamage.Reduce(defense);

        // Assert
        Assert.Equal(5, act.Value);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(15)]
    public void Reduce_Damage_must_never_set_Value_below_0(int defenseValue)
    {
        // Arrange
        var defense = Defense.Create(defenseValue);

        // Act
        var act = _defaultDamage.Reduce(defense);

        // Assert
        Assert.Equal(0, act.Value);
    }
    #endregion
}
