using RobotAction.Gameplay.Combat;
using RobotAction.Gameplay.Parts;
using RobotAction.Gameplay.Parts.Weapons;
using UnityEngine;

namespace RobotAction.Gameplay.Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class DefaultEnemy : EnemyBase
    {
        [SerializeField] private WeaponPartsHandler _rightWeaponHandler;
        [SerializeField] private WeaponPartsHandler _leftWeaponHandler;
        [SerializeField] private float _rushAttackSpeed;
        [SerializeField] private float _rushAttackRange;
        [SerializeField] private int _maxRushHitCount;
        [SerializeField] private float _rushRate;
        [SerializeField] private LayerMask _rushHitLayer;

        private Rigidbody _rightBody;
        private Collider[] _hitColliders;
        private float _rushRateTimer;
        private bool _canRush;
        private bool _isWeaponAttack = true;

        protected override void Awake()
        {
            _hitColliders = new Collider[_maxRushHitCount];

            TryGetComponent(out _rightBody);
            base.Awake();
        }

        private void OnEnable()
        {
            _rightWeaponHandler.OnWeaponEquipped += HandleWeaponEquipped;
            _leftWeaponHandler.OnWeaponEquipped += HandleWeaponEquipped;
        }

        protected override void Update()
        {
            if (!_canRush)
            {
                _rushRateTimer += Time.deltaTime;

                if (_rushRateTimer >= _rushRate)
                {
                    _rushRateTimer = 0;
                    _canRush = true;
                }
            }

            base.Update();
        }

        private void OnDisable()
        {
            _rightWeaponHandler.OnWeaponEquipped -= HandleWeaponEquipped;
            _leftWeaponHandler.OnWeaponEquipped -= HandleWeaponEquipped;
        }


        public override void Attack()
        {
            //Targetがいるかどうかは呼ぶ側が確認しているためここではnullチェックを行わない
            if (_isWeaponAttack)
            {
                _rightWeaponHandler.SetTarget(_blackboard.Target.position);
                _leftWeaponHandler.SetTarget(_blackboard.Target.position);

                _rightWeaponHandler.Attack();
                _leftWeaponHandler.Attack();
            }
            else
            {
                RushAttack();
            }
        }

        private void RushAttack()
        {
            if (!_canRush) return;

            Vector3 direction = (_blackboard.Target.position - transform.position).normalized;
            _rightBody.AddForce(direction * _rushAttackSpeed, ForceMode.Impulse);

            int hitCount = Physics.OverlapSphereNonAlloc(
                transform.position,
                _rushAttackRange,
                _hitColliders,
                _rushHitLayer
            );

            for(int i = 0; i < hitCount; i++)
            {
                if (_hitColliders[i].TryGetComponent(out IDamageable damageable))
                {
                    damageable.GetDamage(_data.BaseAttackPower);
                }
            }

            _canRush = false;
        }

        public override void OnGet()
        {
            base.OnGet();
            _rushRateTimer = 0;
            _canRush = true;
        }

        protected override void Died()
        {
            _rightWeaponHandler.Unequip();
            _leftWeaponHandler.Unequip();

            //武器を落としRushAttackに切り替えるため_rushAttackRangeを代入
            _blackboard.AttackRange = _rushAttackRange;
            _isWeaponAttack = false;

            base.Died();
        }

        private void HandleWeaponEquipped(IWeaponPart weapon)
        {
            _blackboard.AttackRange = weapon.AttackRange;
        }
    }
}
