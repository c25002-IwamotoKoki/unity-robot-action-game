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

        private int _lastCurrentHealth = -1;
        private int _lastCurrentEnergy = -1;

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
            int intCurrent = (int)_playerStatus.CurrentHealth;
            //Å‘åHP‚ª“®“I‚É‘‚¦‚éˆ—‚Í‚È‚¢‚½‚ßCurrentHealth‚Ì‚İ”äŠr‚µ
            bool isChangeText = intCurrent != _lastCurrentHealth;

            //intŒ^‚ÅŒ©‚½•Ï“®‚ª‚ ‚é‚Æ‚«‚Ì‚İText‚ğ•Ï‰»‚³‚¹‚é
            if(isChangeText)
            {
                _lastCurrentHealth = intCurrent;
            }

            _hudView.SetEnergy(_playerStatus.MaxHealth,_playerStatus.CurrentHealth,isChangeText);
        }

        private void SetEnergyView()
        {
            int intCurrent = (int)_playerStatus.CurrentEnergy;
            //Å‘åEnergy‚ª“®“I‚É‘‚¦‚éˆ—‚Í‚È‚¢‚½‚ßCurrentEnergy‚Ì‚İ”äŠr
            bool isChangeText = intCurrent != _lastCurrentEnergy;

            //intŒ^‚ÅŒ©‚½•Ï“®‚ª‚ ‚é‚Æ‚«‚Ì‚İText‚ğ•Ï‰»‚³‚¹‚é
            if (_lastCurrentEnergy != intCurrent)
            {
                _lastCurrentEnergy = intCurrent;
            }

            _hudView.SetEnergy(_playerStatus.MaxEnergy, _playerStatus.CurrentEnergy,isChangeText);
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
