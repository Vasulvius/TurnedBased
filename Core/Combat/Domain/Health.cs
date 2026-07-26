using System;

namespace Combat.Domain
{
    public sealed record Health
    {
        public int Max { get; }
        public int Current { get; }
        public bool IsDepleted => Current == 0;

        private Health(int max, int current)
        {
            if (max <= 0)
            {
                throw new ArgumentException("MaxHealth must be positive", nameof(max));
            }
            if (current < 0)
            {
                throw new ArgumentException("CurrentHealth must not be negative", nameof(current));
            }
            if (max < current)
            {
                throw new ArgumentException("MaxHealth must be higher or equal than CurrentHealth");
            }
            Max = max;
            Current = current;
        }

        public static Health Create(int health)
        {
            return new Health(health, health);
        }

        public Health Reduce(Damage damage)
        {
            return new Health(Max, Math.Max(Current - damage.Value, 0));
        }
    }
}
