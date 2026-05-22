# Networking Architecture (MVP 3.1)

## Authority model

- **Listen-server host** for 2-player co-op (`NetworkAuthorityRules.UseListenServerHost`)
- **Server-authoritative** repair progress, tools, interactables, contracts, and economy
- **Client input** for local player movement (sync details in checklist 3.3)

## Core assets

| Asset | Path |
|-------|------|
| Network manager prefab | `Assets/_Project/Prefabs/Networking/PF_NetworkManager.prefab` |
| Bootstrap settings | `Assets/_Project/Resources/Networking/NetworkBootstrapSettings.asset` |
| Session config | `Assets/_Project/Resources/Networking/NetworkSessionConfig.asset` |
| Prefab registry | `Assets/_Project/Resources/Networking/NetworkPrefabRegistry.asset` |
| Default prefab list | `Assets/DefaultNetworkPrefabs.asset` |

## Runtime flow

1. `Bootstrapper` runs before scene load.
2. `NetworkBootstrap.EnsureInitialized()` loads `NetworkBootstrapSettings` from Resources.
3. Instantiates `PF_NetworkManager` (NetworkManager + UnityTransport + NetworkBootstrap).
4. Applies transport (`127.0.0.1:7777`), max 2 connections, and default network prefab list.

## Lobby flow (3.2)

1. Build starts in `Bootstrap` scene (`BootstrapLobbyController`).
2. Lobby UI: **Host Game**, **Join Game**, address field, status, player count.
3. Host calls `StartHost()`; client calls `StartClient(address)`.
4. Host clicks **Start Session** → `NetworkSessionFlow` loads `SampleScene` for all clients via Netcode scene management.
5. **Disconnect** shuts down Netcode and returns to offline lobby state.

## Networked player (3.3)

- Prefab: `Assets/_Project/Prefabs/Player/PF_NetworkPlayer.prefab`
- Auto-spawned by Netcode when clients connect; positioned at `NetworkPlayerSpawnPoint` markers in gameplay scenes.
- Owner runs movement/input; remote players are interpolated via `NetworkTransform`.
- `NetworkPlayerState` syncs sprint/grounded flags for interaction systems.

## Multiplayer sync (3.4)

- `NetworkRepairable` + `RepairableSurface` for shared repair progress
- `NetworkInteractable` for replicated interactions
- `NetworkPlayerToolController` for tool-use sync
- See `Replication/README.md` for test steps

## Registering spawnable prefabs

Add networked prefabs to `NetworkPrefabRegistry.registeredPrefabs` (requires `NetworkObject`) or to `DefaultNetworkPrefabs.asset`.

## Server-authoritative components

Inherit from `ServerAuthoritativeNetworkBehaviour` for repair and world state types.
