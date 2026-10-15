using RobotAction.Core.Scene;
using RobotAction.Gameplay.Enemy;
using RobotAction.Gameplay.Player;
using RobotAction.UI.Player;
using RobotAction.UI.Game;
using RobotAction.UI.Sensor;
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
        [SerializeField] private LockOnHudView _lockOnHudView;
        [SerializeField] private InputActionReference _submitAction;
        [SerializeField] private int _requiredKillCount;

        private PlayerStatusHudPresenter _playerStatusHudPresenter;
        private int _currnetKillCount;
        private bool _isPlayerDeathProcessed;
        private bool _isGameClearProcessed;

        private void Start()
        {
            _playerStatusHudPresenter = new PlayerStatusHudPresenter(
                _playerStatusHudView,
                _player,
                _player.RightWeaponPartsHandler,
                _player.LeftWeaponPartsHandler
            );

            _lockOnHudView.Initialized(_player.AutoLockSensor);

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

            _submitAction.action.started -= HandleRetryAction;
            _submitAction.action.started -= HandleToTitleAction;
            _submitAction.action.Disable();

            _playerStatusHudPresenter.Dispose();
        }

        private void HandleDiedEnemy()
        {
            _currnetKillCount++;

            _gameHudView.SetKiilCount(_requiredKillCount, _currnetKillCount);

            if(_currnetKillCount >= _requiredKillCount)
            {
                GameClearProcess();
            }
        }

        private void HandlePlayerDied()
        {
            //二重に呼ばれることによるEventの二重購読を防ぐため
            if (_isPlayerDeathProcessed || _isGameClearProcessed) return;

            _gameHudView.SetDefeatDisplay();

            _submitAction.action.Enable();
            _submitAction.action.started += HandleRetryAction;
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

        private void GameClearProcess()
        {
            //二重に呼ばれることによるEventの二重購読を防ぐため
            if (_isGameClearProcessed || _isPlayerDeathProcessed) return;

            _submitAction.action.Enable();
            _submitAction.action.started += HandleToTitleAction;
            _isGameClearProcessed = true;

            _gameHudView.SetGameClearText();

            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void OnToTitleButtonClicked()
        {
            ExecuteLoadTitle();
        }

        private void HandleToTitleAction(InputAction.CallbackContext context)
        {
            ExecuteLoadTitle();
        }

        private void ExecuteLoadTitle()
        {
            Time.timeScale = 1f;
            //タイトルシーンでもカーソルを使うため
            //Cursor.LockStateはNoneでCurSor.visibleはtrueのまま
            SceneManager.LoadScene(SceneNames.Title);
        }    
    }
}
