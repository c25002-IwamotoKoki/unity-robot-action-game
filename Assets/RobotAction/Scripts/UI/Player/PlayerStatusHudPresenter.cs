using RobotAction.Core;
using System;

namespace RobotAction.UI.Player
{
    public sealed class PlayerStatusHudPresenter : IDisposable
    {
        private readonly PlayerStatusHudView _hudView;
        private readonly IPlayerStatus _playerStatus;
        private readonly IWeaponPartHandler _rightWeaponHandler;
        private readonly IWeaponPartHandler _leftWeaponHandler;

        public PlayerStatusHudPresenter(
            PlayerStatusHudView hudView,
            IPlayerStatus playerStatus, 
            IWeaponPartHandler rightWeaponHandler,
            IWeaponPartHandler leftWeaponHandler
        )
        {
            _hudView = hudView;

            _playerStatus = playerStatus;
            _playerStatus.OnHealthChanged += SetHealthView;
            _playerStatus.OnEnergyChanged += SetEnergyView;

            _rightWeaponHandler = rightWeaponHandler;
            _rightWeaponHandler.OnWeaponStatusChanged += SetRightWeaponStatusView;
            _rightWeaponHandler.OnUnequipWeapon += HandleRightWeaponUnequip;

            _leftWeaponHandler = leftWeaponHandler;
            _leftWeaponHandler.OnWeaponStatusChanged += SetLeftWeaponStatusView;
            _leftWeaponHandler.OnUnequipWeapon += HandleLeftWeaponUnequip;

        }
     
        public void Dispose()
        {
            _playerStatus.OnHealthChanged -= SetHealthView;
            _playerStatus.OnEnergyChanged -= SetEnergyView;

            _rightWeaponHandler.OnWeaponStatusChanged -= SetRightWeaponStatusView;
            _rightWeaponHandler.OnUnequipWeapon -= HandleRightWeaponUnequip;
            _leftWeaponHandler.OnWeaponStatusChanged -= SetLeftWeaponStatusView;
            _leftWeaponHandler.OnUnequipWeapon -= HandleLeftWeaponUnequip;
        }

        private void SetHealthView()
        {
            _hudView.SetHelth(_playerStatus.MaxHealth,_playerStatus.CurrentHealth);
        }

        private void SetEnergyView()
        {
            _hudView.SetEenrgy(_playerStatus.MaxEnergy,_playerStatus.CurrentEnergy);
        }

        private void SetRightWeaponStatusView()
        {
            _hudView.SetRightWeapon(_rightWeaponHandler.MaxUseCount,_rightWeaponHandler.RemainingUseCount);
        }

        private void SetLeftWeaponStatusView()
        {
            _hudView.SetLeftWeapon(_leftWeaponHandler.MaxUseCount,_leftWeaponHandler.RemainingUseCount);
        }

        private void HandleRightWeaponUnequip()
        {
            _hudView.SetRightWeaponNone();
        }

        private void HandleLeftWeaponUnequip()
        {
            _hudView.SetLeftWeaponNone();
        }
    }
}
