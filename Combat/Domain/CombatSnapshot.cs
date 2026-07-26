using System.Collections.Generic;

namespace Combat.Domain
{
    public sealed record CombatSnapshot
    {
        public CombatantSnapshot[] Combatants { get; }

        public CombatSnapshot(Combatant[] combatants)
        {
            List<CombatantSnapshot> combatantsSnapshot = new List<CombatantSnapshot>();
            foreach (Combatant combatant in combatants)
            {
                combatantsSnapshot.Add(new CombatantSnapshot(combatant));
            }

            Combatants = combatantsSnapshot.ToArray();
        }
    }
}
