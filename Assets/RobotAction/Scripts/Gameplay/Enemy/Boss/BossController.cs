using RobotAction.Gameplay.Combat;
using RobotAction.Gameplay.Sensors;
using System;
using UnityEngine;

namespace RobotAction.Gameplay.Enemy.Boss
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class BossController : MonoBehaviour, IDamageable, ITargetable
    {
        public event Action OnDied;
        public Transform Transform => transform;

        //ボスがプレイヤーを見失うなどは考えていないためそのままつかんでおきます
        [SerializeField] private Transform _player;
        [SerializeField] private Collider _hitBox;
        [SerializeField] private BossData _data;

        private BossBrain _brain;
        private Rigidbody _rididbody;
        private IDamageable _playerDamageable;
        private float _currentHealth;
        private bool _isDead;

        private void Awake()
        {
            if (!_player.TryGetComponent(out _playerDamageable))
            {
                Debug.LogWarning("インスペクターから指定したPlayerにIDamageableが実装されていません");
            }

            TryGetComponent(out _rididbody);
            _currentHealth = _data.MaxHealth;
        }

        private void Start()
        {
            _brain = new BossBrain(owner: this, _player, _playerDamageable, _data.GlobalAttackInterval);
        }

        private void Update()
        {
            if (_isDead) return;

            _brain.Tick(Time.deltaTime);
        }

        public void GetDamage(float damage)
        {
            _currentHealth -= damage;

            if(_currentHealth <= 0)
            {
                OnDied?.Invoke();
                _isDead = true;
            }
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
                _data.RotateSpeed * Time.deltaTime
            );
        }

        public void MoveForward()
        {
            _rididbody.AddForce(transform.forward * _data.MoveSpeed, ForceMode.Force);
        }

        public void AvoidObstacleProcess()
        {
            Transform hitBoxModel = _hitBox.transform;

            Vector3 localCenter;
            Vector3 localSize;

            //正確なローカルサイズで判定を行うため
            if (_hitBox is BoxCollider box)
            {
                localCenter = box.center;
                localSize =  box.size;
            }
            else
            {
                localCenter = Vector3.zero;
                localSize = Vector3.one;
            }

            //centerからhalfでRayの飛ばす位置をコライダーの外周にするため
            float halfWidth = localSize.x * 0.5f;
            float halfDepth = localSize.z * 0.5f;

            Vector3 rightBackLocal = localCenter + new Vector3(halfWidth,0f,-halfDepth );
            Vector3 leftBackLocal = localCenter + new Vector3(-halfWidth, 0f, -halfDepth);

            Vector3 rightEdge = hitBoxModel.TransformPoint(rightBackLocal);
            Vector3 leftEdge = hitBoxModel.TransformPoint(leftBackLocal);

            //右側の正面が障害物に引っかかっているなら左に回避
            if (Physics.Raycast(rightEdge, hitBoxModel.forward, _data.AvoidObstacleDistance, _data.ObstacleLayer))
            {
                Vector3 avoidOffset = _data.AvoidObstacleSpeed * Time.deltaTime * (-hitBoxModel.right + -hitBoxModel.forward);
                _rididbody.MovePosition(_rididbody.position + avoidOffset);
                return;
            }

            //上記処理の逆
            if (Physics.Raycast(leftEdge, hitBoxModel.forward, _data.AvoidObstacleDistance, _data.ObstacleLayer))
            {
                Vector3 avoidOffset = _data.AvoidObstacleSpeed * Time.deltaTime * (-hitBoxModel.right + -hitBoxModel.forward);
                _rididbody.MovePosition(_rididbody.position + avoidOffset);
            }
        }
    }
}
