namespace Combat.Domain
{
    public abstract record Action;

    public sealed record Attack(CombatantId TargetId) : Action;
}
