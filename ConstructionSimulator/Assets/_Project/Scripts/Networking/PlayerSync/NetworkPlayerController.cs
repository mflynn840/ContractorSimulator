using ContractorSimulator.Interaction;
using ContractorSimulator.Player.Input;
using ContractorSimulator.Player.Movement;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ContractorSimulator.Networking.PlayerSync
{
    [RequireComponent(typeof(NetworkObject))]
    public class NetworkPlayerController : NetworkBehaviour
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private PlayerInputReader inputReader;
        [SerializeField] private PlayerMovementController movementController;
        [SerializeField] private PlayerInteraction playerInteraction;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private NetworkPlayerCamera playerCamera;
        [SerializeField] private NetworkPlayerState playerState;
        [SerializeField] private NetworkPlayerToolController toolController;

        public NetworkPlayerState PlayerState => playerState;
        public NetworkPlayerToolController ToolController => toolController;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            ApplyOwnership();
            NetworkPlayerSyncService.NotifyPlayerSpawned(this);
        }

        public override void OnNetworkDespawn()
        {
            NetworkPlayerSyncService.NotifyPlayerDespawned(this);
            base.OnNetworkDespawn();
        }

        private void ApplyOwnership()
        {
            var isLocalOwner = IsOwner;

            if (playerInput != null)
            {
                playerInput.enabled = isLocalOwner;
            }

            if (inputReader != null)
            {
                inputReader.enabled = isLocalOwner;
            }

            if (movementController != null)
            {
                movementController.enabled = isLocalOwner;
            }

            if (playerInteraction != null)
            {
                playerInteraction.enabled = isLocalOwner;
            }

            if (characterController != null)
            {
                characterController.enabled = isLocalOwner;
            }

            playerCamera?.SetLocalOwner(isLocalOwner);

            if (toolController == null)
            {
                TryGetComponent(out toolController);
            }

            if (isLocalOwner)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
