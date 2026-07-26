namespace Combat.Domain
{
    public sealed record CombatantSnapshot
    {
        public CombatantId Id { get; }
        public int MaxHealth { get; }
        public int CurrentHealth { get; }

        public CombatantSnapshot(Combatant combatant)
        {
            Id = combatant.Id;
            MaxHealth = combatant.Health.Max;
            CurrentHealth = combatant.Health.Current;
        }
    }
}
