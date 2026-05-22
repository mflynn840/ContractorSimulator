using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace ContractorSimulator.Networking
{
    [CreateAssetMenu(
        fileName = "NetworkPrefabRegistry",
        menuName = "Contractor Simulator/Networking/Prefab Registry")]
    public class NetworkPrefabRegistry : ScriptableObject
    {
        [SerializeField] private NetworkPrefabsList defaultNetworkPrefabs;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private List<GameObject> registeredPrefabs = new();

        public NetworkPrefabsList DefaultNetworkPrefabs => defaultNetworkPrefabs;
        public GameObject PlayerPrefab => playerPrefab;
        public IReadOnlyList<GameObject> RegisteredPrefabs => registeredPrefabs;

        public void ApplyToNetworkManager(NetworkManager networkManager)
        {
            if (networkManager == null)
            {
                Debug.LogError("NetworkPrefabRegistry cannot apply to a null NetworkManager.");
                return;
            }

            var config = networkManager.NetworkConfig;
            if (config == null)
            {
                Debug.LogError("NetworkManager has no NetworkConfig.");
                return;
            }

            if (defaultNetworkPrefabs != null && !config.Prefabs.NetworkPrefabsLists.Contains(defaultNetworkPrefabs))
            {
                config.Prefabs.NetworkPrefabsLists.Add(defaultNetworkPrefabs);
            }

            if (playerPrefab != null)
            {
                RegisterPrefab(config.Prefabs, playerPrefab);
            }

            foreach (var prefab in registeredPrefabs)
            {
                RegisterPrefab(config.Prefabs, prefab);
            }
        }

        private static void RegisterPrefab(NetworkPrefabs prefabs, GameObject prefab)
        {
            if (prefab == null)
            {
                return;
            }

            var networkObject = prefab.GetComponent<NetworkObject>();
            if (networkObject == null)
            {
                Debug.LogWarning(
                    $"Skipping prefab '{prefab.name}': no NetworkObject component. " +
                    "Add NetworkObject before registering for spawn.");
                return;
            }

            if (!IsPrefabRegistered(prefabs, prefab))
            {
                prefabs.Add(new NetworkPrefab { Prefab = prefab });
            }
        }

        private static bool IsPrefabRegistered(NetworkPrefabs prefabs, GameObject prefab)
        {
            foreach (var entry in prefabs.Prefabs)
            {
                if (entry.Prefab == prefab)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
