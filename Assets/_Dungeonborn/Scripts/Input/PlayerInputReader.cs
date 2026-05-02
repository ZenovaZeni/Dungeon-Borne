using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dungeonborn.Input
{
    [RequireComponent(typeof(PlayerInput))]
    public sealed class PlayerInputReader : MonoBehaviour
    {
        private PlayerInput playerInput;
        private InputAction moveAction;

        public event Action BasicAttackPressed;
        public event Action DashPressed;
        public event Action Skill1Pressed;
        public event Action Skill2Pressed;
        public event Action Skill3Pressed;
        public event Action UltimatePressed;
        public event Action InteractPressed;
        public event Action PausePressed;
        public event Action ResetSandboxPressed;

        public Vector2 MoveValue => moveAction?.ReadValue<Vector2>() ?? Vector2.zero;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            if (playerInput.actions == null)
            {
                Debug.LogError("PlayerInputReader requires a PlayerInput component with DungeonbornControls.inputactions assigned.", this);
                enabled = false;
                return;
            }

            var actions = playerInput.actions;
            moveAction = actions.FindAction("Move", false);
            if (moveAction == null)
            {
                Debug.LogError("DungeonbornControls is missing the required Move action.", this);
                enabled = false;
                return;
            }

            Bind("BasicAttack", _ => BasicAttackPressed?.Invoke());
            Bind("Dash", _ => DashPressed?.Invoke());
            Bind("Skill1", _ => Skill1Pressed?.Invoke());
            Bind("Skill2", _ => Skill2Pressed?.Invoke());
            Bind("Skill3", _ => Skill3Pressed?.Invoke());
            Bind("Ultimate", _ => UltimatePressed?.Invoke());
            Bind("Interact", _ => InteractPressed?.Invoke());
            Bind("Pause", _ => PausePressed?.Invoke());
            Bind("ResetSandbox", _ => ResetSandboxPressed?.Invoke());
        }

        private void Bind(string actionName, Action<InputAction.CallbackContext> handler)
        {
            var action = playerInput.actions.FindAction(actionName, false);
            if (action == null)
            {
                Debug.LogError($"DungeonbornControls is missing the required {actionName} action.", this);
                enabled = false;
                return;
            }

            action.performed += handler;
        }
    }
}
