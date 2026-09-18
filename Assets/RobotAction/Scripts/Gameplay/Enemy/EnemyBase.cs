using NUnit.Framework;
using RobotAction.Gameplay.Combat;
using RobotAction.Gameplay.Player;
using RobotAction.Gameplay.Scriptables;
using UnityEngine;

namespace RobotAction.Gameplay.Enemy
{
    public abstract class EnemyBase : MonoBehaviour, IDamageable
    {
        [SerializeField] protected EnemyData _data;
        [SerializeField] private Blackboard _blackboard;
        private float _currentHealth;
        [SerializeField] private EnemySearchData _searchData;
        private Collider[] _detectedColliders;

        protected virtual void Awake()
        {
            _currentHealth = _data.MaxHealth;
            _detectedColliders = new Collider[_searchData.MaxSearchCount];
            _blackboard = new Blackboard();
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
    }
}
