using System.Collections.Generic;
using BuildingBlocks;

namespace Combat.Domain
{
    public abstract record ActionResult;

    public sealed record ActionRejected(string Reason) : ActionResult;

    public sealed record ActionApplied(IReadOnlyList<DomainEvent> Events) : ActionResult;
}
