using UnityEngine;

namespace RobotAction.Gameplay.Enemy
{
    public class Blackboard
    {
        public bool IsThinking { get; set; }
        public float ThinkInterval { get; set; } = 0.5f;

        public Transform Target { get; set; }
    }
}
