using RobotAction.Gameplay.Combat;
using UnityEngine;

namespace RobotAction.Gameplay.Enemy.Boss
{
    public sealed class BossRushAttack : BossAttackBase
    {
        public override float AttackRange => _attackRange;

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _rushSpeed;
        [SerializeField] private float _rushDistance;
        [SerializeField] private float _rushDuration;
        [SerializeField] private float _rushCoolTime;
        [SerializeField] private float _hitRange;
        [SerializeField, Range(-1, 1)] private float _hitThreshold;

        private float _attackRange;
        private float _durationTimer;
        private float _rushCoolTimer;
        private bool _isRushing;
        private bool _isRushed;
        private bool _isHit;

        private void Awake()
        {
            _attackRange = _rushDistance * transform.localScale.magnitude;
        }

        private void FixedUpdate()
        {
            if (!_isRushing && _isRushed) return;

            _rigidbody.AddForce(transform.forward * _rushSpeed, ForceMode.Impulse);
        }

        private void Update()
        {
            if (_isRushed)
            {
                _rushCoolTimer += Time.deltaTime;

                if (_rushCoolTimer >= _rushCoolTime)
                {
                    _rushCoolTimer = 0;
                    _isRushed = false;
                }
            }

            if (!_isRushing) return;

            HitProcess();

            _durationTimer -= Time.deltaTime;
            if (_durationTimer < 0)
            {
                _isRushing = false;
                _isHit = false;
                _isRushed = true;
                _durationTimer = 0;
            }
        }

        public override void Execute()
        {
            if (_isRushing || _isRushed) return;

            _isRushing = true;
            _isHit = false;
            _durationTimer = _rushDuration;
        }

        private void HitProcess()
        {
            //命中済みな場合は複数ヒットを避けるためリターン
            if (_isHit) return;

            float toPlayerDistanceSqr = (_player.position - transform.position).sqrMagnitude;

            //攻撃範囲にいないならそもそも検知判定をしない
            if (toPlayerDistanceSqr >= _attackRange * _attackRange) return;

            Vector3 direction = (_player.position - transform.position).normalized;

            float dot = Vector3.Dot(transform.position, direction);

            if (dot >= _hitThreshold)
            {
                _playerDamageable.GetDamage(_baseAttackPower);
                _isHit = true;
            }
        }
    }
}
