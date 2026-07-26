using System;

namespace Combat.Domain
{
    public sealed record Defense
    {
        public int Value { get; }

        private Defense(int value)
        {
            if (value < 0)
            {
                throw new ArgumentException("Defense must not be negative", nameof(value));
            }

            Value = value;
        }

        public static Defense Create(int value)
        {
            return new Defense(value);
        }
    }
}
