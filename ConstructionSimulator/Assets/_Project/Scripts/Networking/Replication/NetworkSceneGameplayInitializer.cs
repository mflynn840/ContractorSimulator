using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace ContractorSimulator.Networking.Replication
{
    /// <summary>
    /// Spawns MVP networked gameplay objects when a gameplay scene loads on the server.
    /// </summary>
    public class NetworkSceneGameplayInitializer : MonoBehaviour
    {
        [SerializeField] private GameObject repairWallPrefab;
        [SerializeField] private Vector3 repairWallPosition = new(0f, 1.5f, 4f);
        [SerializeField] private Vector3 repairWallScale = new(3f, 2f, 0.25f);

        private bool _hasSpawned;

        private void Start()
        {
            StartCoroutine(SpawnWhenServerReady());
        }

        private IEnumerator SpawnWhenServerReady()
        {
            const float timeout = 8f;
            var elapsed = 0f;

            while (elapsed < timeout)
            {
                if (TrySpawnGameplayObjects())
                {
                    yield break;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        private bool TrySpawnGameplayObjects()
        {
            if (_hasSpawned || repairWallPrefab == null)
            {
                return _hasSpawned;
            }

            var networkManager = NetworkManager.Singleton;
            if (networkManager == null || !networkManager.IsServer)
            {
                return false;
            }

            var wallInstance = Instantiate(repairWallPrefab, repairWallPosition, Quaternion.identity);
            wallInstance.transform.localScale = repairWallScale;

            var networkObject = wallInstance.GetComponent<NetworkObject>();
            if (networkObject == null)
            {
                Debug.LogError("Repair wall prefab is missing NetworkObject.");
                Destroy(wallInstance);
                return false;
            }

            networkObject.Spawn();
            _hasSpawned = true;
            return true;
        }
    }
}
