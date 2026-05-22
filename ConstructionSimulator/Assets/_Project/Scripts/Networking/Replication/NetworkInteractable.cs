using ContractorSimulator.Interaction;
using Unity.Netcode;
using UnityEngine;

namespace ContractorSimulator.Networking.Replication
{
    [RequireComponent(typeof(NetworkObject))]
    public class NetworkInteractable : NetworkBehaviour, IInteractable
    {
        [SerializeField] private string interactionPrompt = "Press [E] to interact";
        [SerializeField] private bool canInteract = true;
        [SerializeField] private float maxInteractionDistance = 3.5f;

        private readonly NetworkVariable<bool> _hasBeenInteracted = new(
            false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server);

        public bool CanInteract => canInteract && !_hasBeenInteracted.Value;
        public string InteractionPrompt => interactionPrompt;

        private float _lastInteractRequestTime;

        public void Interact(PlayerInteraction interactor)
        {
            if (!CanInteract || interactor == null)
            {
                return;
            }

            if (!NetworkReplicationService.ShouldAllowAction(
                    _lastInteractRequestTime,
                    NetworkReplicationService.MinSecondsBetweenInteractRpc))
            {
                return;
            }

            _lastInteractRequestTime = Time.time;
            RequestInteractServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void RequestInteractServerRpc(ServerRpcParams rpcParams = default)
        {
            if (!IsServer || !canInteract || _hasBeenInteracted.Value)
            {
                return;
            }

            var senderId = rpcParams.Receive.SenderClientId;
            if (!IsClientInRange(senderId))
            {
                return;
            }

            _hasBeenInteracted.Value = true;
            OnInteractedClientRpc();
        }

        [ClientRpc]
        private void OnInteractedClientRpc()
        {
            Debug.Log($"[NetworkInteractable] {name} interaction replicated to clients.");
        }

        private bool IsClientInRange(ulong clientId)
        {
            if (NetworkManager.Singleton == null ||
                !NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out var client))
            {
                return false;
            }

            if (client.PlayerObject == null)
            {
                return false;
            }

            var distance = Vector3.Distance(
                client.PlayerObject.transform.position,
                transform.position);

            return distance <= maxInteractionDistance;
        }
    }
}
