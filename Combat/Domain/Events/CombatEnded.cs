using BuildingBlocks;

namespace Combat.Domain.Events
{
    public sealed record CombatEnded(CombatantId[] Winners) : DomainEvent;
}
