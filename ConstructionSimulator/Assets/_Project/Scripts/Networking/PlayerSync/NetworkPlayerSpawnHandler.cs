using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ContractorSimulator.Networking.PlayerSync
{
    public class NetworkPlayerSpawnHandler : MonoBehaviour
    {
        [SerializeField] private Vector3 defaultSpawnPosition = new(0f, 1f, 0f);
        [SerializeField] private float spawnSpacing = 2f;

        private readonly List<Vector3> _fallbackSpawnOffsets = new()
        {
            Vector3.zero,
            new Vector3(2f, 0f, 0f)
        };

        private bool _callbacksRegistered;

        private void Start()
        {
            RegisterCallbacks();
        }

        private void OnDestroy()
        {
            UnregisterCallbacks();
        }

        public void RegisterCallbacks()
        {
            if (_callbacksRegistered || NetworkManager.Singleton == null)
            {
                return;
            }

            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;
            _callbacksRegistered = true;
        }

        private void UnregisterCallbacks()
        {
            if (!_callbacksRegistered || NetworkManager.Singleton == null)
            {
                return;
            }

            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnected;
            _callbacksRegistered = false;
        }

        private void HandleClientConnected(ulong clientId)
        {
            if (NetworkManager.Singleton == null || !NetworkManager.Singleton.IsServer)
            {
                return;
            }

            StartCoroutine(PositionPlayerWhenReady(clientId));
        }

        private void HandleClientDisconnected(ulong clientId)
        {
            // PlayerObject is cleaned up by Netcode on disconnect.
        }

        private IEnumerator PositionPlayerWhenReady(ulong clientId)
        {
            const float timeoutSeconds = 3f;
            var elapsed = 0f;

            while (elapsed < timeoutSeconds)
            {
                if (NetworkManager.Singleton != null &&
                    NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out var client) &&
                    client.PlayerObject != null)
                {
                    client.PlayerObject.transform.SetPositionAndRotation(
                        GetSpawnPosition(clientId),
                        Quaternion.identity);
                    yield break;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        private Vector3 GetSpawnPosition(ulong clientId)
        {
            var spawnPoints = FindSpawnPointsInActiveScene();
            if (spawnPoints.Count > 0)
            {
                var index = (int)(clientId % (ulong)spawnPoints.Count);
                return spawnPoints[index].position;
            }

            var fallbackIndex = (int)(clientId % (ulong)_fallbackSpawnOffsets.Count);
            return defaultSpawnPosition + _fallbackSpawnOffsets[fallbackIndex] * spawnSpacing;
        }

        private static List<Transform> FindSpawnPointsInActiveScene()
        {
            var results = new List<Transform>();
            var activeScene = SceneManager.GetActiveScene();
            var roots = activeScene.GetRootGameObjects();

            foreach (var root in roots)
            {
                var points = root.GetComponentsInChildren<NetworkPlayerSpawnPoint>(true);
                foreach (var point in points)
                {
                    results.Add(point.transform);
                }
            }

            return results;
        }
    }
}
