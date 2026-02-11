# AGENTS.md - Godot C# Project Guide

This guide provides essential information for AI coding agents working in this Godot C# codebase.

## Agent Behavior

**CRITICAL: NEVER modify code directly unless DIRECTLY instructed to do so by the user.**

AI agents working in this codebase should:
- Ask for clarification before making code changes
- Explain what changes would be needed and wait for explicit approval
- Only write or modify code when the user explicitly requests it
- Default to reading, analyzing, and suggesting rather than implementing

## Project Overview

- **Engine:** Godot 4.5.1 with C# scripting
- **Framework:** .NET 9.0
- **Type:** 2D game using state machines, object pooling, and event-driven architecture
- **Executable:** `godot-mono` (not standard `godot`)

## Build & Run Commands

### Build
```bash
# Build the project (default: Debug configuration)
dotnet build

# Build with specific configuration
dotnet build --configuration Debug
dotnet build --configuration ExportDebug
dotnet build --configuration ExportRelease
```

### Format Code
```bash
# Restore tools and format all C# files
dotnet tool restore && dotnet csharpier .
```

### Run
```bash
# Launch Godot editor
godot-mono

# Run headless (for CI/testing)
godot-mono --headless
```

### Testing
**No test framework is configured.** All testing must be done manually in the Godot editor.

## Project Structure

```
GdcWinter/
├── src/                    # All C# source code
│   ├── Player/            # Player-related classes
│   ├── Obstacles/         # Obstacle system
│   ├── Weapons/           # Weapon and projectile system
│   ├── Pickups/           # Pickup items
│   ├── Shaders/           # Custom shader files
│   └── *.cs               # Core game systems
├── scenes/                # Godot scene files (.tscn)
├── assets/                # Game assets (sprites, audio, etc.)
└── addons/                # Godot plugins

Note: Namespaces do NOT match folder structure (IDE0130 disabled in .editorconfig)
```

## Code Style Guidelines

### Imports & Organization

**Global Usings:** `System` and `Godot` are globally imported in `src/GlobalUsing.cs`

```csharp
// Already available globally (no need to import):
// - using System;
// - using Godot;

// Import explicitly when needed:
using System.Collections.Generic;
using System.Linq;
using Godot.Collections;
using Game.Weapons;

// Use file-scoped namespaces
namespace Game.Players;

public partial class Player : Node2D
{
    // ...
}
```

### Naming Conventions

- **PascalCase:** Classes, methods, public properties, namespaces
  - `Player`, `ObstacleManager`, `HandleHit()`, `StatController`
- **_camelCase:** Private fields (underscore prefix REQUIRED)
  - `_enabled`, `_spawnTimer`, `_groundSpawnPoint`
- **ALL_CAPS:** Constants
  - `MAX_HEALTH`, `DEFAULT_SPEED`

### Classes & Types

**Partial Classes:** The `partial` modifier is REQUIRED for all Godot node classes.

```csharp
// CORRECT: partial modifier for Godot nodes
public partial class Player : Node2D
{
    // ...
}

// WRONG: missing partial
public class Player : Node2D
{
    // ...
}
```

**Type Usage:**
- Prefer explicit types over `var` when not obvious
- Nullable reference types are not strictly enforced
- Use interfaces for polymorphism: `IHittable`, `IState`, `IPickupable`, `IObstacle`

### Godot-Specific Patterns

#### Attributes on Separate Lines

**ALWAYS place attributes on a separate line above the declaration:**

```csharp
// CORRECT: Attribute on separate line
[Export]
private Node2D _groundSpawnPoint;

[Export]
private float _spawnTimeMin = 0;

[ExportCategory("Properties")]
[Export]
private bool _enabled = true;

// WRONG: Inline attribute
[Export] private Node2D _groundSpawnPoint;
```

#### Prefer [Export] Over GetNode()

**STRONGLY prefer `[Export]` references set in the Godot inspector over `GetNode()` calls:**

```csharp
// GOOD: Export references (set in inspector)
[Export]
private AnimatedSprite2D _sprite;

[Export]
private Node2D _groundSpawnPoint;

[Export]
private StatController _statController;

// AVOID: GetNode() calls
var sprite = GetNode<AnimatedSprite2D>("Sprite");
var spawnPoint = GetNode<Node2D>("SpawnPoint");

// EXCEPTION: Autoload singletons are OK
GetNode("/root/DebugMenu").Set("style", 2);
GetNode("/root/Game");
```

#### Signals

Use the `[Signal]` attribute with delegate pattern for custom events:

```csharp
[Signal]
public delegate void OnStateChangeEventHandler(State newState);

[Signal]
public delegate void OnPlayerDeathEventHandler();

// Emit signals like this:
EmitSignal(SignalName.OnStateChange, CurrentState);
```

#### Lifecycle Methods

Override these methods for Godot node lifecycle:

```csharp
public override void _EnterTree()
{
    // Called when node enters scene tree
}

public override void _Ready()
{
    // Called when node is ready (after children)
}

public override void _Process(double delta)
{
    // Called every frame
}

public override void _PhysicsProcess(double delta)
{
    // Called every physics frame (fixed timestep)
}
```

### Error Handling & Logging

**Use the custom `Logger` class** (not `GD.Print`):

```csharp
Logger.LogInfo("Informational message");
Logger.LogDebug("Debug details");
Logger.LogWarning("Warning message");
Logger.LogError("Error message");
```

**Prefer early returns with logging over exceptions:**

```csharp
public void ChangeState<T>() where T : State
{
    var newState = _states.OfType<T>().FirstOrDefault();
    if (newState is null)
    {
        Logger.LogError("Changing state with state type failed. State not found");
        return; // Early return instead of throwing
    }
    
    // Continue with valid state...
}
```

### Comments & Documentation

- Use inline `TODO:`, `BUG:`, `NOTE:` comments where helpful
- Attribute external code sources (see `Utils.cs` for example)
- Minimal XML documentation - prefer self-documenting code
- Keep commented-out code if it provides context (e.g., performance notes)

```csharp
// BUG: Performance issues. Shuffle is O(n) and RemoveAt is O(n) at worse case.
public void SpawnObstacle()
{
    // ...
}

// Source attribution example:
// Source - https://stackoverflow.com/a/xxxxx
// Posted by username
// Retrieved 2025-12-23, License - CC BY-SA 2.5
```

## Common Patterns

### State Machine Pattern

Uses Lock/Unlock mechanism to prevent state conflicts:

```csharp
public void Lock(object locker)
{
    _isLocked = true;
    _lockedBy = locker;
}

public void Unlock(object locker)
{
    _isLocked = false;
    _lockedBy = null;
}
```

### Object Pooling

Toggle `ProcessMode` instead of destroying/instantiating objects:

```csharp
// Disable pooled object
obstacle.ProcessMode = ProcessModeEnum.Disabled;

// Re-enable pooled object
obstacle.ProcessMode = ProcessModeEnum.Inherit;
```

### Deferred Calls

**REQUIRED for scene tree and physics modifications:**

```csharp
// Use CallDeferred for scene tree changes
Callable.From(() =>
{
    obstacle.ProcessMode = ProcessModeEnum.Disabled;
}).CallDeferred();

// Can't disable without deferring during physics
obstacle.OnExit += () =>
{
    Callable.From(() =>
    {
        obstacle.ProcessMode = ProcessModeEnum.Disabled;
    }).CallDeferred();
};
```

### Event-Driven Architecture

Combine C# events with Godot signals:

```csharp
// C# event for internal communication
GameWorld.Instance.OnPlayerDeath += () =>
{
    _enabled = false;
    _spawnTimer.Stop();
};

// Godot signal for editor connections
EmitSignal(SignalName.OnStateChange, CurrentState);
```

### Singleton Pattern

Use for global game state access:

```csharp
public override void _EnterTree()
{
    GameWorld.Instance.MainPlayer = this;
}
```

### Godot Collections

Use Godot collection types for exported properties:

```csharp
[Export]
private Godot.Collections.Array<State> _states = [];

[Export]
private Godot.Collections.Dictionary<PackedScene, int> _obstacleTypes = [];
```

## Important Reminders

1. **Always use `partial`** for all classes inheriting from Godot nodes
2. **Prefer `[Export]` references** over `GetNode()` calls (except autoloads)
3. **Attributes go on separate lines** above declarations
4. **Use `CallDeferred()`** for scene tree modifications during physics/signals
5. **Toggle ProcessMode** for object pooling (don't destroy/recreate)
6. **Use custom `Logger` class** instead of `GD.Print()`
7. **No automated tests** - all testing is manual in the Godot editor
8. **Namespaces don't match folders** - this is intentional (IDE0130 disabled)
9. **Use `godot-mono`** as the executable, not standard `godot`
10. **Format with CSharpier** before committing

---

**Happy coding! This guide should help you navigate and contribute to this Godot C# project effectively.**
