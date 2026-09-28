using RobotAction.Core;
using UnityEngine;

namespace RobotAction.UI
{
    public sealed class HubPresenter : MonoBehaviour
    {
        [SerializeField] private HubView _hubView;

        private IPlayerStatus _playerStatus;
        private IWeaponStatus _rightStatus;
        private IWeaponStatus _leftStatus;
        private bool _isInitialized;

        public void Initialize(IPlayerStatus status, IWeaponStatus rightStatus,IWeaponStatus leftStatus)
        {
            if (_isInitialized) return;

            _isInitialized = true;

            _playerStatus = status;
            _playerStatus.OnHealthChanged += SetHealthView;
            _playerStatus.OnEnergyChanged += SetEnergyView;

            _rightStatus = rightStatus;
            _rightStatus.OnWeaponStatusChanged += SetRightWeaponStatusView;

            _leftStatus = leftStatus;
            _leftStatus.OnWeaponStatusChanged += SetLeftWeaponStatusView;

        }

        private void OnDisable()
        {
            _playerStatus.OnHealthChanged -= SetHealthView;
            _playerStatus.OnEnergyChanged -= SetEnergyView;

            _rightStatus.OnWeaponStatusChanged -= SetRightWeaponStatusView;
            _leftStatus.OnWeaponStatusChanged -= SetLeftWeaponStatusView;
        }

        public void SetHealthView(PlayerHealthInfo healthInfo)
        {
            _hubView.SetHelth(healthInfo.MaxHealth,healthInfo.CurrentHealth);
        }

        public void SetEnergyView(PlayerEnergyInfo energyInfo)
        {
            _hubView.SetEenrgy(energyInfo.MaxEnergy,energyInfo.CurrentEnergy);
        }

        private void SetRightWeaponStatusView(WeaponStatusInfo status)
        {
            _hubView.SetRightWeapon(status.MaxUseCount,status.RemainingUseCount);
        }

        public void SetLeftWeaponStatusView(WeaponStatusInfo status)
        {
            _hubView.SetLeftWeapon(status.MaxUseCount,status.RemainingUseCount);
        }
    }
}
