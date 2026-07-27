using Combat.Domain;

namespace Tests;

public class UnitTest1
{
    [Fact]
    public void Reference_to_Core_works()
    {
        var health = Health.Create(100);
        Assert.Equal(100, health.Current);
    }
}
