using RobotAction.Gameplay.Interfaces;
using RobotAction.Gameplay.Scriptables;
using UnityEngine;

namespace RobotAction.Gameplay.Enemy
{
    public abstract class EnemyBase : MonoBehaviour,IDamageable
    {
        [SerializeField] private EnemyData _data;
        private float _currentHealth;

        protected virtual void Awake()
        {
            _currentHealth = _data.MaxHealth;
        }

        public void GetDamage(float damage)
        {
            
        }
    }
}
