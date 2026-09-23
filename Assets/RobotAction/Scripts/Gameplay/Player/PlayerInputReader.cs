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
        private readonly InputAction _rightAttackAction;
        private readonly InputAction _leftAttackAction;
        private readonly InputAction _hoverAction;
        private readonly InputAction _rightEquipAction;
        private readonly InputAction _rightUnequipAction;
        private readonly InputAction _leftEquipAction;
        private readonly InputAction _leftUnequipAction;
        private readonly InputAction _lockOnAction;
        private readonly InputAction _lookAction;

        public Vector2 MoveDirection { get; private set; }
        public Vector2 LookValue => _lookAction?.ReadValue<Vector2>() ?? Vector2.zero;

        public bool IsMouseLook => _lookAction.activeControl?.device is Mouse;

        public event Action OnBoost;
        public event Action<bool> OnRightAttack;
        public event Action<bool> OnLeftAttack;
        public event Action<bool> OnHover;
        public event Action OnRightEquip;
        public event Action OnRightUnequip;
        public event Action OnLeftEquip;
        public event Action OnLeftUnequip;
        public event Action OnLockOn;

        public PlayerInputReader(PlayerInputActions inputActions)
        {
            _inputActions = inputActions;
            _inputActions.Enable();

            _moveAction = _inputActions.Player.Move;
            _boostAction = _inputActions.Player.Boost;
            _hoverAction = _inputActions.Player.Hover;

            _rightAttackAction = _inputActions.Player.RightAttack;
            _leftAttackAction = _inputActions.Player.LeftAttack;

            _rightEquipAction = _inputActions.Player.RightEquip;
            _rightUnequipAction = _inputActions.Player.RightUnequip;
            _leftEquipAction = _inputActions.Player.LeftEquip;
            _leftUnequipAction = _inputActions.Player.LeftUnequip;

            _lockOnAction = _inputActions.Player.LockOn;
            _lookAction = _inputActions.Player.Look;

            _moveAction.performed += HandleMovePerformed;
            _moveAction.canceled += HandleMoveCanceled;
            _boostAction.started += HandleBoostStarted;
            _hoverAction.started += HandleHoverInputStateChanged;
            _hoverAction.canceled += HandleHoverInputStateChanged;

            _rightAttackAction.started += HandleRightAttackInputStateChanged;
            _rightAttackAction.canceled += HandleRightAttackInputStateChanged;
            _leftAttackAction.started += HandleLeftAttackInputStateChange;
            _leftAttackAction.canceled += HandleLeftAttackInputStateChange;

            _rightEquipAction.performed += HandleRightEquipPerformed;
            _rightUnequipAction.performed += HandleRightUnEquipPerformed;
            _leftEquipAction.performed += HandleLeftEquipPerformed;
            _leftUnequipAction.performed += HandleLeftUnequipPerformed;

            _lockOnAction.started += HandleLockOnStarted;
       
        }

        public void Dispose()
        {
            _moveAction.performed -= HandleMovePerformed;
            _moveAction.canceled -= HandleMoveCanceled;
            _boostAction.started -= HandleBoostStarted;
            _hoverAction.started -= HandleHoverInputStateChanged;
            _hoverAction.canceled -= HandleHoverInputStateChanged;

            _rightAttackAction.started -= HandleRightAttackInputStateChanged;
            _rightAttackAction.canceled -= HandleRightAttackInputStateChanged;
            _leftAttackAction.started -= HandleLeftAttackInputStateChange;
            _leftAttackAction.canceled -= HandleLeftAttackInputStateChange;

            _rightEquipAction.performed -= HandleRightEquipPerformed;
            _rightUnequipAction.performed -= HandleRightUnEquipPerformed;
            _leftEquipAction.performed -= HandleLeftEquipPerformed;
            _leftUnequipAction.performed -= HandleLeftUnequipPerformed;

            _lockOnAction.started -= HandleLockOnStarted;

            _inputActions.Disable();
            _inputActions.Dispose();
        }

        private void HandleBoostStarted(InputAction.CallbackContext context)
        {
            OnBoost?.Invoke();
        }

        private void HandleMovePerformed(InputAction.CallbackContext context)
        {
            MoveDirection = context.ReadValue<Vector2>();
        }

        private void HandleMoveCanceled(InputAction.CallbackContext context)
        {
            MoveDirection = Vector2.zero;
        }

        private void HandleRightAttackInputStateChanged(InputAction.CallbackContext context)
        {
            OnRightAttack?.Invoke(context.ReadValueAsButton());
        }

        public void HandleLeftAttackInputStateChange(InputAction.CallbackContext context)
        {
            OnLeftAttack?.Invoke(context.ReadValueAsButton());
        }

        private void HandleHoverInputStateChanged(InputAction.CallbackContext context)
        {
            OnHover?.Invoke(context.ReadValueAsButton());
        }

        private void HandleRightEquipPerformed(InputAction.CallbackContext context)
        {
            OnRightEquip?.Invoke();
        }

        private void HandleRightUnEquipPerformed(InputAction.CallbackContext context)
        {
            OnRightUnequip?.Invoke();
        }

        private void HandleLeftEquipPerformed(InputAction.CallbackContext context)
        {
            OnLeftEquip?.Invoke();
        }

        private void HandleLeftUnequipPerformed(InputAction.CallbackContext context)
        {
            OnLeftUnequip?.Invoke();
        }

        private void HandleLockOnStarted(InputAction.CallbackContext context)
        {
            OnLockOn?.Invoke();
        }
    }
}