using System;

namespace Combat.Domain
{
    public sealed record AttackPower
    {
        public int Value { get; }

        private AttackPower(int value)
        {
            if (value < 0)
            {
                throw new ArgumentException("AttackPower must not be negative", nameof(value));
            }

            Value = value;
        }

        public static AttackPower Create(int value)
        {
            return new AttackPower(value);
        }

        public Damage ToDamage()
        {
            return Damage.Create(Value);
        }
    }
}
