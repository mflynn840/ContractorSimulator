using UnityEngine;

namespace ContractorSimulator.Networking
{
    [CreateAssetMenu(
        fileName = "NetworkBootstrapSettings",
        menuName = "Contractor Simulator/Networking/Bootstrap Settings")]
    public class NetworkBootstrapSettings : ScriptableObject
    {
        [SerializeField] private GameObject networkManagerPrefab;
        [SerializeField] private NetworkSessionConfig sessionConfig;
        [SerializeField] private NetworkPrefabRegistry prefabRegistry;

        public GameObject NetworkManagerPrefab => networkManagerPrefab;
        public NetworkSessionConfig SessionConfig => sessionConfig;
        public NetworkPrefabRegistry PrefabRegistry => prefabRegistry;
    }
}
