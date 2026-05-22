using UnityEngine;
using ContractorSimulator.Player.Input;

namespace ContractorSimulator.Interaction
{
    [RequireComponent(typeof(PlayerInputReader))]
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("Interaction")]
        [SerializeField] private float interactDistance = 3f;
        [SerializeField] private LayerMask interactionMask = ~0;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private InteractionPromptUI promptUI;

        private PlayerInputReader _inputReader;
        private IInteractable _currentInteractable;
        private IToolUsable _currentToolUsable;

        private void Awake()
        {
            _inputReader = GetComponent<PlayerInputReader>();
            if (_inputReader == null)
            {
                Debug.LogError("PlayerInteraction requires PlayerInputReader on the same GameObject.");
            }

            if (playerCamera == null)
            {
                playerCamera = GetComponentInChildren<Camera>(true);
            }

            if (playerCamera == null)
            {
                playerCamera = Camera.main;
            }
        }

        private void OnEnable()
        {
            if (_inputReader == null)
                return;

            _inputReader.OnInteract += HandleInteract;
            _inputReader.OnUseTool += HandleUseTool;
        }

        private void OnDisable()
        {
            if (_inputReader == null)
                return;

            _inputReader.OnInteract -= HandleInteract;
            _inputReader.OnUseTool -= HandleUseTool;
        }

        private void Update()
        {
            RefreshTarget();
        }

        private void RefreshTarget()
        {
            if (playerCamera == null)
            {
                promptUI?.HidePrompt();
                _currentInteractable = null;
                _currentToolUsable = null;
                return;
            }

            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactionMask, QueryTriggerInteraction.Ignore))
            {
                _currentInteractable = hit.collider.GetComponentInParent<IInteractable>();
                _currentToolUsable = hit.collider.GetComponentInParent<IToolUsable>();

                if (_currentInteractable != null && _currentInteractable.CanInteract)
                {
                    promptUI?.ShowPrompt(_currentInteractable.InteractionPrompt);
                    return;
                }

                if (_currentToolUsable != null && _currentToolUsable.CanUseTool)
                {
                    promptUI?.ShowPrompt(_currentToolUsable.ToolPrompt);
                    return;
                }
            }
            else
            {
                _currentInteractable = null;
                _currentToolUsable = null;
            }

            promptUI?.HidePrompt();
        }

        private void HandleInteract()
        {
            if (_currentInteractable != null && _currentInteractable.CanInteract)
            {
                _currentInteractable.Interact(this);
            }
        }

        private void HandleUseTool()
        {
            if (_currentToolUsable != null && _currentToolUsable.CanUseTool)
            {
                _currentToolUsable.UseTool(this);
            }
        }
    }
}
