# Networking tests

Place Netcode for GameObjects integration tests here.

## Planned patterns

- **Host + client**: Unity Multiplayer Play Mode (install `com.unity.multiplayer.playmode` when ready) or programmatic `NetworkManager` start in Play Mode tests.
- **Replication**: assert `NetworkVariable` / RPC state after simulated frames (`yield return null` × N).
- **Repair sync**: two clients interact with one `NetworkRepairable`; assert shared progress on server and clients.

## Conventions

- Mark slow tests: `[Category(TestCategories.Slow)]` and `[Category(TestCategories.Networking)]`.
- Use dedicated test scenes in Build Settings (e.g. `TestScene`) with minimal NetworkManager + transport setup.
- Avoid relying on player prefabs from production menus; spawn test rigs in `SetUp`.

## Stub

`NetworkingPlaceholderTests.cs` documents the category until Netcode test scenes exist.
