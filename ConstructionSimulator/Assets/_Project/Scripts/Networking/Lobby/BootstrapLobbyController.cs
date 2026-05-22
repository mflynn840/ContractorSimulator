using ContractorSimulator.UI;
using UnityEngine;

namespace ContractorSimulator.Networking.Lobby
{
    /// <summary>
    /// Boots the MVP lobby UI when the Bootstrap scene is active.
    /// </summary>
    public class BootstrapLobbyController : MonoBehaviour
    {
        private void Start()
        {
            NetworkBootstrap.EnsureInitialized();

            var bootstrap = NetworkBootstrap.Instance;
            if (bootstrap == null)
            {
                Debug.LogError("BootstrapLobbyController: NetworkBootstrap is missing.");
                return;
            }

            var lobbyService = bootstrap.GetComponent<NetworkLobbyService>();
            var sessionFlow = bootstrap.GetComponent<NetworkSessionFlow>();
            if (lobbyService == null || sessionFlow == null)
            {
                Debug.LogError("BootstrapLobbyController: Lobby services are missing on NetworkBootstrap.");
                return;
            }

            var lobbyUiObject = new GameObject("LobbyUI");
            var lobbyUi = lobbyUiObject.AddComponent<LobbyUIController>();
            lobbyUi.Initialize(lobbyService, sessionFlow, bootstrap.SessionConfig);
        }
    }
}
