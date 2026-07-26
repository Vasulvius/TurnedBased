using BuildingBlocks;
using Combat.Application;
using Combat.Domain;
using Combat.Domain.Events;
using Godot;

public partial class GodotCombat : Node
{
    [Export]
    private GodotPlayer _player = null!;

    [Export]
    private GodotEnemy _enemy = null!;

    [Export]
    private Label label = null!;
    private readonly CombatService _combatService = new CombatService();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        label.Visible = false;
        (CombatantId Id, int Health, int AttackPower, int Defense) player = (
            _player.Id,
            _player.Stats.MaxHealth,
            _player.Stats.AttackPower,
            _player.Stats.Defense
        );

        (CombatantId Id, int Health, int AttackPower, int Defense) enemy = (
            _enemy.Id,
            _enemy.Stats.MaxHealth,
            _enemy.Stats.AttackPower,
            _enemy.Stats.Defense
        );
        StartCombatCommand cmd = new StartCombatCommand([player, enemy]);
        _combatService.StartCombat(cmd);

        _combatService.CombatEvent += OnCombatEvent;
        _player.AttackRequested += OnPlayerAttackRequested;
        _enemy.AttackRequested += OnEnemyAttackRequested;
    }

    private void OnCombatEvent(object? sender, DomainEvent evt)
    {
        switch (evt)
        {
            case CombatantDied combatantDied:
                GD.Print(combatantDied);
                break;
            case CombatEnded combatEnded:
                GD.Print(combatEnded);
                _player.ChangeActionPanelVibility(false);
                bool playerWon = false;
                foreach (CombatantId winner in combatEnded.Winners)
                {
                    if (winner == _player.Id)
                    {
                        playerWon = true;
                    }
                }
                if (playerWon)
                {
                    label.Text = "Player won!";
                }
                else
                {
                    label.Text = "Player defeated!";
                }
                label.Visible = true;
                break;
            case DamageTaken damageTaken:
                GD.Print(damageTaken);
                if (damageTaken.Target == _player.Id)
                {
                    _player.UpdateHealth(damageTaken.RemainingHealth);
                }
                else if (damageTaken.Target == _enemy.Id)
                {
                    _enemy.UpdateHealth(damageTaken.RemainingHealth);
                }
                break;
            case TurnStarted turnStarted:
                GD.Print(turnStarted);
                _player.ChangeActionPanelVibility(turnStarted.Combatant == _player.Id);
                if (turnStarted.Combatant == _enemy.Id)
                {
                    _enemy.Attack();
                }
                break;
            default:
                GD.Print("Unknowned event: ", evt);
                break;
        }
    }

    private void OnPlayerAttackRequested()
    {
        _combatService.Attack(new AttackCommand(_player.Id, _enemy.Id));
    }

    private void OnEnemyAttackRequested()
    {
        _combatService.Attack(new AttackCommand(_enemy.Id, _player.Id));
    }

    public override void _ExitTree()
    {
        _combatService.CombatEvent -= OnCombatEvent;
        _player.AttackRequested -= OnPlayerAttackRequested;
        _enemy.AttackRequested -= OnEnemyAttackRequested;
        base._ExitTree();
    }
}
