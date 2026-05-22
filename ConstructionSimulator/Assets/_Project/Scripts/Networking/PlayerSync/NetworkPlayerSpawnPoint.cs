using UnityEngine;

namespace ContractorSimulator.Networking.PlayerSync
{
    public class NetworkPlayerSpawnPoint : MonoBehaviour
    {
        [SerializeField] private Color gizmoColor = new(0.2f, 0.85f, 0.35f, 0.85f);

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(transform.position, 0.35f);
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 0.75f);
        }
    }
}
