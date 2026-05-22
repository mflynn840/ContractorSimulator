using System;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace ContractorSimulator.Networking.Lobby
{
    public class NetworkLobbyService : MonoBehaviour
    {
        public event Action<LobbyConnectionStatus> StatusChanged;
        public event Action<int> PlayerCountChanged;
        public event Action<string> StatusMessageChanged;

        private NetworkSessionConfig _sessionConfig;
        private bool _callbacksRegistered;

        public LobbyConnectionStatus Status { get; private set; } = LobbyConnectionStatus.Offline;
        public string StatusMessage { get; private set; } = "Offline";
        public int ConnectedPlayerCount { get; private set; }
        public bool IsHost => NetworkManager.Singleton != null && NetworkManager.Singleton.IsHost;
        public bool IsClient => NetworkManager.Singleton != null && NetworkManager.Singleton.IsClient;
        public bool IsInLobby =>
            Status == LobbyConnectionStatus.Hosting ||
            Status == LobbyConnectionStatus.Connected;

        public void Initialize(NetworkSessionConfig sessionConfig)
        {
            _sessionConfig = sessionConfig;
            RegisterCallbacks();
        }

        private void OnDestroy()
        {
            UnregisterCallbacks();
        }

        public void StartHost()
        {
            if (!EnsureNetworkManager())
            {
                return;
            }

            if (IsInLobby)
            {
                SetStatus(LobbyConnectionStatus.Failed, "Already connected. Disconnect first.");
                return;
            }

            SetStatus(LobbyConnectionStatus.StartingHost, "Starting host...");
            var started = NetworkManager.Singleton.StartHost();
            if (!started)
            {
                SetStatus(LobbyConnectionStatus.Failed, "Failed to start host.");
            }
        }

        public void StartClient(string address = null)
        {
            if (!EnsureNetworkManager())
            {
                return;
            }

            if (IsInLobby)
            {
                SetStatus(LobbyConnectionStatus.Failed, "Already connected. Disconnect first.");
                return;
            }

            ApplyClientTransportAddress(address);
            SetStatus(LobbyConnectionStatus.Connecting, "Connecting to host...");
            var started = NetworkManager.Singleton.StartClient();
            if (!started)
            {
                SetStatus(LobbyConnectionStatus.Failed, "Failed to start client.");
            }
        }

        public void Disconnect()
        {
            if (NetworkManager.Singleton == null)
            {
                SetStatus(LobbyConnectionStatus.Offline, "Offline");
                return;
            }

            SetStatus(LobbyConnectionStatus.Disconnecting, "Disconnecting...");
            NetworkManager.Singleton.Shutdown();
            SetStatus(LobbyConnectionStatus.Offline, "Offline");
            SetPlayerCount(0);
        }

        public bool CanStartGameplaySession()
        {
            if (NetworkManager.Singleton == null || !NetworkManager.Singleton.IsServer)
            {
                return false;
            }

            return ConnectedPlayerCount >= 1;
        }

        private bool EnsureNetworkManager()
        {
            NetworkBootstrap.EnsureInitialized();
            if (NetworkManager.Singleton == null)
            {
                SetStatus(LobbyConnectionStatus.Failed, "NetworkManager is not available.");
                return false;
            }

            RegisterCallbacks();
            return true;
        }

        private void RegisterCallbacks()
        {
            if (_callbacksRegistered || NetworkManager.Singleton == null)
            {
                return;
            }

            var networkManager = NetworkManager.Singleton;
            networkManager.OnServerStarted += HandleServerStarted;
            networkManager.OnClientStarted += HandleClientStarted;
            networkManager.OnClientConnectedCallback += HandleClientConnected;
            networkManager.OnClientDisconnectCallback += HandleClientDisconnected;
            networkManager.OnServerStopped += HandleServerStopped;
            networkManager.OnClientStopped += HandleClientStopped;
            networkManager.OnTransportFailure += HandleTransportFailure;
            _callbacksRegistered = true;
        }

        private void UnregisterCallbacks()
        {
            if (!_callbacksRegistered || NetworkManager.Singleton == null)
            {
                return;
            }

            var networkManager = NetworkManager.Singleton;
            networkManager.OnServerStarted -= HandleServerStarted;
            networkManager.OnClientStarted -= HandleClientStarted;
            networkManager.OnClientConnectedCallback -= HandleClientConnected;
            networkManager.OnClientDisconnectCallback -= HandleClientDisconnected;
            networkManager.OnServerStopped -= HandleServerStopped;
            networkManager.OnClientStopped -= HandleClientStopped;
            networkManager.OnTransportFailure -= HandleTransportFailure;
            _callbacksRegistered = false;
        }

        private void ApplyClientTransportAddress(string address)
        {
            if (NetworkManager.Singleton == null)
            {
                return;
            }

            var transport = NetworkManager.Singleton.NetworkConfig.NetworkTransport as UnityTransport;
            if (transport == null)
            {
                return;
            }

            var resolvedAddress = string.IsNullOrWhiteSpace(address)
                ? _sessionConfig != null ? _sessionConfig.ConnectionAddress : "127.0.0.1"
                : address.Trim();

            transport.ConnectionData.Address = resolvedAddress;
            if (_sessionConfig != null)
            {
                transport.ConnectionData.Port = _sessionConfig.ConnectionPort;
            }
        }

        private void HandleServerStarted()
        {
            SetStatus(LobbyConnectionStatus.Hosting, "Hosting — waiting for players");
            RefreshPlayerCount();
        }

        private void HandleClientStarted()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                return;
            }

            SetStatus(LobbyConnectionStatus.Connected, "Connected to host");
            RefreshPlayerCount();
        }

        private void HandleClientConnected(ulong clientId)
        {
            RefreshPlayerCount();

            if (!NetworkManager.Singleton.IsServer)
            {
                return;
            }

            var maxPlayers = _sessionConfig != null
                ? _sessionConfig.MaxPlayers
                : NetworkAuthorityRules.MvpMaxPlayers;

            if (ConnectedPlayerCount > maxPlayers)
            {
                NetworkManager.Singleton.DisconnectClient(clientId);
                SetStatus(LobbyConnectionStatus.Hosting, $"Rejected connection — lobby full ({maxPlayers} max)");
                RefreshPlayerCount();
                return;
            }

            SetStatus(
                LobbyConnectionStatus.Hosting,
                $"Hosting — {ConnectedPlayerCount}/{maxPlayers} players");
        }

        private void HandleClientDisconnected(ulong clientId)
        {
            RefreshPlayerCount();

            if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsServer)
            {
                var maxPlayers = _sessionConfig != null
                    ? _sessionConfig.MaxPlayers
                    : NetworkAuthorityRules.MvpMaxPlayers;
                SetStatus(
                    LobbyConnectionStatus.Hosting,
                    $"Hosting — {ConnectedPlayerCount}/{maxPlayers} players");
            }
            else if (Status != LobbyConnectionStatus.Disconnecting && Status != LobbyConnectionStatus.Offline)
            {
                SetStatus(LobbyConnectionStatus.Failed, "Disconnected from host");
            }
        }

        private void HandleServerStopped(bool _)
        {
            if (Status != LobbyConnectionStatus.Disconnecting)
            {
                SetStatus(LobbyConnectionStatus.Offline, "Host stopped");
            }

            SetPlayerCount(0);
        }

        private void HandleClientStopped(bool _)
        {
            if (Status != LobbyConnectionStatus.Disconnecting)
            {
                SetStatus(LobbyConnectionStatus.Offline, "Offline");
            }

            SetPlayerCount(0);
        }

        private void HandleTransportFailure()
        {
            SetStatus(LobbyConnectionStatus.Failed, "Network transport failure");
        }

        private void RefreshPlayerCount()
        {
            if (NetworkManager.Singleton == null)
            {
                SetPlayerCount(0);
                return;
            }

            SetPlayerCount(NetworkManager.Singleton.ConnectedClientsIds.Count);
        }

        private void SetStatus(LobbyConnectionStatus status, string message)
        {
            Status = status;
            StatusMessage = message;
            StatusChanged?.Invoke(status);
            StatusMessageChanged?.Invoke(message);
        }

        private void SetPlayerCount(int count)
        {
            ConnectedPlayerCount = count;
            PlayerCountChanged?.Invoke(count);
        }
    }
}
