using System;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

namespace ContractorSimulator.Player.Input
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(StarterAssetsInputs))]
    public class PlayerInputReader : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private StarterAssetsInputs starterInputs;

        public Vector2 Move => starterInputs.move;
        public Vector2 Look => starterInputs.look;
        public bool Jump => starterInputs.jump;
        public bool Sprint => starterInputs.sprint;

        public event Action OnInteract;
        public event Action OnUseTool;
        public event Action OnMenu;

        private InputAction _moveAction;
        private InputAction _lookAction;
        private InputAction _jumpAction;
        private InputAction _sprintAction;
        private InputAction _interactAction;
        private InputAction _useToolAction;
        private InputAction _menuAction;

        private void Awake()
        {
            playerInput ??= GetComponent<PlayerInput>();
            starterInputs ??= GetComponent<StarterAssetsInputs>();

            if (playerInput == null)
            {
                Debug.LogError("PlayerInputReader requires a PlayerInput component.");
            }

            if (starterInputs == null)
            {
                Debug.LogError("PlayerInputReader requires a StarterAssetsInputs component.");
            }
        }

        private void OnEnable()
        {
            if (playerInput == null || playerInput.actions == null)
                return;

            _moveAction = playerInput.actions["Move"];
            _lookAction = playerInput.actions["Look"];
            _jumpAction = playerInput.actions["Jump"];
            _sprintAction = playerInput.actions["Sprint"];
            _interactAction = playerInput.actions["Interact"];
            _useToolAction = playerInput.actions["UseTool"];
            _menuAction = playerInput.actions["Menu"];

            if (_interactAction != null)
                _interactAction.performed += OnInteractPerformed;

            if (_useToolAction != null)
                _useToolAction.performed += OnUseToolPerformed;

            if (_menuAction != null)
                _menuAction.performed += OnMenuPerformed;
        }

        private void OnDisable()
        {
            if (_interactAction != null)
                _interactAction.performed -= OnInteractPerformed;

            if (_useToolAction != null)
                _useToolAction.performed -= OnUseToolPerformed;

            if (_menuAction != null)
                _menuAction.performed -= OnMenuPerformed;
        }

        private void Update()
        {
            if (starterInputs == null)
                return;

            if (_moveAction != null)
                starterInputs.MoveInput(_moveAction.ReadValue<Vector2>());

            if (_lookAction != null)
                starterInputs.LookInput(_lookAction.ReadValue<Vector2>());

            if (_jumpAction != null)
                starterInputs.JumpInput(_jumpAction.IsPressed());

            if (_sprintAction != null)
                starterInputs.SprintInput(_sprintAction.IsPressed());
        }

        private void OnInteractPerformed(InputAction.CallbackContext context)
        {
            OnInteract?.Invoke();
        }

        private void OnUseToolPerformed(InputAction.CallbackContext context)
        {
            OnUseTool?.Invoke();
        }

        private void OnMenuPerformed(InputAction.CallbackContext context)
        {
            OnMenu?.Invoke();
        }
    }
}
