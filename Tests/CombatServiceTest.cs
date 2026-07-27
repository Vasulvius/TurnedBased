using BuildingBlocks;
using Combat.Application;
using Combat.Domain;
using Combat.Domain.Events;

namespace Tests;

public class CombatServiceTest
{
    #region Nominal path
    [Fact]
    public void CombatService_must_start_combat_properly()
    {
        // Given
        var service = new CombatService();
        var id1 = CombatantId.Create();
        var id2 = CombatantId.Create();
        service.StartCombat(new StartCombatCommand([(id1, 100, 10, 5), (id2, 80, 20, 3)]));

        // When
        var snapshot = service.GetView();

        // Then
        Assert.Equal(2, snapshot.Combatants.Length);

        var c1 = snapshot.Combatants.First(c => c.Id == id1);
        Assert.Equal(100, c1.MaxHealth);
        Assert.Equal(100, c1.CurrentHealth);

        var c2 = snapshot.Combatants.First(c => c.Id == id2);
        Assert.Equal(80, c2.MaxHealth);
        Assert.Equal(80, c2.CurrentHealth);
    }

    [Fact]
    public void Attack_must_republish_correct_events_for_non_lethal_attack()
    {
        // Given
        var service = new CombatService();
        var id1 = CombatantId.Create();
        var id2 = CombatantId.Create();
        service.StartCombat(new StartCombatCommand([(id1, 100, 10, 5), (id2, 100, 10, 0)]));

        var captured = new List<DomainEvent>();
        service.CombatEvent += (sender, evt) => captured.Add(evt);

        // When
        service.Attack(new AttackCommand(id1, id2));

        // Then
        Assert.Equal(2, captured.Count);
        Assert.IsType<DamageTaken>(captured[0]);
        Assert.IsType<TurnStarted>(captured[1]);
    }

    [Fact]
    public void Attack_must_return_ActionApplied_for_non_lethal_attack()
    {
        // Given
        var service = new CombatService();
        var id1 = CombatantId.Create();
        var id2 = CombatantId.Create();
        service.StartCombat(new StartCombatCommand([(id1, 100, 10, 5), (id2, 100, 10, 0)]));

        // When
        var result = service.Attack(new AttackCommand(id1, id2));

        // Then
        Assert.IsType<ActionApplied>(result);
    }

    [Fact]
    public void Attack_must_republish_correct_events_for_lethal_attack()
    {
        // Given
        var service = new CombatService();
        var id1 = CombatantId.Create();
        var id2 = CombatantId.Create();
        service.StartCombat(new StartCombatCommand([(id1, 100, 100, 5), (id2, 1, 10, 0)]));

        var captured = new List<DomainEvent>();
        service.CombatEvent += (sender, evt) => captured.Add(evt);

        // When
        service.Attack(new AttackCommand(id1, id2));

        // Then
        Assert.Equal(3, captured.Count);
        Assert.IsType<DamageTaken>(captured[0]);
        Assert.IsType<CombatantDied>(captured[1]);
        Assert.IsType<CombatEnded>(captured[2]);
    }

    [Fact]
    public void Attack_must_return_ActionApplied_for_lethal_attack()
    {
        // Given
        var service = new CombatService();
        var id1 = CombatantId.Create();
        var id2 = CombatantId.Create();
        service.StartCombat(new StartCombatCommand([(id1, 100, 100, 5), (id2, 1, 10, 0)]));

        // When
        var result = service.Attack(new AttackCommand(id1, id2));

        // Then
        Assert.IsType<ActionApplied>(result);
    }
    #endregion

    #region Action Rejected
    [Fact]
    public void Attack_must_republish_nothing_for_incorrect_action()
    {
        // Given
        var service = new CombatService();
        var id1 = CombatantId.Create();
        var id2 = CombatantId.Create();
        service.StartCombat(new StartCombatCommand([(id1, 100, 10, 5), (id2, 100, 10, 0)]));

        var captured = new List<DomainEvent>();
        service.CombatEvent += (sender, evt) => captured.Add(evt);

        // When
        service.Attack(new AttackCommand(id2, id1)); // example of incorrect action

        // Then
        Assert.Empty(captured);
    }

    [Fact]
    public void Attack_must_return_ActionRejected_for_incorrect_action()
    {
        // Given
        var service = new CombatService();
        var id1 = CombatantId.Create();
        var id2 = CombatantId.Create();
        service.StartCombat(new StartCombatCommand([(id1, 100, 10, 5), (id2, 100, 10, 0)]));

        // When
        var result = service.Attack(new AttackCommand(id2, id1)); // example of incorrect action

        // Then
        Assert.IsType<ActionRejected>(result);
    }
    #endregion

    #region Exception throwing
    [Fact]
    public void Attack_must_throw_exception_while_attacking_before_combat_started()
    {
        // Given
        var service = new CombatService();
        var id1 = CombatantId.Create();
        var id2 = CombatantId.Create();

        // When
        var result = () => service.Attack(new AttackCommand(id1, id2));

        // Then
        Assert.Throws<InvalidOperationException>(result);
    }

    [Fact]
    public void GetView_must_throw_exception_while_getting_view_before_combat_started()
    {
        // Given
        var service = new CombatService();
        var id1 = CombatantId.Create();
        var id2 = CombatantId.Create();

        // When
        var result = () => service.GetView();

        // Then
        Assert.Throws<InvalidOperationException>(result);
    }
    #endregion
}
