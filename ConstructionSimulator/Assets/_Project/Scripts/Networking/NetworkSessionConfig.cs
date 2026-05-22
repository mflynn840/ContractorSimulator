using UnityEngine;

namespace ContractorSimulator.Networking
{
    [CreateAssetMenu(
        fileName = "NetworkSessionConfig",
        menuName = "Contractor Simulator/Networking/Session Config")]
    public class NetworkSessionConfig : ScriptableObject
    {
        [Header("Transport")]
        [SerializeField] private string connectionAddress = "127.0.0.1";
        [SerializeField] private ushort connectionPort = 7777;

        [Header("Session")]
        [SerializeField] private int maxPlayers = 2;
        [SerializeField] private bool runInBackground = true;
        [SerializeField] private string gameplaySceneName = "SampleScene";
        [SerializeField] private string lobbySceneName = "Bootstrap";

        public string ConnectionAddress => connectionAddress;
        public ushort ConnectionPort => connectionPort;
        public int MaxPlayers => maxPlayers;
        public bool RunInBackground => runInBackground;
        public string GameplaySceneName => gameplaySceneName;
        public string LobbySceneName => lobbySceneName;
    }
}
