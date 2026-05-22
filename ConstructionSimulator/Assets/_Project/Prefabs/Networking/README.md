Network-enabled prefabs folder.

## MVP prefabs

- `PF_NetworkManager.prefab` — persistent Netcode stack (NetworkManager, UnityTransport, NetworkBootstrap)

Bootstrap loads this prefab via `Resources/Networking/NetworkBootstrapSettings.asset`.

Purpose: prefabs configured for multiplayer replication and ownership.