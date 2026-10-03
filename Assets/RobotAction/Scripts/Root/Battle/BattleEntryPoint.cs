using RobotAction.Gameplay.Player;
using RobotAction.UI;
using UnityEngine;

namespace RobotAction.Root.Battle
{
    public sealed class BattleEntryPoint : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private PlayerStatusHudView _playerStatusHudView;

        private PlayerStatusHudPresenter _playerstatusHudPresenter;

        private void Start()
        {
            _playerstatusHudPresenter = new PlayerStatusHudPresenter(
                _playerStatusHudView,
                _player,
                _player.RightWeaponPartsHandler,
                _player.LeftWeaponPartsHandler
            );
        }

        private void OnDisable()
        {
            _playerstatusHudPresenter.Dispose();
        }
    }
}
