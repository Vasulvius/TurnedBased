using BuildingBlocks;
using Combat.Domain;
using Combat.Domain.Events;

namespace Tests;

public class CombatTest
{
    private sealed record CombatantSpec(
        int Hp = 100,
        int Atk = 10,
        int Def = 5,
        CombatantId? Id = null
    );

    private static Combat.Domain.Combat MakeCombat(CombatantSpec[]? combatantSpecs = null)
    {
        combatantSpecs ??= [new CombatantSpec(), new CombatantSpec()];

        List<Combatant> combatants = [];
        foreach (CombatantSpec combatantSpec in combatantSpecs)
        {
            combatants.Add(
                new Combatant(
                    new CombatantBlueprint(
                        combatantSpec.Id ?? CombatantId.Create(),
                        Health.Create(combatantSpec.Hp),
                        AttackPower.Create(combatantSpec.Atk),
                        Defense.Create(combatantSpec.Def)
                    )
                )
            );
        }

        return new Combat.Domain.Combat(combatants.ToArray());
    }

    #region ActionRejected
    [Fact]
    public void Combat_refuse_that_a_combatant_to_act_if_it_is_not_it_turn()
    {
        // Given
        var id1 = CombatantId.Create();
        var id2 = CombatantId.Create();
        var combat = MakeCombat([new CombatantSpec(Id: id1), new CombatantSpec(Id: id2)]);

        // When
        var result = combat.ExecuteAction(id2, new Attack(id1));

        // then
        Assert.Equal(new ActionRejected(RejectionReason.NotCurrentCombatant), result);
    }

    [Fact]
    public void Combat_refuse_that_a_combatant_to_act_if_combat_is_finished()
    {
        // Given
        var id1 = CombatantId.Create();
        var id2 = CombatantId.Create();
        var combat = MakeCombat([
            new CombatantSpec(Id: id1, Atk: 100),
            new CombatantSpec(Id: id2, Hp: 1, Def: 0),
        ]);
        combat.ExecuteAction(id1, new Attack(id2)); // kill combatant_2 and then give initative to combatant_1

        // When
        var result = combat.ExecuteAction(id1, new Attack(id2));

        // then
        Assert.Equal(new ActionRejected(RejectionReason.CombatFinished), result);
    }

    [Fact]
    public void Combat_refuse_that_a_combatant_target_an_unknown_combatant()
    {
        // Given
        var id1 = CombatantId.Create();
        var unknownId = CombatantId.Create();
        var combat = MakeCombat([new CombatantSpec(Id: id1), new CombatantSpec()]);

        // When
        var result = combat.ExecuteAction(id1, new Attack(unknownId));

        // then
        Assert.Equal(new ActionRejected(RejectionReason.TargetNotFound), result);
    }

    [Fact]
    public void Combat_refuse_that_a_combatant_target_a_dead_combatant()
    {
        // Given
        var id1 = CombatantId.Create();
        var id2 = CombatantId.Create();
        var id3 = CombatantId.Create();
        var combat = MakeCombat([
            new CombatantSpec(Id: id1, Atk: 10),
            new CombatantSpec(Id: id2, Hp: 1, Def: 0),
            new CombatantSpec(Id: id3),
        ]);
        combat.ExecuteAction(id1, new Attack(id2)); // combatant_1 kills combatant_2, then it is combatant_3's turn

        // When
        var result = combat.ExecuteAction(id3, new Attack(id2));

        // then
        Assert.Equal(new ActionRejected(RejectionReason.TargetDefeated), result);
    }
    #endregion

    #region Nominal path
    [Fact]
    public void Attack_produce_valid_DamageTaken_followed_by_the_next_turn()
    {
        // Given
        var id1 = CombatantId.Create();
        var id2 = CombatantId.Create();
        var combat = MakeCombat([
            new CombatantSpec(Id: id1, Atk: 10),
            new CombatantSpec(Id: id2, Hp: 100, Def: 0),
        ]);

        // When
        var result = combat.ExecuteAction(id1, new Attack(id2));

        // Then
        var applied = Assert.IsType<ActionApplied>(result);
        DomainEvent[] expected =
        {
            new DamageTaken(id2, Damage.Create(10), 90),
            new TurnStarted(id2),
        };

        Assert.Equal(expected, applied.Events);
    }
    #endregion

    #region Death and Victory
    [Fact]
    public void Lethal_attack_must_end_combat_properly()
    {
        // Given
        var id1 = CombatantId.Create();
        var id2 = CombatantId.Create();
        var combat = MakeCombat([
            new CombatantSpec(Id: id1, Atk: 100),
            new CombatantSpec(Id: id2, Hp: 1, Def: 0),
        ]);

        // When
        var result = combat.ExecuteAction(id1, new Attack(id2));

        // Then
        var applied = Assert.IsType<ActionApplied>(result);
        Assert.Equal(3, applied.Events.Count);

        var damage = Assert.IsType<DamageTaken>(applied.Events[0]);
        Assert.Equal(new DamageTaken(id2, Damage.Create(100), 0), damage);

        var died = Assert.IsType<CombatantDied>(applied.Events[1]);
        Assert.Equal(new CombatantDied(id2), died);

        var end = Assert.IsType<CombatEnded>(applied.Events[2]);
        Assert.Single(end.Winners);
        Assert.Equal(id1, end.Winners[0]);
    }
    #endregion

    #region GetSnapshot
    [Fact]
    public void GetSnapshot_is_correct_after_combat_instantiation()
    {
        // Given
        var id1 = CombatantId.Create();
        var id2 = CombatantId.Create();
        var combat = MakeCombat([
            new CombatantSpec(Id: id1, Hp: 100, Atk: 10, Def: 5),
            new CombatantSpec(Id: id2, Hp: 100, Atk: 10, Def: 5),
        ]);

        // When
        var snapshot = combat.GetSnapshot();

        // Then
        Assert.Equal(2, snapshot.Combatants.Length);

        var c1 = snapshot.Combatants.First(c => c.Id == id1);
        Assert.Equal(100, c1.MaxHealth);
        Assert.Equal(100, c1.CurrentHealth);

        var c2 = snapshot.Combatants.First(c => c.Id == id2);
        Assert.Equal(100, c2.MaxHealth);
        Assert.Equal(100, c2.CurrentHealth);
    }

    [Fact]
    public void GetSnapshot_is_correct_after_attack()
    {
        // Given
        var id1 = CombatantId.Create();
        var id2 = CombatantId.Create();
        var combat = MakeCombat([
            new CombatantSpec(Id: id1, Hp: 100, Atk: 10, Def: 5),
            new CombatantSpec(Id: id2, Hp: 100, Atk: 10, Def: 5),
        ]);

        // When
        combat.ExecuteAction(id1, new Attack(id2));
        var snapshot = combat.GetSnapshot();

        // Then
        Assert.Equal(2, snapshot.Combatants.Length);

        var c1 = snapshot.Combatants.First(c => c.Id == id1);
        Assert.Equal(100, c1.MaxHealth);
        Assert.Equal(100, c1.CurrentHealth);

        var c2 = snapshot.Combatants.First(c => c.Id == id2);
        Assert.Equal(100, c2.MaxHealth);
        Assert.Equal(95, c2.CurrentHealth);
    }
    #endregion
}
