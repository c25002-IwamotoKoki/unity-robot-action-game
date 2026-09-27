using RobotAction.Core;
using UnityEngine;

namespace RobotAction.UI
{
    public sealed class HubPresenter : MonoBehaviour
    {
        [SerializeField] private HubView _hubView;

        private IPlayerStatus _playerStatus;
        private bool _isInitialized;

        public void Initialize(IPlayerStatus status)
        {
            if (_isInitialized) return;

            _isInitialized = true;

            _playerStatus = status;
            _playerStatus.OnHealthChanged += SetHealthView;
            _playerStatus.OnEnergyChanged += SetEnergyView;

        }

        private void OnDisable()
        {
            _playerStatus.OnHealthChanged -= SetHealthView;
            _playerStatus.OnEnergyChanged -= SetEnergyView;
        }

        public void SetHealthView(PlayerHealthInfo healthInfo)
        {
            _hubView.SetHelth(healthInfo);
        }

        public void SetEnergyView(PlayerEnergyInfo energyInfo)
        {
            _hubView.SetEenrgy(energyInfo.MaxEnergy,energyInfo.CurrentEnergy);
            Debug.Log($"EnergyView‚ðƒZƒbƒg|Current = {energyInfo.CurrentEnergy}|Max = {energyInfo.MaxEnergy}");
        }
    }
}
