using System;

namespace Combat.Domain
{
    public sealed class Combatant : IEquatable<Combatant>
    {
        public CombatantId Id { get; }
        public Health Health { get; private set; }
        public AttackPower AttackPower { get; }
        public Defense Defense { get; }
        public bool IsDefeated => Health.IsDepleted;

        public Combatant(Health health, AttackPower attackPower, Defense defense)
        {
            Id = CombatantId.Create();
            Health = health;
            AttackPower = attackPower;
            Defense = defense;
        }

        public bool Equals(Combatant? other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Id == other.Id;
        }

        public override bool Equals(object? obj) => Equals(obj as Combatant);

        public override int GetHashCode() => Id.GetHashCode();

        public static bool operator ==(Combatant? left, Combatant? right)
        {
            if (left is null)
                return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(Combatant? left, Combatant? right) => !(left == right);

        public Damage TakeDamage(Damage rawDamage)
        {
            Damage netDamage = rawDamage.Reduce(Defense);
            Health = Health.Reduce(netDamage);
            return netDamage;
        }
    }
}
