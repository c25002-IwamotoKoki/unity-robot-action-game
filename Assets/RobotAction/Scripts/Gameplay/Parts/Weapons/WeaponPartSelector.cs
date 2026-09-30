using RobotAction.Core.RobotAssembly;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RobotAction.Gameplay.Parts.Weapons
{
    public class WeaponPartSelector : MonoBehaviour
    {
        [SerializeField] private WeaponPartCatalog _catalog;
        [SerializeField] private InputActionReference _toggleSlotAction;
        [SerializeField] private InputActionReference _submitWeaponAction;
        [SerializeField] private InputActionReference _switchNextAction;
        [SerializeField] private InputActionReference _switchPreviousAction;

        public event Action<IWeaponPartData> OnSelected;
        public event Action<WeaponPartSlot> OnSlotChanged;

        private WeaponPartSlot _currentSlot;
        private int _currentSelectIndex;

        public void Awake()
        {
            _catalog.SetupDictionary();
        }

        private void Start()
        {
            SelectForCurrentSlot(0);
            ToggleSlot();
            SelectForCurrentSlot(0);
            ToggleSlot();
        }

        private void OnEnable()
        {
            EnableAction(_submitWeaponAction, HandleSubmit);
            EnableAction(_toggleSlotAction, HandleToggle);
            EnableAction(_switchNextAction, HandleSwitchNext);
            EnableAction(_switchPreviousAction, HandleSwitchPrevious);
        }

        private void OnDisable()
        {
            DisableAction(_submitWeaponAction, HandleSubmit);
            DisableAction(_toggleSlotAction, HandleToggle);
            DisableAction(_switchNextAction, HandleSwitchNext);
            DisableAction(_switchPreviousAction, HandleSwitchPrevious);
        }

        private void HandleSubmit(InputAction.CallbackContext context)
        {
            SubmitWeapon();
        }

        private void SubmitWeapon()
        {

        }

        private void HandleToggle(InputAction.CallbackContext context)
        {
            ToggleSlot();
        }

        private void ToggleSlot()
        {
            _currentSlot = (_currentSlot == WeaponPartSlot.Right)
                 ? WeaponPartSlot.Left
                 : WeaponPartSlot.Right;

            OnSlotChanged?.Invoke(_currentSlot);
        }

        private void SelectForCurrentSlot(int index)
        {
            //èzä¬éÆ
            _currentSelectIndex = (index + _catalog.Data.Count) % _catalog.Data.Count;
            OnSelected?.Invoke(_catalog.Data[_currentSelectIndex]);
        }

        private void HandleSwitchNext(InputAction.CallbackContext context)
        {
            SelectNext();
        }

        private void HandleSwitchPrevious(InputAction.CallbackContext context)
        {
            SelectPrevious();
        }

        private void SelectNext()
        {
            SelectForCurrentSlot(_currentSelectIndex + 1);
        }

        private void SelectPrevious()
        {
            SelectForCurrentSlot(_currentSelectIndex - 1);
        }

        private void EnableAction(
            InputActionReference actionReference,
            Action<InputAction.CallbackContext> handler
        )
        {
            if (actionReference == null) return;
            actionReference.action.Enable();
            actionReference.action.performed += handler;
        }

        private void DisableAction(
            InputActionReference actionReference,
            Action<InputAction.CallbackContext> handler
        )
        {
            if (actionReference == null) return;
            actionReference.action.performed -= handler;
            actionReference.action.Disable();
        }
    }
}
