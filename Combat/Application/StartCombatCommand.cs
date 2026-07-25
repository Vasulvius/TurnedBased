using System.Collections.Generic;
using Combat.Domain;

namespace Combat.Application
{
    public record StartCombatCommand
    {
        public CombatantBlueprint[] CombatantStats { get; }

        public StartCombatCommand((int Health, int AttackPower, int Defense)[] combatantStats)
        {
            List<CombatantBlueprint> stats = new List<CombatantBlueprint>();
            foreach ((int Health, int AttackPower, int Defense) t in combatantStats)
            {
                stats.Add(
                    new CombatantBlueprint(
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
