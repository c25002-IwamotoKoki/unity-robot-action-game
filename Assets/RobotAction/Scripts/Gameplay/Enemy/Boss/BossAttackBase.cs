using RobotAction.Gameplay.Combat;
using UnityEngine;

namespace RobotAction.Gameplay.Enemy.Boss
{
    public abstract class BossAttackBase : MonoBehaviour
    {
        public abstract float AttackRange { get; }

        [SerializeField] protected float _baseAttackPower;

        protected Transform _player;
        protected IDamageable _playerDamageable;

        public virtual void SetPlayer(Transform player,IDamageable damageable)
        {
            _player = player;
            _playerDamageable = damageable;
        }

        public virtual bool CanAttack(float distanceSqr)
        {
            return distanceSqr <= AttackRange * AttackRange;
        }

        public abstract void Execute();
    }
}
