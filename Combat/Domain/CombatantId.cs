using System;

namespace Combat.Domain
{
    public sealed record CombatantId
    {
        public Guid Value { get; }

        private CombatantId(Guid value)
        {
            Value = value;
        }

        public static CombatantId Create()
        {
            return new CombatantId(Guid.NewGuid());
        }
    }
}
