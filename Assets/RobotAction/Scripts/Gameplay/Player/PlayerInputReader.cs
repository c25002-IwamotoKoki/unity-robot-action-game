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
        private readonly InputAction _attackAction;
        private readonly InputAction _hoverAction;
        private readonly InputAction _rightEquipAction;
        private readonly InputAction _rightUnequipAction;
        private readonly InputAction _leftEquipAction;
        private readonly InputAction _leftUnequipAction;

        public Vector2 MoveDirection { get; private set; }

        public event Action OnBoost;
        public event Action<bool> OnAttack;
        public event Action<bool> OnHover;
        public event Action OnRightEquip;
        public event Action OnRightUnequip;
        public event Action OnLeftEquip;
        public event Action OnLeftUnequip;

        public PlayerInputReader(PlayerInputActions inputActions)
        {
            _inputActions = inputActions;
            _inputActions.Enable();

            _moveAction = _inputActions.Player.Move;
            _boostAction = _inputActions.Player.Boost;
            _attackAction = _inputActions.Player.Attack;
            _hoverAction = _inputActions.Player.Hover;
            _rightEquipAction = _inputActions.Player.RightEquip;
            _rightUnequipAction = _inputActions.Player.RightUnequip;
            _leftEquipAction = _inputActions.Player.LeftEquip;
            _leftUnequipAction = _inputActions.Player.LeftUnequip;

            _moveAction.performed += MovePerformed;
            _moveAction.canceled += MoveCanceled;
            _boostAction.started += BoostStarted;
            _attackAction.started += OnAttackInputStateChanged;
            _attackAction.canceled += OnAttackInputStateChanged;
            _hoverAction.started += OnHoverInputStateChanged;
            _hoverAction.canceled += OnHoverInputStateChanged;
            _rightEquipAction.performed += OnRightEquipPerformed;
            _rightUnequipAction.performed += OnRightUnEquipPerformed;
            _leftEquipAction.performed += OnLeftEquipPerformed;
            _leftUnequipAction.performed += OnLeftUnequipPerformed;
        }

        public void Dispose()
        {
            _moveAction.performed -= MovePerformed;
            _moveAction.canceled -= MoveCanceled;
            _boostAction.started -= BoostStarted;
            _attackAction.started -= OnAttackInputStateChanged;
            _attackAction.canceled -= OnAttackInputStateChanged;
            _hoverAction.started -= OnHoverInputStateChanged;
            _hoverAction.canceled -= OnHoverInputStateChanged;
            _rightEquipAction.performed -= OnRightEquipPerformed;
            _leftUnequipAction.performed -= OnRightUnEquipPerformed;
            _leftEquipAction.performed -= OnLeftEquipPerformed;
            _leftUnequipAction.performed -= OnLeftUnequipPerformed;
            _inputActions.Disable();
            _inputActions.Dispose();
        }

        private void BoostStarted(InputAction.CallbackContext context)
        {
            OnBoost?.Invoke();
        }

        private void MovePerformed(InputAction.CallbackContext context)
        {
            MoveDirection = context.ReadValue<Vector2>();
        }

        private void MoveCanceled(InputAction.CallbackContext context)
        {
            MoveDirection = Vector2.zero;
        }

        private void OnAttackInputStateChanged(InputAction.CallbackContext context)
        {
             OnAttack?.Invoke(context.ReadValueAsButton());
        }

        private void OnHoverInputStateChanged(InputAction.CallbackContext context)
        {
            OnHover?.Invoke(context.ReadValueAsButton());
        }

        private void OnRightEquipPerformed(InputAction.CallbackContext context)
        {
            OnRightEquip?.Invoke();
        }

        private void OnRightUnEquipPerformed(InputAction.CallbackContext context)
        {
            OnRightUnequip?.Invoke();
        }

        private void OnLeftEquipPerformed(InputAction.CallbackContext context)
        {
            OnLeftEquip?.Invoke();
        }

        private void OnLeftUnequipPerformed(InputAction.CallbackContext context)
        {
            OnLeftUnequip?.Invoke();
        }
    }
}