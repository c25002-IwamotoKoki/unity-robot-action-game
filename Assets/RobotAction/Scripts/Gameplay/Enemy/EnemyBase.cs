using RobotAction.Gameplay.Combat;
using RobotAction.Gameplay.Player;
using RobotAction.Gameplay.Scriptables;
using UnityEngine;

namespace RobotAction.Gameplay.Enemy
{
    public abstract class EnemyBase : MonoBehaviour, IDamageable
    {
        [SerializeField] protected EnemyData _data;

        [SerializeField] private EnemySearchData _searchData;

        private float _currentHealth;
        private StateMachine _stateMachine;
        private Blackboard _blackboard;
        private PatrolState _patrolState;
        private AttackState _attackState;
        private ChaseState _chaseState;
        private Collider[] _detectedColliders;

        public PatrolState PatrolState => _patrolState;
        public AttackState AttackState => _attackState;
        public ChaseState ChaseState => _chaseState;


        protected virtual void Awake()
        {
            _currentHealth = _data.MaxHealth;
            _detectedColliders = new Collider[_searchData.MaxSearchCount];
            _blackboard = new Blackboard();
            _stateMachine = new StateMachine(this, _blackboard);
            _patrolState = new PatrolState(_stateMachine, this, _blackboard);
            _attackState = new AttackState(_stateMachine,this,_blackboard);
            _chaseState = new ChaseState(_stateMachine,this,_blackboard);

            _stateMachine.Initialize(_patrolState);
        }

        protected virtual void Update()
        {
            _stateMachine.Tick();
        }

        public void GetDamage(float damage)
        {
            _currentHealth -= damage;
        }

        public void SearchNearlyTarget()
        {
            Vector3 sarchPosition = transform.position + _searchData.SearchPositionOffset;

            int detectedCount = Physics.OverlapSphereNonAlloc(sarchPosition,
                                                              _searchData.SearchRange,
                                                              _detectedColliders,
                                                              _searchData.SearchLayer);

            for (int i = 0; i < detectedCount; i++)
            {
                _detectedColliders[i].TryGetComponent(out PlayerController player);
                _blackboard.Target = player.transform;
            }
        }

        public abstract void Attack();

        public void RotateTowards(Vector3 direction)
        {
            if(direction == Vector3.zero)
            {
                return;
            }

            Quaternion lookRotate = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  lookRotate,
                                                  _data.RotateSpeed * Time.deltaTime);
        }

        public void Move(Vector3 direction)
        {
            transform.position += direction * _data.MoveSpeed * Time.deltaTime;
        }
    }
}
