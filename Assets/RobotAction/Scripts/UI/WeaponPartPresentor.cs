using RobotAction.Core.RobotAssembly;
using RobotAction.Gameplay.Parts.Weapons;
using UnityEngine;

namespace RobotAction.UI
{
    public sealed class WeaponPartPresentor : MonoBehaviour
    {
        [SerializeField] private WeaponPartSelector _selector;
        [SerializeField] private WeaponPartHubView _hubView;

        private WeaponPartSlot _currentSlot;

        private void OnEnable()
        {
            _selector.OnSelected += HandleSelected;
            _selector.OnSlotChanged += HandleToggleSlot;
            _selector.OnSubmitted += HandleSubmitted;
            _selector.OnCanceled += HandleCanceled;
        }

        private void OnDisable()
        {
            _selector.OnSelected -= HandleSelected;
            _selector.OnSlotChanged -= HandleToggleSlot;
            _selector.OnSubmitted -= HandleSubmitted;
            _selector.OnCanceled -= HandleCanceled;
        }

        private void HandleSelected(IWeaponPartData data)
        {
            switch (_currentSlot)
            {
                case WeaponPartSlot.Right:
                    _hubView.SetRightWeapon(data.Sprite, data.Name);
                    return;

                case WeaponPartSlot.Left:
                    _hubView.SetLeftWeapon(data.Sprite, data.Name);
                    return;
            }
        }

        private void HandleToggleSlot(WeaponPartSlot slot)
        {
            _currentSlot = slot;

            switch (_currentSlot)
            {
                case WeaponPartSlot.Right:
                    _hubView.SetRightWeaponHighlight();
                    return;

                case WeaponPartSlot.Left:
                    _hubView.SetLeftWeaponHighlight();
                    return;
            }
        }

        private void HandleSubmitted(WeaponPartSlot slot)
        {
            _currentSlot = slot;

            switch (_currentSlot)
            {
                case WeaponPartSlot.Right:
                    _hubView.SetRightSubmitted();
                    return;

                case WeaponPartSlot.Left:
                    _hubView.SetLeftSubmitted();
                    return;
            }
        }

        private void HandleCanceled()
        {
            switch (_currentSlot)
            {
                case WeaponPartSlot.Right:
                    _hubView.SetRightUnsubmitted();
                    return;

                case WeaponPartSlot.Left:
                    _hubView.SetLeftUnsubmitted();
                    return;
            }
        }
    }
}
