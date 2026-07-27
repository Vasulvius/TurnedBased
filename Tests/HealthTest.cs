using Combat.Domain;

namespace Tests;

public class HealthTest
{
    [Fact]
    public void Instantiate_Health_refuse_negative_values()
    {
        // Act
        var health = () => Health.Create(-10);

        // Assert
        Assert.Throws<ArgumentException>(health);
    }

    [Fact]
    public void Instantiate_Health_set_current_to_max_health()
    {
        // Act
        var health = Health.Create(100);

        // Assert
        Assert.Equal(100, health.Max);
        Assert.Equal(100, health.Current);
    }

    [Fact]
    public void Instantiate_Health_must_set_IsDepleted_to_false()
    {
        // Act
        var health = Health.Create(100);

        // Assert
        Assert.False(health.IsDepleted);
    }

    [Fact]
    public void Reduce_Health_must_reduce_Current()
    {
        // Arrange
        var health = Health.Create(100);
        var damage = Damage.Create(50);

        // Act
        Health act = health.Reduce(damage);

        // Assert
        Assert.Equal(50, act.Current);
    }

    [Fact]
    public void Reduce_Health_must_not_affect_Max()
    {
        // Arrange
        var health = Health.Create(100);
        var damage = Damage.Create(50);

        // Act
        Health act = health.Reduce(damage);

        // Assert
        Assert.Equal(100, act.Max);
    }

    [Fact]
    public void Reduce_Health_above_0_must_keep_IsDepleted_false()
    {
        // Arrange
        var health = Health.Create(100);
        var damage = Damage.Create(50);

        // Act
        Health act = health.Reduce(damage);

        // Assert
        Assert.False(act.IsDepleted);
    }

    [Fact]
    public void OverReduce_Health_must_never_have_Current_below_0()
    {
        // Arrange
        var health = Health.Create(100);
        var damage = Damage.Create(110);

        // Act
        Health act = health.Reduce(damage);

        // Assert
        Assert.Equal(0, act.Current);
    }

    [Theory]
    [InlineData(100)]
    [InlineData(110)]
    public void Reduce_Health_to_0_must_set_IsDepleted_true(int damageValue)
    {
        // Arrange
        var health = Health.Create(100);
        var damage = Damage.Create(damageValue);

        // Act
        Health act = health.Reduce(damage);

        // Assert
        Assert.True(act.IsDepleted);
    }
}
