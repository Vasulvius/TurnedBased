using System;

namespace Combat.Domain
{
    public sealed record Damage
    {
        public int Value { get; }

        private Damage(int value)
        {
            if (value < 0)
            {
                throw new ArgumentException("Damage must not be negative", nameof(value));
            }

            Value = value;
        }

        public static Damage Create(int value)
        {
            return new Damage(value);
        }

        public Damage Reduce(Defense defense)
        {
            return new Damage(Math.Max(Value - defense.Value, 0));
        }
    }
}
