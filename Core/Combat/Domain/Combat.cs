using System;
using System.Collections.Generic;
using System.Linq;
using BuildingBlocks;
using Combat.Domain.Events;

namespace Combat.Domain
{
    public sealed class Combat : Entity<CombatId>
    {
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
            : base(CombatId.Create())
        {
            Dictionary<CombatantId, Combatant> tempCombatants =
                new Dictionary<CombatantId, Combatant>();
            foreach (Combatant combatant in combatants)
            {
                tempCombatants.Add(combatant.Id, combatant);
            }

            TurnOrder = TurnOrder.Create(tempCombatants.Keys.ToArray());
            _combatants = tempCombatants;
        }

        public CombatSnapshot GetSnapshot()
        {
            return new CombatSnapshot(_combatants.Values.ToArray());
        }

        public ActionResult ExecuteAction(CombatantId from, Action action)
        {
            var events = new List<DomainEvent>();
            if (TurnOrder.CurrentCombatant != from)
            {
                return new ActionRejected(RejectionReason.NotCurrentCombatant);
            }
            if (_isCombatFinished)
            {
                return new ActionRejected(RejectionReason.CombatFinished);
            }

            return action switch
            {
                Attack attack => HandleAttack(from, attack),
                _ => throw new NotSupportedException($"Unhandled action: {action.GetType().Name}"),
            };
        }

        private ActionResult HandleAttack(CombatantId from, Attack attack)
        {
            List<DomainEvent> events = [];
            if (!_combatants.TryGetValue(attack.TargetId, out Combatant? target))
                return new ActionRejected(RejectionReason.TargetNotFound);
            if (target.IsDefeated)
                return new ActionRejected(RejectionReason.TargetDefeated);
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
                return new ActionApplied(events);
            }
            PassToNextTurn();
            events.Add(new TurnStarted(TurnOrder.CurrentCombatant));
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
    }
}
