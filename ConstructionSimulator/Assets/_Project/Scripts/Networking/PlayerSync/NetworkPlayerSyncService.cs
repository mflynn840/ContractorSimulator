using System;
using System.Collections.Generic;

namespace ContractorSimulator.Networking.PlayerSync
{
    public static class NetworkPlayerSyncService
    {
        private static readonly HashSet<NetworkPlayerController> ActivePlayers = new();

        public static event Action<NetworkPlayerController> PlayerSpawned;
        public static event Action<NetworkPlayerController> PlayerDespawned;

        public static int ExpectedPlayerCount { get; set; } = NetworkAuthorityRules.MvpMaxPlayers;

        public static IReadOnlyCollection<NetworkPlayerController> ActivePlayerControllers => ActivePlayers;

        public static void NotifyPlayerSpawned(NetworkPlayerController controller)
        {
            if (controller == null || !ActivePlayers.Add(controller))
            {
                return;
            }

            PlayerSpawned?.Invoke(controller);
        }

        public static void NotifyPlayerDespawned(NetworkPlayerController controller)
        {
            if (controller == null || !ActivePlayers.Remove(controller))
            {
                return;
            }

            PlayerDespawned?.Invoke(controller);
        }

        public static void Clear()
        {
            ActivePlayers.Clear();
        }
    }
}
