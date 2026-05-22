using ContractorSimulator.Networking.PlayerSync;
using Unity.Netcode;
using UnityEngine;
using ContractorSimulator.Player.Input;

namespace ContractorSimulator.Player.Movement
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInputReader))]
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float walkSpeed = 4f;
        [SerializeField] private float sprintSpeed = 6f;
        [SerializeField] private float rotationSpeed = 200f;
        [SerializeField] private float gravity = -15f;
        [SerializeField] private float jumpHeight = 1.2f;

        [Header("Ground Check")]
        [SerializeField] private float groundedOffset = -0.14f;
        [SerializeField] private float groundedRadius = 0.5f;
        [SerializeField] private LayerMask groundLayers = ~0;

        [Header("Camera")]
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float topClamp = 90f;
        [SerializeField] private float bottomClamp = -90f;

        private CharacterController _controller;
        private PlayerInputReader _inputReader;
        private NetworkObject _networkObject;
        private NetworkPlayerState _playerState;
        private float _verticalVelocity;
        private float _targetRotationY;
        private float _cameraPitch;
        private bool _grounded;

        private const float InputThreshold = 0.01f;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _inputReader = GetComponent<PlayerInputReader>();
            TryGetComponent(out _networkObject);
            TryGetComponent(out _playerState);

            if (cameraTransform == null)
            {
                var childCamera = GetComponentInChildren<Camera>(true);
                if (childCamera != null)
                {
                    cameraTransform = childCamera.transform;
                }
            }

            if (cameraTransform == null && Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }

            _targetRotationY = transform.eulerAngles.y;
            if (cameraTransform != null)
            {
                _cameraPitch = cameraTransform.localEulerAngles.x;
                if (_cameraPitch > 180f)
                {
                    _cameraPitch -= 360f;
                }
            }
            else
            {
                _cameraPitch = 0f;
            }
        }

        private void Update()
        {
            if (!IsLocallyControlled())
            {
                return;
            }

            GroundedCheck();
            HandleJumpAndGravity();
            HandleMove();
            HandleLook();
            SyncPlayerState();
        }

        private bool IsLocallyControlled()
        {
            return _networkObject == null || _networkObject.IsOwner;
        }

        private void SyncPlayerState()
        {
            if (_playerState == null || _inputReader == null)
            {
                return;
            }

            _playerState.SetSprinting(_inputReader.Sprint);
            _playerState.SetGrounded(_grounded);
        }

        private void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + groundedOffset, transform.position.z);
            _grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);
        }

        private void HandleMove()
        {
            Vector2 moveInput = _inputReader.Move;
            float targetSpeed = _inputReader.Sprint ? sprintSpeed : walkSpeed;

            if (moveInput == Vector2.zero)
            {
                targetSpeed = 0f;
            }

            Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
            Vector3 movement = moveDirection.normalized * targetSpeed * Time.deltaTime;
            movement.y = _verticalVelocity * Time.deltaTime;

            _controller.Move(movement);
        }

        private void HandleLook()
        {
            Vector2 lookInput = _inputReader.Look;
            if (lookInput.sqrMagnitude < InputThreshold || cameraTransform == null)
                return;

            float deltaTimeMultiplier = 1f;
            _targetRotationY += lookInput.x * rotationSpeed * deltaTimeMultiplier * Time.deltaTime;
            _cameraPitch -= lookInput.y * rotationSpeed * deltaTimeMultiplier * Time.deltaTime;
            _cameraPitch = Mathf.Clamp(_cameraPitch, bottomClamp, topClamp);

            transform.rotation = Quaternion.Euler(0f, _targetRotationY, 0f);
            cameraTransform.localRotation = Quaternion.Euler(_cameraPitch, 0f, 0f);
        }

        private void HandleJumpAndGravity()
        {
            if (_grounded)
            {
                if (_verticalVelocity < 0f)
                {
                    _verticalVelocity = -2f;
                }

                if (_inputReader.Jump)
                {
                    _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                }
            }

            _verticalVelocity += gravity * Time.deltaTime;
        }
    }
}
