using System;
using System.Collections.Generic;
using System.Linq;
using BuildingBlocks;
using Combat.Domain.Events;

namespace Combat.Domain
{
    public sealed class Combat : IEquatable<Combat>
    {
        public CombatId Id { get; }
        public TurnOrder TurnOrder { get; private set; }
        private readonly IReadOnlyDictionary<CombatantId, Combatant> _combatants;
        private bool _isCombatFinished
        {
            get
            {
                int nbLiving = 0;
                foreach (Combatant combatant in _combatants.Values.ToArray())
                {
                    if (!combatant.IsDefeated)
                        nbLiving += 1;
                }
                return nbLiving == 1;
            }
        }

        public Combat(Combatant[] combatants)
        {
            Dictionary<CombatantId, Combatant> tempCombatants =
                new Dictionary<CombatantId, Combatant>();
            foreach (Combatant combatant in combatants)
            {
                tempCombatants.Add(combatant.Id, combatant);
            }

            Id = CombatId.Create();
            TurnOrder = TurnOrder.Create(tempCombatants.Keys.ToArray());
            _combatants = tempCombatants;
        }

        public ActionResult ExecuteAction(CombatantId from, Action action)
        {
            var events = new List<DomainEvent>();
            if (TurnOrder.CurrentCombatant != from)
            {
                return new ActionRejected("Action not possible, it is not the current combatant.");
            }
            if (_isCombatFinished)
            {
                return new ActionRejected("Action not possible, the combat is finished.");
            }
            switch (action)
            {
                case Attack attack:
                    if (!_combatants.TryGetValue(attack.TargetId, out Combatant? target))
                        return new ActionRejected("Cannot attack a null target.");
                    if (target.IsDefeated)
                        return new ActionRejected("Cannot attack a dead combatant.");
                    Damage raw = _combatants[from].AttackPower.ToDamage();

                    Damage net = target.TakeDamage(raw);
                    events.Add(new DamageTaken(target.Id, net, target.Health.Current));

                    if (target.IsDefeated)
                        events.Add(new CombatantDied(target.Id));
                    if (_isCombatFinished)
                    {
                        CombatantId[] winners = _combatants
                            .Values.Where(combatant => !combatant.IsDefeated)
                            .Select(combatant => combatant.Id)
                            .ToArray();
                        events.Add(new CombatEnded(winners));
                        break;
                    }
                    PassToNextTurn();
                    break;
                default:
                    return new ActionRejected("Unknown type of action.");
            }

            return new ActionApplied(events);
        }

        private void PassToNextTurn()
        {
            TurnOrder turnOrderCandidate = TurnOrder.Next();
            while (_combatants[turnOrderCandidate.CurrentCombatant].IsDefeated)
            {
                turnOrderCandidate = turnOrderCandidate.Next();
            }
            TurnOrder = turnOrderCandidate;
        }

        public bool Equals(Combat? other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Id == other.Id;
        }

        public override bool Equals(object? obj) => Equals(obj as Combat);

        public override int GetHashCode() => Id.GetHashCode();

        public static bool operator ==(Combat? left, Combat? right)
        {
            if (left is null)
                return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(Combat? left, Combat? right) => !(left == right);
    }
}
