using System.Collections.Generic;
using BuildingBlocks;

namespace Combat.Domain
{
    public abstract record ActionResult;

    public sealed record ActionApplied(IReadOnlyList<DomainEvent> Events) : ActionResult;

    public sealed record ActionRejected(RejectionReason Reason) : ActionResult;

    public enum RejectionReason
    {
        NotCurrentCombatant,
        TargetNotFound,
        TargetDefeated,
        CombatFinished,
    }
}
