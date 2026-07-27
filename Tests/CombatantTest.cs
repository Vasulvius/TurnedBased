using Combat.Domain;

namespace Tests;

public class CombatantTest
{
    private static Combatant MakeCombatant(
        int hp = 100,
        int atk = 15,
        int def = 5,
        CombatantId? id = null
    ) =>
        new(
            new CombatantBlueprint(
                id ?? CombatantId.Create(),
                Health.Create(hp),
                AttackPower.Create(atk),
                Defense.Create(def)
            )
        );

    #region Equality Tests
    [Fact]
    public void Combatants_with_same_id_but_different_stats_are_equal()
    {
        // Arrange
        var commonId = CombatantId.Create();

        // Act
        var combatant1 = MakeCombatant(id: commonId, atk: 20);
        var combatant2 = MakeCombatant(id: commonId, atk: 10);

        // Assert
        Assert.Equal(combatant1, combatant2);
    }

    [Fact]
    public void Combatants_with_different_id_but_same_stats_are_different()
    {
        // Arrange
        var combatant1 = MakeCombatant();
        var combatant2 = MakeCombatant();

        // Assert
        Assert.NotEqual(combatant1, combatant2);
    }
    #endregion

    #region TakeDamage Tests
    [Fact]
    public void TakeDamage_must_reduce_health()
    {
        // Arrange
        var damage = Damage.Create(10);
        var combatant = MakeCombatant(hp: 100, def: 0);

        // Act
        combatant.TakeDamage(damage);

        // Assert
        Assert.Equal(90, combatant.Health.Current);
    }

    [Fact]
    public void TakeDamage_must_mitigate_damages()
    {
        // Arrange
        var damage = Damage.Create(10);
        var combatant = MakeCombatant(hp: 100, def: 5);

        // Act
        combatant.TakeDamage(damage);

        // Assert
        Assert.Equal(95, combatant.Health.Current);
    }

    [Fact]
    public void TakeDamage_return_mitigated_damages()
    {
        // Arrange
        var damage = Damage.Create(10);
        var combatant = MakeCombatant(def: 5);

        // Act
        var mitigatedDamages = combatant.TakeDamage(damage);

        // Assert
        Assert.Equal(Damage.Create(5), mitigatedDamages);
    }

    [Fact]
    public void Take_lethal_damage_must_defeate_combatant()
    {
        // Arrange
        var damage = Damage.Create(110);
        var combatant = MakeCombatant(hp: 100, def: 0);

        // Act
        combatant.TakeDamage(damage);

        // Assert
        Assert.True(combatant.IsDefeated);
    }

    [Fact]
    public void Take_not_lethal_damage_must_keep_combatant_undefeated()
    {
        // Arrange
        var damage = Damage.Create(10);
        var combatant = MakeCombatant(hp: 100, def: 0);

        // Act
        combatant.TakeDamage(damage);

        // Assert
        Assert.False(combatant.IsDefeated);
    }
    #endregion
}
