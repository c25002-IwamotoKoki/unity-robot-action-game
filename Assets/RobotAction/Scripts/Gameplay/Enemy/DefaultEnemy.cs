using RobotAction.Gameplay.Parts.Weapons;
using UnityEngine;

namespace RobotAction.Gameplay.Enemy
{
    public class DefaultEnemy : EnemyBase
    {
        [SerializeField] private WeaponPartsHandler _rightWeaponHandler;
        [SerializeField] private WeaponPartsHandler _leftWeaponHandler;

        protected override void Awake()
        {
            base.Awake();
        }

        public override void Attack()
        {
            _rightWeaponHandler.Attack();
            _leftWeaponHandler.Attack();
        }

    }
}
