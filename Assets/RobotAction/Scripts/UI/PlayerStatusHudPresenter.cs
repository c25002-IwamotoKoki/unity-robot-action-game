using RobotAction.Core;
using System;

namespace RobotAction.UI
{
    public sealed class PlayerStatusHudPresenter : IDisposable
    {
        private readonly PlayerStatusHudView _hubView;
        private readonly IPlayerStatus _playerStatus;
        private readonly IWeaponStatus _rightStatus;
        private readonly IWeaponStatus _leftStatus;

        public PlayerStatusHudPresenter(
            PlayerStatusHudView hudView,
            IPlayerStatus playerStatus, 
            IWeaponStatus rightstatus,
            IWeaponStatus leftStatus
        )
        {
            _hubView = hudView;

            _playerStatus = playerStatus;
            _playerStatus.OnHealthChanged += SetHealthView;
            _playerStatus.OnEnergyChanged += SetEnergyView;

            _rightStatus = rightstatus;
            _rightStatus.OnWeaponStatusChanged += SetRightWeaponStatusView;

            _leftStatus = leftStatus;
            _leftStatus.OnWeaponStatusChanged += SetLeftWeaponStatusView;

        }
     
        public void Dispose()
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
