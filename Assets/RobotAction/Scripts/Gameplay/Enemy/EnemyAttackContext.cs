using UnityEngine;

namespace RobotAction.Gameplay.Enemy
{
    public readonly struct EnemyAttackContext
    {
        public Transform TargetTransform { get; }
        public float BaseDamage { get; }

        public EnemyAttackContext(Transform target,float baseDamage)
        {
            TargetTransform = target;
            BaseDamage = baseDamage;
        }
    }

}
