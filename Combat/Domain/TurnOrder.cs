using System;
using System.Linq;

namespace Combat.Domain
{
    public sealed record TurnOrder
    {
        public CombatantId[] Combatants { get; }

        public CombatantId CurrentCombatant { get; }

        private TurnOrder(CombatantId[] combatants, CombatantId currentCombatant)
        {
            if (combatants.Length < 2)
            {
                throw new ArgumentException(
                    "Combatant list must be contain minimum 2 ids",
                    nameof(combatants)
                );
            }
            if (!combatants.Contains(currentCombatant))
            {
                throw new ArgumentException(
                    "CurrentCombatant must be in Combatant list",
                    nameof(currentCombatant)
                );
            }
            Combatants = combatants;
            CurrentCombatant = currentCombatant;
        }

        public static TurnOrder Create(CombatantId[] combatants, CombatantId currentCombatant)
        {
            return new TurnOrder(combatants, currentCombatant);
        }

        public static TurnOrder Create(CombatantId[] combatants)
        {
            return new TurnOrder(combatants, combatants[0]);
        }

        public TurnOrder Next()
        {
            int index = Array.IndexOf(Combatants, CurrentCombatant);
            if (index == -1)
                throw new ArgumentException("This combatant is not on the list.");

            int nextIndex = (index + 1) % Combatants.Length;
            return TurnOrder.Create(Combatants, Combatants[nextIndex]);
        }
    }
}
