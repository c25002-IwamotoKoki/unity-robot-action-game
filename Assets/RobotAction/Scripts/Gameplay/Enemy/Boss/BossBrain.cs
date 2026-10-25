using RobotAction.Gameplay.Combat;
using UnityEngine;

namespace RobotAction.Gameplay.Enemy.Boss
{
    public enum BossState
    {
        Chase,
        Attack,
    }

    public class BossBrain
    {
        private readonly BossController _owner;
        private readonly BossAttackBase[] _attacks;
        private readonly Transform _player;
        private readonly IDamageable _playerDamageable;
        private readonly float _globalAttackInterval;

        private BossAttackBase _currentAttack;
        private BossState _currentState;
        private float _globalAttackTimer;

        public BossState CurrentState => _currentState;

        public BossBrain(BossController owner, Transform player, IDamageable playerDamageable, float globalAttackInterval)
        {
            _owner = owner;
            _attacks = _owner.GetComponentsInChildren<BossAttackBase>();
            _player = player;
            _playerDamageable = playerDamageable;
            _globalAttackInterval = globalAttackInterval;

            foreach (var attack in _attacks)
            {
                attack.SetPlayer(_player, _playerDamageable);
            }
        }

        public void Tick(float deltaTime)
        {
            if (deltaTime == 0) return;

            //AttackStateであること自体が_currentAttackを保持している証拠なためnullチェックはしない
            if (_currentState == BossState.Attack)
            {
                //攻撃中に別の行動で上書きされることを防止
                if (_currentAttack.IsExecuting) return;

                _globalAttackTimer = 0f;
                _currentState = BossState.Chase;
            }

            _globalAttackTimer += deltaTime;

            if (_globalAttackTimer >= _globalAttackInterval && TrySetAttack())
            {
                _currentState = BossState.Attack;
                _currentAttack.Execute();
                return;
            }

            //ここまで処理が回ってきているなら追いかける処理を行う
            _owner.RotateTwordsToPlayer();
            _owner.MoveForward();
        }

        private bool TrySetAttack()
        {
            float distanceSqr = (_player.position - _owner.transform.position).sqrMagnitude;

            foreach (var attack in _attacks)
            {
                if (attack.CanAttack(distanceSqr))
                {
                    _currentAttack = attack;
                    return true;
                }
            }

            //全て攻撃範囲外ならnullを代入
            _currentAttack = null;
            return false;
        }
    }
}
