using RobotAction.Gameplay.Interfaces;
using UnityEngine;

namespace RobotAction.Gameplay.Enemy
{
    public class DefaultEnemy : EnemyBase
    {
        private IEnemyAttack _attackModule;
        [SerializeField] private Transform _targetTransform;

        protected override void Awake()
        {
            TryGetComponent(out _attackModule);
            base.Awake();
        }

        private void Update()
        {
            var attackContext = new EnemyAttackContext(_targetTransform, 
                                                       _data.BaseAttackPower);
            _attackModule.Attack(attackContext);
        }

    }
}
