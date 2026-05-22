using ContractorSimulator.Networking.Lobby;
using ContractorSimulator.Networking.PlayerSync;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace ContractorSimulator.Networking
{
    /// <summary>
    /// Ensures a persistent NetworkManager + UnityTransport exist and applies MVP session settings.
    /// </summary>
    [DefaultExecutionOrder(-900)]
    public class NetworkBootstrap : MonoBehaviour
    {
        private const string BootstrapSettingsResourcePath = "Networking/NetworkBootstrapSettings";

        private static bool _isInitialized;

        [SerializeField] private NetworkSessionConfig sessionConfig;
        [SerializeField] private NetworkPrefabRegistry prefabRegistry;

        public static NetworkBootstrap Instance { get; private set; }

        public NetworkSessionConfig SessionConfig => sessionConfig;

        public static void EnsureInitialized()
        {
            if (_isInitialized || NetworkManager.Singleton != null)
            {
                return;
            }

            var existing = FindFirstObjectByType<NetworkBootstrap>();
            if (existing != null)
            {
                existing.ConfigureNetworkStack();
                _isInitialized = true;
                return;
            }

            var settings = Resources.Load<NetworkBootstrapSettings>(BootstrapSettingsResourcePath);
            if (settings != null && settings.NetworkManagerPrefab != null)
            {
                var instance = Instantiate(settings.NetworkManagerPrefab);
                instance.name = "PF_NetworkManager";
                DontDestroyOnLoad(instance);

                if (instance.TryGetComponent<NetworkBootstrap>(out var bootstrap))
                {
                    bootstrap.ApplySettingsFromAsset(settings);
                }

                _isInitialized = true;
                return;
            }

            var fallback = new GameObject("PF_NetworkManager");
            fallback.AddComponent<NetworkBootstrap>();
            DontDestroyOnLoad(fallback);
            _isInitialized = true;
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            var settings = Resources.Load<NetworkBootstrapSettings>(BootstrapSettingsResourcePath);
            if (settings != null)
            {
                ApplySettingsFromAsset(settings);
            }

            ConfigureNetworkStack();
            _isInitialized = true;
        }

        public void ApplySettingsFromAsset(NetworkBootstrapSettings settings)
        {
            if (settings == null)
            {
                return;
            }

            if (settings.SessionConfig != null)
            {
                sessionConfig = settings.SessionConfig;
            }

            if (settings.PrefabRegistry != null)
            {
                prefabRegistry = settings.PrefabRegistry;
            }
        }

        public void ConfigureNetworkStack()
        {
            var networkManager = GetOrAddComponent<NetworkManager>();
            var transport = GetOrAddComponent<UnityTransport>();

            ApplySessionConfig(networkManager, transport);
            ApplyPrefabRegistry(networkManager);
            EnsureLobbyServices();
        }

        private void EnsureLobbyServices()
        {
            var lobbyService = GetOrAddComponent<NetworkLobbyService>();
            var sessionFlow = GetOrAddComponent<NetworkSessionFlow>();
            var spawnHandler = GetOrAddComponent<NetworkPlayerSpawnHandler>();
            lobbyService.Initialize(sessionConfig);
            sessionFlow.Initialize(sessionConfig, lobbyService);
            spawnHandler.RegisterCallbacks();
            ApplyPlayerPrefabToNetworkManager(GetOrAddComponent<NetworkManager>());
        }

        private void ApplyPlayerPrefabToNetworkManager(NetworkManager networkManager)
        {
            if (prefabRegistry == null)
            {
                var settings = Resources.Load<NetworkBootstrapSettings>(BootstrapSettingsResourcePath);
                prefabRegistry = settings != null ? settings.PrefabRegistry : null;
            }

            var playerPrefab = prefabRegistry != null ? prefabRegistry.PlayerPrefab : null;
            if (playerPrefab == null)
            {
                return;
            }

            networkManager.NetworkConfig.PlayerPrefab = playerPrefab;
        }

        private void ApplySessionConfig(NetworkManager networkManager, UnityTransport transport)
        {
            if (sessionConfig == null)
            {
                var settings = Resources.Load<NetworkBootstrapSettings>(BootstrapSettingsResourcePath);
                sessionConfig = settings != null ? settings.SessionConfig : null;
            }

            networkManager.RunInBackground = sessionConfig != null && sessionConfig.RunInBackground;

            var address = sessionConfig != null ? sessionConfig.ConnectionAddress : "127.0.0.1";
            var port = sessionConfig != null ? sessionConfig.ConnectionPort : (ushort)7777;

            transport.ConnectionData.Address = address;
            transport.ConnectionData.Port = port;

            networkManager.NetworkConfig.NetworkTransport = transport;
            networkManager.NetworkConfig.TickRate = 30;
            networkManager.NetworkConfig.ClientConnectionBufferTimeout = 10;
            networkManager.NetworkConfig.ConnectionApproval = false;

            if (sessionConfig != null)
            {
                networkManager.NetworkConfig.MaxConnections = sessionConfig.MaxPlayers;
            }
            else
            {
                networkManager.NetworkConfig.MaxConnections = NetworkAuthorityRules.MvpMaxPlayers;
            }
        }

        private void ApplyPrefabRegistry(NetworkManager networkManager)
        {
            if (prefabRegistry == null)
            {
                var settings = Resources.Load<NetworkBootstrapSettings>(BootstrapSettingsResourcePath);
                prefabRegistry = settings != null ? settings.PrefabRegistry : null;
            }

            prefabRegistry?.ApplyToNetworkManager(networkManager);
        }

        private T GetOrAddComponent<T>() where T : Component
        {
            if (!TryGetComponent<T>(out var component))
            {
                component = gameObject.AddComponent<T>();
            }

            return component;
        }
    }
}
