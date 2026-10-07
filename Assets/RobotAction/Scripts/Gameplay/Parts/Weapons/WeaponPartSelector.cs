using RobotAction.Core.RobotAssembly;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RobotAction.Gameplay.Parts.Weapons
{
    public class WeaponPartSelector : MonoBehaviour
    {
        public event Action<IWeaponPartData> OnSelected;
        public event Action<WeaponPartSlot> OnSlotChanged;
        public event Action OnSubmitted;
        public event Action OnCanceled;
        public event Action<RobotAssemblyInfo> OnAllSubmitted;

        [SerializeField] private WeaponPartCatalog _catalog;
        [SerializeField] private InputActionReference _toggleSlotAction;
        [SerializeField] private InputActionReference _submitWeaponAction;
        [SerializeField] private InputActionReference _switchNextAction;
        [SerializeField] private InputActionReference _switchPreviousAction;
        [SerializeField] private InputActionReference _cancelAction;

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
            EnableAction(_cancelAction, HandleCancel);
        }

        private void OnDisable()
        {
            DisableAction(_submitWeaponAction, HandleSubmit);
            DisableAction(_toggleSlotAction, HandleToggle);
            DisableAction(_switchNextAction, HandleSwitchNext);
            DisableAction(_switchPreviousAction, HandleSwitchPrevious);
            DisableAction(_cancelAction,HandleCancel);
        }

        private void HandleSubmit(InputAction.CallbackContext context)
        {
            SubmitCurrentSlot();
        }

        private void SubmitCurrentSlot()
        {
            if(_isRightSubmitted && _isLeftSubmitted)
            {
                var robotAssemblyInfo = new RobotAssemblyInfo(_currentRightId, _currentLeftId);

                OnAllSubmitted?.Invoke(robotAssemblyInfo);
            }

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

            OnSubmitted?.Invoke();
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

        private void HandleCancel(InputAction.CallbackContext context)
        {
            CancelCurrentSlot();
        }

        private void CancelCurrentSlot()
        {
            if (!IsCurrentSlotSubmitted())
            {
                return;
            }

            switch (_currentSlot)
            {
                case WeaponPartSlot.Right:
                    _isRightSubmitted = false;
                    break;

                    case WeaponPartSlot.Left:
                    _isLeftSubmitted = false;
                    break;
            }

            OnCanceled?.Invoke();
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
            if (IsCurrentSlotSubmitted())
            {
                return;
            }

            switch (_currentSlot)
            {
                case WeaponPartSlot.Right:
                    _currentRightSelectIndex++;
                    _currentRightSelectIndex %= _catalog.Data.Count;
                    OnSelected?.Invoke(_catalog.Data[_currentRightSelectIndex]);
                    break;

                case WeaponPartSlot.Left:
                    _currentLeftSelectIndex++;
                    _currentLeftSelectIndex %= _catalog.Data.Count;
                    OnSelected?.Invoke(_catalog.Data[_currentLeftSelectIndex]);
                    break;
            }
        }

        private void SelectCurrentSlotPrevious()
        {
            if (IsCurrentSlotSubmitted())
            {
                return;
            }

            //èzä¬éÆ
            switch (_currentSlot)
            {
                case WeaponPartSlot.Right:
                    _currentRightSelectIndex--;
                    _currentRightSelectIndex = (_currentRightSelectIndex + _catalog.Data.Count) % _catalog.Data.Count;
                    OnSelected?.Invoke(_catalog.Data[_currentRightSelectIndex]);
                    break;

                case WeaponPartSlot.Left:
                    _currentLeftSelectIndex--;
                    _currentLeftSelectIndex = (_currentLeftSelectIndex + _catalog.Data.Count) % _catalog.Data.Count;
                    OnSelected?.Invoke(_catalog.Data[_currentLeftSelectIndex]);
                    break;
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
