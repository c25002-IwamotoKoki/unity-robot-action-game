using UnityEngine;

namespace RobotAction.Gameplay.Enemy
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _baseAttackPower;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotateSpeed;

        public string Name => _name;
        public float MaxHealth => _maxHealth;
        public float BaseAttackPower => _baseAttackPower;
        public float MoveSpeed => _moveSpeed;
        public float RotateSpeed => _rotateSpeed;
    }
}
