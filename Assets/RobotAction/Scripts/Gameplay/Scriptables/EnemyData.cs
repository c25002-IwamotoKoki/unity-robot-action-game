using UnityEngine;

namespace RobotAction.Gameplay.Scriptables
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private float _maxHealth;

        public string Name => _name;
        public float MaxHealth => _maxHealth;
    }
}
