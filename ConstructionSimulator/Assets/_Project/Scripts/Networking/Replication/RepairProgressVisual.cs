using UnityEngine;

namespace ContractorSimulator.Networking.Replication
{
    [RequireComponent(typeof(NetworkRepairable))]
    public class RepairProgressVisual : MonoBehaviour
    {
        [SerializeField] private Renderer targetRenderer;
        [SerializeField] private Color damagedColor = new(0.45f, 0.3f, 0.25f, 1f);
        [SerializeField] private Color repairedColor = new(0.75f, 0.72f, 0.68f, 1f);

        private NetworkRepairable _networkRepairable;
        private MaterialPropertyBlock _propertyBlock;
        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

        private void Awake()
        {
            _networkRepairable = GetComponent<NetworkRepairable>();
            _propertyBlock = new MaterialPropertyBlock();

            if (targetRenderer == null)
            {
                targetRenderer = GetComponentInChildren<Renderer>();
            }
        }

        private void OnEnable()
        {
            if (_networkRepairable != null)
            {
                _networkRepairable.RepairProgressChanged += HandleRepairProgressChanged;
                HandleRepairProgressChanged(_networkRepairable.RepairProgress);
            }
        }

        private void OnDisable()
        {
            if (_networkRepairable != null)
            {
                _networkRepairable.RepairProgressChanged -= HandleRepairProgressChanged;
            }
        }

        private void HandleRepairProgressChanged(float progress)
        {
            if (targetRenderer == null)
            {
                return;
            }

            var color = Color.Lerp(damagedColor, repairedColor, progress);
            targetRenderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetColor(BaseColorId, color);
            targetRenderer.SetPropertyBlock(_propertyBlock);
        }
    }
}
