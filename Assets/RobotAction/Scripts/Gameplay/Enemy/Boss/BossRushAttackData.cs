using UnityEngine;

namespace RobotAction.Gameplay.Enemy.Boss
{
    [CreateAssetMenu(fileName = "BossRushAttackData", menuName = "Scriptable Objects/BossRushAttackData")]
    public class BossRushAttackData : ScriptableObject
    {
        [SerializeField] private float _baseAttackPower;
        [SerializeField] private float _rushSpeed;
        [SerializeField] private float _rushDistance;
        [SerializeField] private float _rushDuration;
        [SerializeField] private float _rushCoolTime;
        [SerializeField] private float _hitRangeOffset;
        [SerializeField, Range(-1, 1)] private float _hitThreshold;

        public float BaseAttackPower => _baseAttackPower;
        public float RushSpeed => _rushSpeed;
        public float RushDistance => _rushDistance;
        public float RushDuration => _rushDuration;
        public float RushCoolTime => _rushCoolTime;
        public float HitRangeOffset => _hitRangeOffset;
        public float HitThreshold => _hitThreshold;
    }
}
