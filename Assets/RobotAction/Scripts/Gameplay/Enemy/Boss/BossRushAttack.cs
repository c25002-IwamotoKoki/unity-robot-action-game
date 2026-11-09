using UnityEngine;

namespace RobotAction.Gameplay.Enemy.Boss
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class BossRushAttack : BossAttackBase
    {
        public override float AttackRange => _attackRange;

        [SerializeField] private BossRushAttackData _data;
        [Header("突進攻撃の攻撃判定を決める際にモデル(見た目)の大きさが必要なため")]
        [SerializeField] private Transform _modelTransform;

        private Rigidbody _rigidbody;
        private float _attackRange;
        private float _hitRange;
        private float _durationTimer;
        private float _rushCoolTimer;
        private bool _isRushing;
        private bool _isRushed;
        private bool _isHit;

        private void Awake()
        {
            TryGetComponent(out _rigidbody);

            //何度もTransformにアクセスするコストを削減
            Vector3 localScale = _modelTransform.localScale;
            float scaleAverage = (localScale.x + localScale.y + localScale.z) / 3;

            _attackRange = (_data.HitRangeOffset + scaleAverage) * _data.RushDistance;
            _hitRange = _data.HitRangeOffset + scaleAverage;
        }

        private void FixedUpdate()
        {
            if (!_isRushing || _isRushed) return;

            _rigidbody.AddForce(transform.forward * _data.RushDistance, ForceMode.Impulse);
        }

        private void Update()
        {
            if (_isRushed)
            {
                _rushCoolTimer += Time.deltaTime;

                if (_rushCoolTimer >= _data.RushCoolTime)
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
                IsExecuting = false;
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
            IsExecuting = true;
            _isHit = false;
            _durationTimer = _data.RushDuration;
        }

        private void HitProcess()
        {
            //命中済みな場合は複数ヒットを避けるためリターン
            if (_isHit) return;

            float toPlayerDistanceSqr = (_player.position - transform.position).sqrMagnitude;

            //攻撃範囲にいないならそもそも検知判定をしない
            if (toPlayerDistanceSqr >= _hitRange * _hitRange / _data.RushDistance) return;

            Vector3 direction = (_player.position - transform.position).normalized;

            float dot = Vector3.Dot(transform.forward, direction);

            if (dot >= _data.HitThreshold)
            {
                _playerDamageable.GetDamage(_data.BaseAttackPower);
                _isHit = true;
            }
        }
    }
}
