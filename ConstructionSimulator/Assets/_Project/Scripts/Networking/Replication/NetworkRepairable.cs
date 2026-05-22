using System;
using ContractorSimulator.Interaction;
using Unity.Netcode;
using UnityEngine;

namespace ContractorSimulator.Networking.Replication
{
    public class NetworkRepairable : ServerAuthoritativeNetworkBehaviour
    {
        [Header("Repair")]
        [SerializeField] private float repairAmountPerUse = 0.1f;
        [SerializeField] private float maxRepairDistance = 3.5f;

        private readonly NetworkVariable<float> _repairProgress = new(
            0f,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server);

        private readonly NetworkVariable<bool> _isComplete = new(
            false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server);

        public override NetworkStateDomain StateDomain => NetworkStateDomain.RepairProgress;

        public float RepairProgress => _repairProgress.Value;
        public bool IsComplete => _isComplete.Value;
        public float RepairAmountPerUse => repairAmountPerUse;
        public bool CanAcceptRepair => IsSpawned && !_isComplete.Value;

        public event Action<float> RepairProgressChanged;
        public event Action RepairCompleted;

        private float _lastRepairRequestTime;

        public void RequestRepairFromClient(PlayerInteraction interactor)
        {
            if (!CanAcceptRepair || interactor == null)
            {
                return;
            }

            if (!NetworkReplicationService.ShouldAllowAction(
                    _lastRepairRequestTime,
                    NetworkReplicationService.MinSecondsBetweenRepairRpc))
            {
                return;
            }

            _lastRepairRequestTime = Time.time;
            RequestRepairServerRpc(repairAmountPerUse);
        }

        [ServerRpc(RequireOwnership = false)]
        private void RequestRepairServerRpc(float amount, ServerRpcParams rpcParams = default)
        {
            if (!ValidateServerWrite() || !CanAcceptRepair)
            {
                return;
            }

            var senderId = rpcParams.Receive.SenderClientId;
            if (!IsClientInRange(senderId))
            {
                return;
            }

            ApplyRepair(amount);
        }

        private void ApplyRepair(float amount)
        {
            var newProgress = Mathf.Clamp01(_repairProgress.Value + Mathf.Max(0f, amount));
            _repairProgress.Value = newProgress;

            if (newProgress >= 1f && !_isComplete.Value)
            {
                _isComplete.Value = true;
            }
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

            return distance <= maxRepairDistance;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            _repairProgress.OnValueChanged += HandleRepairProgressChanged;
            _isComplete.OnValueChanged += HandleCompleteChanged;
            HandleRepairProgressChanged(0f, _repairProgress.Value);
            HandleCompleteChanged(false, _isComplete.Value);
        }

        public override void OnNetworkDespawn()
        {
            _repairProgress.OnValueChanged -= HandleRepairProgressChanged;
            _isComplete.OnValueChanged -= HandleCompleteChanged;
            base.OnNetworkDespawn();
        }

        private void HandleRepairProgressChanged(float previousValue, float newValue)
        {
            RepairProgressChanged?.Invoke(newValue);
        }

        private void HandleCompleteChanged(bool previousValue, bool newValue)
        {
            if (!previousValue && newValue)
            {
                RepairCompleted?.Invoke();
            }
        }
    }
}
