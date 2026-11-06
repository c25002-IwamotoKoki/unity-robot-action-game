using UnityEngine;

namespace RobotAction.Gameplay.Enemy.Boss
{
    [CreateAssetMenu(fileName = "BossData", menuName = "Scriptable Objects/BossData")]
    public class BossData : ScriptableObject
    {
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private float _avoidObstacleDistance;
        [SerializeField] private float _avoidObstacleSpeed;
        [SerializeField] private float _globalAttackInterval;
        [SerializeField] private LayerMask _obstacleLayer;

        public float MaxHealth => _maxHealth;
        public float MoveSpeed => _moveSpeed;
        public float RotateSpeed => _rotateSpeed;
        public float AvoidObstacleDistance => _avoidObstacleDistance;
        public float AvoidObstacleSpeed => _avoidObstacleSpeed;
        public float GlobalAttackInterval => _globalAttackInterval;
        public LayerMask ObstacleLayer => _obstacleLayer;
    }
}
