using BuildingBlocks;

namespace Combat.Domain.Events
{
    public sealed record DamageTaken(CombatantId Target, Damage Amount, int RemainingHealth)
        : DomainEvent;
}
