using System;
using System.Collections.Generic;
using Combat.Domain;

namespace Combat.Application
{
    public class CombatService
    {
        private Domain.Combat? _combat;

        public Domain.Combat StartCombat(StartCombatCommand cmd)
        {
            List<Combatant> combatants = new List<Combatant>();
            foreach (CombatantBluePrint stats in cmd.CombatantStats)
            {
                combatants.Add(new Combatant(stats));
            }

            Domain.Combat combat = new Domain.Combat(combatants.ToArray());
            _combat = combat;
            return combat;
        }

        public ActionResult Attack(AttackCommand cmd)
        {
            if (_combat is null)
            {
                throw new NullReferenceException("Combat never started.");
            }
            ActionResult attackResult = _combat.ExecuteAction(cmd.Attacker, new Attack(cmd.Target));

            if (attackResult is ActionApplied)
            {
                // TODO: publish event
            }

            return attackResult;
        }
    }
}
