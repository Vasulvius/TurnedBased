using BuildingBlocks;

namespace Combat.Domain
{
    public sealed class Combatant : Entity<CombatantId>
    {
        public Health Health { get; private set; }
        public AttackPower AttackPower { get; }
        public Defense Defense { get; }
        public bool IsDefeated => Health.IsDepleted;

        public Combatant(CombatantBlueprint bluePrint)
            : base(bluePrint.Id)
        {
            Health = bluePrint.Health;
            AttackPower = bluePrint.AttackPower;
            Defense = bluePrint.Defense;
        }

        public Damage TakeDamage(Damage rawDamage)
        {
            Damage netDamage = rawDamage.Reduce(Defense);
            Health = Health.Reduce(netDamage);
            return netDamage;
        }
    }
}
