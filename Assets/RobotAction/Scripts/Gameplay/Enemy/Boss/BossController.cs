using RobotAction.Gameplay.Combat;
using RobotAction.Gameplay.Sensors;
using UnityEngine;

namespace RobotAction.Gameplay.Enemy.Boss
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class BossController : MonoBehaviour, IDamageable, ITargetable
    {
        public Transform Transform => transform;

        //ボスがプレイヤーを見失うなどは考えていないためそのままつかんでおきます
        [SerializeField] private Transform _player;
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private float _globalAttackInterval;
        [SerializeField] private BossState _currentState;

        private BossBrain _brain; 
        private Rigidbody _rididbody;
        private IDamageable _playerDamageable;
        private float _currentHealth;

        private void Awake()
        {
            if (!_player.TryGetComponent(out _playerDamageable))
            {
                Debug.LogWarning("インスペクターから指定したPlayerにIDamageableが実装されていません");
            }

            TryGetComponent(out _rididbody);
            _currentHealth = _maxHealth;
        }

        private void Start()
        {
            _brain = new BossBrain(owner: this, _player, _playerDamageable, _globalAttackInterval);
        }

        private void Update()
        {
            _brain.Tick(Time.deltaTime);
            _currentState = _brain.CurrentState;
        }

        public void GetDamage(float damage)
        {
            _currentHealth -= damage;
        }

        public void RotateTwordsToPlayer()
        {
            Vector3 direction = (_player.position - transform.position).normalized;

            Quaternion lookDirection = Quaternion.LookRotation(direction);

            lookDirection.x = 0;
            lookDirection.z = 0;

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                lookDirection,
                _rotateSpeed * Time.deltaTime
            );
        }

        public void MoveForward()
        {
            _rididbody.AddForce(transform.forward * _moveSpeed,ForceMode.Force);
        }
    }
}
