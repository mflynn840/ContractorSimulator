# Multiplayer Sync (MVP 3.4)

## Components

| Component | Purpose |
|-----------|---------|
| `NetworkRepairable` | Server-authoritative repair progress (`NetworkVariable`) |
| `RepairableSurface` | `IToolUsable` bridge; sends one ServerRpc per tool press |
| `RepairProgressVisual` | Updates wall color from synced progress |
| `NetworkInteractable` | Server-authoritative generic interactable state |
| `NetworkPlayerToolController` | Syncs equipped tool + tool-use timing |
| `NetworkSceneGameplayInitializer` | Server spawns `PF_RepairWall` in gameplay scenes |

## Replication rules

- **State:** `NetworkVariable` for repair progress and completion (no per-frame RPCs).
- **Actions:** discrete `ServerRpc` on tool/interact input with client-side throttling.
- **Authority:** server validates distance before applying repair/interaction.

## Two-player test

1. Open Bootstrap scene, enter Play Mode, **Host Game**.
2. Build or ParrelSync clone → **Join Game** at `127.0.0.1`.
3. Host clicks **Start Session** with 2 players connected.
4. Both walk to the repair wall (z ≈ 4).
5. Hold/use **Use Tool** binding — both should see progress % and color sync.
6. At 100%, wall is marked complete on both clients.

Input: Use Tool action from `InputSystem_Actions` (bound in Starter Assets defaults).
