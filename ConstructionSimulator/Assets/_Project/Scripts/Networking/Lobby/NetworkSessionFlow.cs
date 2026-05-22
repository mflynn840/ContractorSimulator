using ContractorSimulator.Managers;
using ContractorSimulator.Networking.PlayerSync;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ContractorSimulator.Networking.Lobby
{
    public class NetworkSessionFlow : MonoBehaviour
    {
        [SerializeField] private NetworkSessionConfig sessionConfig;

        private NetworkLobbyService _lobbyService;

        public void Initialize(NetworkSessionConfig config, NetworkLobbyService lobbyService)
        {
            sessionConfig = config;
            _lobbyService = lobbyService;
        }

        public bool TryStartGameplaySession()
        {
            if (_lobbyService == null || !_lobbyService.CanStartGameplaySession())
            {
                Debug.LogWarning("Cannot start gameplay session from current lobby state.");
                return false;
            }

            var networkManager = NetworkManager.Singleton;
            if (networkManager == null || !networkManager.IsServer)
            {
                Debug.LogWarning("Only the host/server can start the gameplay session.");
                return false;
            }

            var sceneName = sessionConfig != null ? sessionConfig.GameplaySceneName : "SampleScene";
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                Debug.LogError("Gameplay scene name is not configured.");
                return false;
            }

            GameManager.Instance?.StartSession();

            if (networkManager.SceneManager != null && networkManager.NetworkConfig.EnableSceneManagement)
            {
                networkManager.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            }
            else if (SceneLoader.Instance != null)
            {
                SceneLoader.Instance.LoadScene(sceneName);
            }
            else
            {
                SceneManager.LoadScene(sceneName);
            }

            return true;
        }

        public void ReturnToLobby()
        {
            GameManager.Instance?.EndSession();
            NetworkPlayerSyncService.Clear();

            if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening)
            {
                NetworkManager.Singleton.Shutdown();
            }

            var lobbyScene = sessionConfig != null ? sessionConfig.LobbySceneName : "Bootstrap";
            if (SceneLoader.Instance != null)
            {
                SceneLoader.Instance.LoadScene(lobbyScene);
            }
            else
            {
                SceneManager.LoadScene(lobbyScene);
            }
        }
    }
}
