using Combat.Domain;

namespace Combat.Application
{
    public sealed record AttackCommand
    {
        public CombatantId Attacker { get; }
        public CombatantId Target { get; }

        public AttackCommand(CombatantId attacker, CombatantId target)
        {
            Attacker = attacker;
            Target = target;
        }
    }
}
