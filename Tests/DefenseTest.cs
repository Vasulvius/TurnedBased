using Combat.Domain;

namespace Tests;

public class DefenseTest
{
    [Fact]
    public void Instantiate_Defense_refuse_negative_values()
    {
        // Act
        var defense = () => Defense.Create(-10);

        // Assert
        Assert.Throws<ArgumentException>(defense);
    }

    [Fact]
    public void Instantiate_Defense_must_set_Value_correctly()
    {
        // Act
        var defense = Defense.Create(10);

        // Assert
        Assert.Equal(10, defense.Value);
    }
}
