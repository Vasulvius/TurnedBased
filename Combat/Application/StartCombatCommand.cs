using System.Collections.Generic;
using Combat.Domain;

namespace Combat.Application
{
    public record StartCombatCommand
    {
        public CombatantBluePrint[] CombatantStats { get; }

        public StartCombatCommand((int Health, int AttackPower, int Defense)[] combatantStats)
        {
            List<CombatantBluePrint> stats = new List<CombatantBluePrint>();
            foreach ((int Health, int AttackPower, int Defense) t in combatantStats)
            {
                stats.Add(
                    new CombatantBluePrint(
                        Health.Create(t.Health),
                        AttackPower.Create(t.AttackPower),
                        Defense.Create(t.Defense)
                    )
                );
            }
            CombatantStats = stats.ToArray();
        }
    }
}
