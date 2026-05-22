# Player Prefabs

## MVP

- `PF_NetworkPlayer.prefab` — networked first-person player (Netcode spawn prefab)

Components:
- `NetworkObject`, `NetworkTransform` (owner authority)
- `NetworkPlayerController`, `NetworkPlayerState`
- `PlayerInput`, `PlayerInputReader`, `PlayerMovementController`, `PlayerInteraction`
- Child `PlayerCamera` with `NetworkPlayerCamera` (local owner only)

Registered via `NetworkPrefabRegistry` and `DefaultNetworkPrefabs`.
