# TurnedBased

A turn-based combat prototype (Godot 4.5 + C#/.NET 8), built with a **Domain-Driven Design** architecture. The business core is a pure C# project, with no dependency on the engine: the entire combat system — turns, damage, victory — could run in a console, without launching Godot.

## Stack

- **Godot 4.5.1** (.NET/Mono)
- **C# / .NET 8**
- Two projects: a pure business core, and a Godot presentation layer.

## Architecture

A single *bounded context*: **Combat**. The code is split into layers, with a strict dependency rule — outer layers depend on inner ones, never the other way around:

```
Presentation (Godot)  ──►  Application  ──►  Domain
     Godot/                 Core/Combat/Application   Core/Combat/Domain
```

### The inviolable rule

> The domain never depends on the engine.

This isn't a convention: it's **guaranteed by the compiler**. The business core lives in `Core/` (`TurnedBased.Core.csproj`), a `Microsoft.NET.Sdk` project that doesn't reference Godot. A `using Godot;` in the domain becomes a **compilation error**, not a possible oversight.

## Structure

```
Core/                         TurnedBased.Core.csproj — pure C#, zero Godot
  BuildingBlocks/             generic DDD building blocks (DomainEvent)
  Combat/
    Domain/                   aggregate, entity, value objects, events
    Application/              application service, commands
Godot/                        presentation (part of TurnedBased.csproj)
  *.cs                        Godot adapters (nodes, stats resource)
  *.tscn                      scenes
TurnedBased.csproj            Godot project (root), references Core
TurnedBased.sln
```

### The domain (`Core/Combat/Domain`)

- **`Combat`** — the aggregate root. The only entry point; it protects the invariants (whose turn it is, valid and alive target, end of combat).
- **`Combatant`** — entity, identified by a `CombatantId`, equal by identity.
- **Value objects** — `Health`, `Damage`, `AttackPower`, `Defense`, `TurnOrder`, `Action`, `ActionResult`, `CombatantBlueprint`: immutable, self-validating (an invalid state cannot be constructed).
- **Events** — `DamageTaken`, `CombatantDied`, `TurnStarted`, `CombatEnded`: past-tense facts, raised by the aggregate.
- **Projection** — `CombatSnapshot` / `CombatantSnapshot`: read-only, for display purposes.

### The flow

> Intentions come in through commands. Facts go out through events.

```
click / AI  ──►  AttackCommand  ──►  CombatService.Attack  ──►  Combat.ExecuteAction
                                                                     │
                        health bars, animations  ◄── DomainEvent ──┘  (C# pub/sub)
```

The human (button) and the AI are two symmetrical *drivers*: the domain has no idea which one decided the action.

## Getting started

Prerequisites: **.NET 8 SDK** and **Godot 4.5.1 (.NET)**.

```bash
# Build (also checks domain purity)
dotnet build
```

To play: open the project in Godot, then run the main scene (`Godot/CombatScene.tscn`).

To export: in the editor, *Project → Export*. `TurnedBased.Core.dll` is automatically bundled with the game.

## Quality

A pre-commit hook (`.githooks/pre-commit`) checks domain purity and that the project builds. Enable it once per clone:

```bash
git config core.hooksPath .githooks
```

## Coming up

- **Tests** — since the domain has no Godot dependency, it can be tested in pure C#, without launching the engine.
- Initiative mechanics, target selection, reasoning AI, save system.
