using RobotAction.Core.Scene;
using RobotAction.Gameplay.Enemy;
using RobotAction.Gameplay.Player;
using RobotAction.UI;
using RobotAction.UI.Game;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace RobotAction.Root.Battle
{
    public sealed class BattleEntryPoint : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private PlayerStatusHudView _playerStatusHudView;
        [SerializeField] private RandomAreaEnemyGenerator _randomAreaEnemyGenerator;
        [SerializeField] private GameHudView _gameHudView;
        [SerializeField] private InputActionReference _retryAction;
        [SerializeField] private int _requiredKillCount;

        private PlayerStatusHudPresenter _playerStatusHudPresenter;
        private int _currnetKillCount;
        private bool _isPlayerDeathProcessed;

        private void Start()
        {
            _playerStatusHudPresenter = new PlayerStatusHudPresenter(
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
            _player.OnDied += HandlePlayerDied;
        }

        private void OnDisable()
        {
            _randomAreaEnemyGenerator.OnEnemyDied -= HandleDiedEnemy;
            _player.OnDied -= HandlePlayerDied;

            _retryAction.action.started -= HandleRetryAction;
            _retryAction.action.Disable();

            _playerStatusHudPresenter.Dispose();
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

        private void HandlePlayerDied()
        {
            if (_isPlayerDeathProcessed) return;

            _gameHudView.SetDefeatDisplay();

            _retryAction.action.Enable();
            _retryAction.action.started += HandleRetryAction;
            _isPlayerDeathProcessed = true;

            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void OnRetryButtonClicked()
        {
            ExecuteRetry();
        }

        private void HandleRetryAction(InputAction.CallbackContext context)
        {
            ExecuteRetry();
        }

        private void ExecuteRetry()
        {
            Time.timeScale = 1f;

            SceneManager.LoadScene(SceneNames.Battle);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
