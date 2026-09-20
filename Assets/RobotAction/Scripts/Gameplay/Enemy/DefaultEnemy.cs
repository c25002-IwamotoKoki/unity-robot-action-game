using RobotAction.Gameplay.Parts;
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

        private void OnEnable()
        {
            _rightWeaponHandler.OnWeaponEquipped += HandleWeaponEquipped;
            _leftWeaponHandler.OnWeaponEquipped += HandleWeaponEquipped;
        }

        public override void Attack()
        {
            if(_blackboard.Target != null)
            {
                _rightWeaponHandler.SetTarget(_blackboard.Target.position);
                _leftWeaponHandler.SetTarget(_blackboard.Target.position);
            }
            
            _rightWeaponHandler.Attack();
            _leftWeaponHandler.Attack();
        }

        private void OnDisable()
        {
            _rightWeaponHandler.OnWeaponEquipped -= HandleWeaponEquipped;
            _leftWeaponHandler.OnWeaponEquipped -= HandleWeaponEquipped;
        }

        private void HandleWeaponEquipped(IWeaponPart weapon)
        {
            _blackboard.AttackRange = weapon.AttackRange;
        }

    }
}
