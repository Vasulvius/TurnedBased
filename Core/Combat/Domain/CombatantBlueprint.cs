namespace Combat.Domain
{
    public record CombatantBlueprint(
        CombatantId Id,
        Health Health,
        AttackPower AttackPower,
        Defense Defense
    );
}
