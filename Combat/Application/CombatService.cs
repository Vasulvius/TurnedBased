using System;
using System.Collections.Generic;
using BuildingBlocks;
using Combat.Domain;

namespace Combat.Application
{
    public class CombatService
    {
        public event EventHandler<DomainEvent>? CombatEvent;
        private Domain.Combat? _combat;

        public CombatSnapshot GetView()
        {
            if (_combat is null)
            {
                throw new InvalidOperationException("Combat never started.");
            }
            return _combat.GetSnapshot();
        }

        public void StartCombat(StartCombatCommand cmd)
        {
            List<Combatant> combatants = new List<Combatant>();
            foreach (CombatantBlueprint stats in cmd.CombatantStats)
            {
                combatants.Add(new Combatant(stats));
            }

            Domain.Combat combat = new Domain.Combat(combatants.ToArray());
            _combat = combat;
        }

        public ActionResult Attack(AttackCommand cmd)
        {
            if (_combat is null)
            {
                throw new InvalidOperationException("Combat never started.");
            }
            ActionResult attackResult = _combat.ExecuteAction(cmd.Attacker, new Attack(cmd.Target));

            if (attackResult is ActionApplied applied)
            {
                foreach (DomainEvent evt in applied.Events)
                {
                    CombatEvent?.Invoke(this, evt);
                }
            }

            return attackResult;
        }
    }
}
