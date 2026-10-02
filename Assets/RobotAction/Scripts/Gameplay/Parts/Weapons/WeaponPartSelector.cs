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
        public event Action<WeaponPartSlot> OnSubmitted;

        private WeaponPartSlot _currentSlot;
        private int _currentRightSelectIndex;
        private int _currentLeftSelectIndex;
        private string _currentRightId;
        private string _currentLeftId;
        private bool _isRightSubmitted;
        private bool _isLeftSubmitted;

        public void Awake()
        {
            _catalog.SetupDictionary();
        }

        private void Start()
        {
            SelectCurrentSlotNext();
            ToggleSlot();
            SelectCurrentSlotNext();
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
            switch (_currentSlot)
            {
                case WeaponPartSlot.Right:
                    _currentRightId = _catalog.Data[_currentRightSelectIndex].Id;
                    _isRightSubmitted = true;
                    break;

                case WeaponPartSlot.Left:
                    _currentLeftId = _catalog.Data[_currentLeftSelectIndex].Id;
                    _isLeftSubmitted = true;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(_currentSlot), _currentSlot, "ñ¢íËã`ÇÃílÇ™ì¸ÇËÇ‹ÇµÇΩ");

            }

            OnSubmitted?.Invoke(_currentSlot);
        }

        private bool IsCurrentSlotSubmitted()
        {
            return _currentSlot switch
            {
                WeaponPartSlot.Right => _isRightSubmitted,
                WeaponPartSlot.Left => _isLeftSubmitted,
                _ => throw new ArgumentOutOfRangeException(nameof(_currentSlot), _currentSlot, null),
            };
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

        private void SelectCurrentSlotNext()
        {
            //èzä¬éÆ
            if(_currentSlot == WeaponPartSlot.Right)
            {
                    _currentRightSelectIndex++;
                    _currentRightSelectIndex %= _catalog.Data.Count;
                    OnSelected?.Invoke(_catalog.Data[_currentRightSelectIndex]);
            }
            else
            {
                    _currentLeftSelectIndex++;
                    _currentLeftSelectIndex %= _catalog.Data.Count;
                    OnSelected?.Invoke(_catalog.Data[_currentLeftSelectIndex]);
            }
        }

        private void SelectCurrentSlotPrevious()
        {
            //èzä¬éÆ
            if (_currentSlot == WeaponPartSlot.Right)
            {
                    _currentRightSelectIndex--;
                _currentRightSelectIndex = (_currentRightSelectIndex + _catalog.Data.Count) % _catalog.Data.Count;
                    OnSelected?.Invoke(_catalog.Data[_currentRightSelectIndex]);
            }
            else
            {
                    _currentLeftSelectIndex--;
                _currentLeftSelectIndex = (_currentLeftSelectIndex + _catalog.Data.Count) % _catalog.Data.Count;
                    OnSelected?.Invoke(_catalog.Data[_currentLeftSelectIndex]);
            }
        }

        private void HandleSwitchNext(InputAction.CallbackContext context)
        {
            SelectCurrentSlotNext();
        }

        private void HandleSwitchPrevious(InputAction.CallbackContext context)
        {
            SelectCurrentSlotPrevious();
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
