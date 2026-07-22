using BuildingBlocks;

namespace Combat.Domain.Events
{
    public sealed record TurnStarted(CombatantId Combatant) : DomainEvent;
}
