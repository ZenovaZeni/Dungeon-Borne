using Dungeonborn.Input;
using UnityEngine;

namespace Dungeonborn.Characters
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInputReader))]
    public sealed class PlayerMotor : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5.5f;
        [SerializeField] private float rotationSpeed = 14f;
        [SerializeField] private float dashDistance = 4f;
        [SerializeField] private float dashDuration = 0.12f;
        [SerializeField] private Transform cameraTransform;

        private CharacterController controller;
        private PlayerInputReader input;
        private Vector3 lastMoveDirection = Vector3.forward;
        private float dashTimeRemaining;
        private Vector3 dashVelocity;

        public Vector3 FacingDirection => lastMoveDirection;
        public bool IsDashing => dashTimeRemaining > 0f;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            input = GetComponent<PlayerInputReader>();
        }

        private void OnEnable()
        {
            input.DashPressed += TryDash;
        }

        private void OnDisable()
        {
            input.DashPressed -= TryDash;
        }

        private void Update()
        {
            if (cameraTransform == null && UnityEngine.Camera.main != null)
            {
                cameraTransform = UnityEngine.Camera.main.transform;
            }

            if (dashTimeRemaining > 0f)
            {
                dashTimeRemaining -= Time.deltaTime;
                controller.Move(dashVelocity * Time.deltaTime);
                return;
            }

            var move = input.MoveValue;
            var direction = GetCameraRelativeDirection(move);

            if (direction.sqrMagnitude > 0.001f)
            {
                lastMoveDirection = direction.normalized;
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(lastMoveDirection, Vector3.up),
                    rotationSpeed * Time.deltaTime);
            }

            controller.Move(direction * (moveSpeed * Time.deltaTime));
        }

        private Vector3 GetCameraRelativeDirection(Vector2 move)
        {
            var inputDirection = new Vector3(move.x, 0f, move.y);

            if (cameraTransform == null || inputDirection.sqrMagnitude <= 0.001f)
            {
                return inputDirection;
            }

            var forward = cameraTransform.forward;
            forward.y = 0f;
            forward.Normalize();

            var right = cameraTransform.right;
            right.y = 0f;
            right.Normalize();

            return Vector3.ClampMagnitude(right * move.x + forward * move.y, 1f);
        }

        private void TryDash()
        {
            if (IsDashing)
            {
                return;
            }

            dashTimeRemaining = dashDuration;
            dashVelocity = lastMoveDirection.normalized * (dashDistance / dashDuration);
        }
    }
}
