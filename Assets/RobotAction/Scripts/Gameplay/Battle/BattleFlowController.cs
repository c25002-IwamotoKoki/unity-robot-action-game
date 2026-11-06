using RobotAction.Core;
using RobotAction.Gameplay.Enemy;
using System;

namespace RobotAction.Gameplay.Battle
{
    public class BattleFlowController : IDisposable
    {
        private enum BattlePhase
        {
            MobBattle,
            Cleared,
            Defeated,
        }

        public event Action OnPlayerDied;
        public event Action OnEnemyDied;
        public event Action OnEnemyTargetCleared;

        private readonly IPlayerStatus _playerStatus;
        private readonly RandomAreaEnemyGenerator _randomAreaEnemyGenerator;
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
            int requiredEnemyKillCount
        )
        {
            _playerStatus = playerStatus;
            _randomAreaEnemyGenerator = enemyGenerator;
            _requiredEnemyKillCount = requiredEnemyKillCount;

            _playerStatus.OnDied += HandlePlayerOnDied;
            _randomAreaEnemyGenerator.OnEnemyDied += HandleEnemyDied;      

            _currentPhase = BattlePhase.MobBattle;
        }

        public void Dispose()
        {
            _playerStatus.OnDied -= HandlePlayerOnDied;
            _randomAreaEnemyGenerator.OnEnemyDied -= HandleEnemyDied;
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
                _currentPhase = BattlePhase.Cleared;
            }
        }
    }
}
