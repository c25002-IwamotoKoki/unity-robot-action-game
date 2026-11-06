using RobotAction.Core;
using RobotAction.Gameplay.Enemy;
using RobotAction.Gameplay.Enemy.Boss;
using System;

namespace RobotAction.Gameplay.Battle
{
    public class BattleFlowController : IDisposable
    {
        private enum BattlePhase
        {
            MobBattle,
            BossBattle,
            Cleared,
            Defeated,
        }

        public event Action OnPlayerDied;
        public event Action OnEnemyDied;
        public event Action OnEnemyTargetCleared;
        public event Action OnBossDied;

        private readonly IPlayerStatus _playerStatus;
        private readonly RandomAreaEnemyGenerator _randomAreaEnemyGenerator;
        private readonly BossController _bossController;
        private readonly int _requiredEnemyKillCount;

        public int CurrentDiedEnemyCount => _currentDiedEnemyCount;

        private BattlePhase _currentPhase;
        private int _currentDiedEnemyCount;

        /// <summary>
        /// PlayerStatus、Generatorからのイベント購読を行い、MobBattleを開始・管理する処理
        /// </summary>
        /// <param name="playerStatus"></param>
        /// <param name="enemyGenerator"></param>
        /// <param name="bossController"></param>
        public BattleFlowController(
            IPlayerStatus playerStatus,
            RandomAreaEnemyGenerator enemyGenerator,
            BossController bossController,
            int requiredEnemyKillCount
        )
        {
            _playerStatus = playerStatus;
            _randomAreaEnemyGenerator = enemyGenerator;
            _bossController = bossController;
            _requiredEnemyKillCount = requiredEnemyKillCount;

            _playerStatus.OnDied += HandlePlayerOnDied;
            _randomAreaEnemyGenerator.OnEnemyDied += HandleEnemyDied;
            _bossController.OnDied += HandleBossDied;

            _bossController.gameObject.SetActive(false);

            _currentPhase = BattlePhase.MobBattle;
        }

        public void Dispose()
        {
            _playerStatus.OnDied -= HandlePlayerOnDied;
            _randomAreaEnemyGenerator.OnEnemyDied -= HandleEnemyDied;
            _bossController.OnDied -= HandleBossDied;
        }

        private void HandlePlayerOnDied()
        {
            OnPlayerDied?.Invoke();
            _currentPhase = BattlePhase.Defeated;
        }

        private void HandleEnemyDied()
        {
            //モブバトルでないときにカウントする必要はないためガード
            if (_currentPhase != BattlePhase.MobBattle) return;

            _currentDiedEnemyCount++;
            OnEnemyDied?.Invoke();

            if(_currentDiedEnemyCount >= _requiredEnemyKillCount)
            {
                OnEnemyTargetCleared?.Invoke();
                _randomAreaEnemyGenerator.DespawnAllEnemy();
                _randomAreaEnemyGenerator.gameObject.SetActive(false);
                _bossController.gameObject.SetActive(true);
                _currentPhase = BattlePhase.BossBattle;
            }
        }

        private void HandleBossDied()
        {
            _bossController.gameObject.SetActive(false);
            OnBossDied?.Invoke();
        }
    }
}
