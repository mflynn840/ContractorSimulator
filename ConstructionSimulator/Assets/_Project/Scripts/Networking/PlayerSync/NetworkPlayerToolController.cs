using Unity.Netcode;
using UnityEngine;

namespace ContractorSimulator.Networking.PlayerSync
{
    public class NetworkPlayerToolController : NetworkBehaviour
    {
        public enum ToolType
        {
            None = 0,
            Hammer = 1
        }

        [SerializeField] private ToolType defaultEquippedTool = ToolType.Hammer;

        private readonly NetworkVariable<int> _equippedToolId = new(
            (int)ToolType.None,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        private readonly NetworkVariable<float> _lastToolUseTime = new(
            0f,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server);

        public ToolType EquippedTool => (ToolType)_equippedToolId.Value;
        public bool HasRepairTool => EquippedTool == ToolType.Hammer;
        public float LastToolUseTime => _lastToolUseTime.Value;

        public event System.Action ToolUsed;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsOwner)
            {
                _equippedToolId.Value = (int)defaultEquippedTool;
            }

            _lastToolUseTime.OnValueChanged += HandleToolUseTimeChanged;
        }

        public override void OnNetworkDespawn()
        {
            _lastToolUseTime.OnValueChanged -= HandleToolUseTimeChanged;
            base.OnNetworkDespawn();
        }

        public void NotifyToolUsed()
        {
            if (!IsOwner)
            {
                return;
            }

            NotifyToolUsedServerRpc();
        }

        [ServerRpc]
        private void NotifyToolUsedServerRpc()
        {
            _lastToolUseTime.Value = Time.time;
            NotifyToolUsedClientRpc();
        }

        [ClientRpc]
        private void NotifyToolUsedClientRpc()
        {
            ToolUsed?.Invoke();
        }

        private void HandleToolUseTimeChanged(float previousValue, float newValue)
        {
            if (newValue > previousValue)
            {
                ToolUsed?.Invoke();
            }
        }
    }
}
