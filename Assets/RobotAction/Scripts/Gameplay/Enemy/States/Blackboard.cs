using UnityEngine;

namespace RobotAction.Gameplay.Enemy
{
    public class Blackboard
    {
        public float PeriodicTickInterval { get; set; } = 0.5f;
        public float AttackRange { get; set; }
        public float AttackRangeSqr => AttackRange * AttackRange;

        public Transform Target { get; set; }
    }
}
