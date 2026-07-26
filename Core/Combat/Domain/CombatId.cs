using System;

namespace Combat.Domain
{
    public sealed record CombatId
    {
        public Guid Value { get; }

        private CombatId(Guid value)
        {
            Value = value;
        }

        public static CombatId Create()
        {
            return new CombatId(Guid.NewGuid());
        }
    }
}
