using ContractorSimulator.Networking.PlayerSync;
using ContractorSimulator.Networking.Replication;
using UnityEngine;

namespace ContractorSimulator.Interaction
{
    [RequireComponent(typeof(NetworkRepairable))]
    public class RepairableSurface : MonoBehaviour, IToolUsable
    {
        [SerializeField] private string toolPrompt = "Press [Use Tool] to repair wall";
        [SerializeField] private bool requiresRepairTool = true;

        private NetworkRepairable _networkRepairable;

        private void Awake()
        {
            _networkRepairable = GetComponent<NetworkRepairable>();
        }

        public bool CanUseTool => _networkRepairable != null && _networkRepairable.CanAcceptRepair;

        public string ToolPrompt
        {
            get
            {
                if (_networkRepairable == null)
                {
                    return toolPrompt;
                }

                var percent = Mathf.RoundToInt(_networkRepairable.RepairProgress * 100f);
                return $"{toolPrompt} ({percent}%)";
            }
        }

        public void UseTool(PlayerInteraction interactor)
        {
            if (_networkRepairable == null || interactor == null)
            {
                return;
            }

            if (requiresRepairTool && !InteractorHasRepairTool(interactor))
            {
                return;
            }

            _networkRepairable.RequestRepairFromClient(interactor);

            var toolSync = interactor.GetComponent<NetworkPlayerToolController>();
            toolSync?.NotifyToolUsed();
        }

        private static bool InteractorHasRepairTool(PlayerInteraction interactor)
        {
            var toolController = interactor.GetComponent<NetworkPlayerToolController>();
            return toolController == null || toolController.HasRepairTool;
        }
    }
}
