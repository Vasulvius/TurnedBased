using BuildingBlocks;

namespace Combat.Domain.Events
{
    public sealed record CombatantDied(CombatantId Combatant) : DomainEvent;
}
