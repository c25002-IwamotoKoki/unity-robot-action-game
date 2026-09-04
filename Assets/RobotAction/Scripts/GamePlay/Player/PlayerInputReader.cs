using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RobotAction.Gameplay.Player
{
    public class PlayerInputReader : IDisposable
    {
        private readonly PlayerInputActions _inputActions;
        private readonly InputAction _moveAction;
        private readonly InputAction _boostAction;

        public Vector2 MoveDirection { get; private set; }

        public event Action OnBoost;

        public PlayerInputReader(PlayerInputActions inputActions)
        {
            _inputActions = inputActions;
            _inputActions.Enable();

            _moveAction = _inputActions.Player.Move;
            _boostAction = _inputActions.Player.Boost;

            _moveAction.performed += MovePerformed;
            _moveAction.canceled += MoveCanceled;
            _boostAction.started += BoostStarted;
        }

        public void Dispose()
        {
            _moveAction.performed -= MovePerformed;
            _moveAction.canceled -= MoveCanceled;
            _boostAction.started -= BoostStarted;
            _inputActions.Disable();
            _inputActions.Dispose();
        }

        public void BoostStarted(InputAction.CallbackContext context)
        {
            OnBoost?.Invoke();
        }

        public void MovePerformed(InputAction.CallbackContext context)
        {
            MoveDirection = context.ReadValue<Vector2>();
        }

        public void MoveCanceled(InputAction.CallbackContext context)
        {
            MoveDirection = Vector2.zero;
        }

    }
}
