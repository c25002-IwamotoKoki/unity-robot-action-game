using RobotAction.Gameplay.Enemy;
using RobotAction.Gameplay.Player;
using RobotAction.UI;
using RobotAction.UI.Game;
using UnityEngine;

namespace RobotAction.Root.Battle
{
    public sealed class BattleEntryPoint : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private PlayerStatusHudView _playerStatusHudView;
        [SerializeField] private RandomAreaEnemyGenerator _randomAreaEnemyGenerator;
        [SerializeField] private GameHudView _gameHudView;
        [SerializeField] private int _requiredKillCount;

        private PlayerStatusHudPresenter _playerstatusHudPresenter;
        private int _currnetKillCount;

        private void Start()
        {
            _playerstatusHudPresenter = new PlayerStatusHudPresenter(
                _playerStatusHudView,
                _player,
                _player.RightWeaponPartsHandler,
                _player.LeftWeaponPartsHandler
            );

            _gameHudView.SetKiilCount(_requiredKillCount, _currnetKillCount);
        }

        private void OnEnable()
        {
            _randomAreaEnemyGenerator.OnEnemyDied += HandleDiedEnemy;
            _player.OnDied += HandleDiedPlayer;
        }

        private void OnDisable()
        {
            _randomAreaEnemyGenerator.OnEnemyDied -= HandleDiedEnemy;
            _player.OnDied -= HandleDiedPlayer;
            _playerstatusHudPresenter.Dispose();
        }

        private void HandleDiedEnemy()
        {
            _currnetKillCount++;

            _gameHudView.SetKiilCount(_requiredKillCount, _currnetKillCount);

            if(_currnetKillCount >= _requiredKillCount)
            {
                _gameHudView.SetGameClearText();
            }
        }

        private void HandleDiedPlayer()
        {
            _gameHudView.SetDefeatText();
        }
    }
}
