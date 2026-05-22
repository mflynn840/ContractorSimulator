using Unity.Netcode;
using UnityEngine;

namespace ContractorSimulator.Networking.PlayerSync
{
    public class NetworkPlayerState : NetworkBehaviour
    {
        private readonly NetworkVariable<bool> _isSprinting = new(
            false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        private readonly NetworkVariable<bool> _isGrounded = new(
            true,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        public bool IsSprinting => _isSprinting.Value;
        public bool IsGrounded => _isGrounded.Value;

        public void SetSprinting(bool isSprinting)
        {
            if (!IsOwner)
            {
                return;
            }

            _isSprinting.Value = isSprinting;
        }

        public void SetGrounded(bool isGrounded)
        {
            if (!IsOwner)
            {
                return;
            }

            _isGrounded.Value = isGrounded;
        }
    }
}
